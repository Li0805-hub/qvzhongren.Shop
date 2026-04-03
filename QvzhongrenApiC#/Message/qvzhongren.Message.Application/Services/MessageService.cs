using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Contracts.Clients;
using qvzhongren.Contracts.Dtos;
using qvzhongren.Message.Application.Dtos;
using qvzhongren.Message.Application.Hubs;
using qvzhongren.Message.Model;
using SqlSugar;

namespace qvzhongren.Message.Application.Services;

/// <summary>
/// 消息服务
/// </summary>
public class MessageService : BaseService
{
    private readonly ISqlSugarClient _db;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IPermissionApiClient _permissionApi;

    public MessageService(ISqlSugarClient db, IHubContext<ChatHub> chatHub, IPermissionApiClient permissionApi)
    {
        _db = db;
        _chatHub = chatHub;
        _permissionApi = permissionApi;
    }

    /// <summary>发送消息</summary>
    [HttpPost("Send")]
    public async Task<ResultDto<bool>> SendAsync([FromBody] SendMessageDto dto)
    {
        try
        {
            var msg = new SysMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                MsgType = dto.MsgType ?? "text",
                FileName = dto.FileName,
                FileUrl = dto.FileUrl,
                IsRead = "0",
                SendTime = DateTime.Now,
            };
            await _db.Insertable(msg).ExecuteCommandAsync();

            var pushData = new
            {
                id = msg.Id,
                senderId = msg.SenderId,
                receiverId = msg.ReceiverId,
                content = msg.Content,
                msgType = msg.MsgType,
                fileName = msg.FileName,
                fileUrl = msg.FileUrl,
                isRead = msg.IsRead,
                sendTime = msg.SendTime,
            };
            await _chatHub.Clients.Group(dto.ReceiverId).SendAsync("ReceiveMessage", pushData);
            await _chatHub.Clients.Group(dto.SenderId).SendAsync("ReceiveMessage", pushData);

            return ResultDto<bool>.Success(true, "发送成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"发送失败: {ex.Message}");
        }
    }

    /// <summary>获取会话列表（每个聊天对象的最后一条消息+未读数）</summary>
    [HttpPost("GetConversations")]
    public async Task<ResultDto<List<ConversationDto>>> GetConversationsAsync([FromBody] string userId)
    {
        try
        {
            var messages = await _db.Queryable<SysMessage>()
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SendTime)
                .ToListAsync();

            var users = await _permissionApi.GetAllUsersAsync();
            var userMap = users.ToDictionary(u => u.UserId, u => u.RealName ?? u.UserName);

            var conversations = new Dictionary<string, ConversationDto>();
            foreach (var msg in messages)
            {
                var partnerId = msg.SenderId == userId ? msg.ReceiverId : msg.SenderId;
                if (!conversations.ContainsKey(partnerId))
                {
                    var unread = messages.Count(m => m.SenderId == partnerId && m.ReceiverId == userId && m.IsRead == "0");
                    conversations[partnerId] = new ConversationDto
                    {
                        UserId = partnerId,
                        UserName = userMap.GetValueOrDefault(partnerId, partnerId),
                        LastMessage = msg.Content,
                        LastTime = msg.SendTime,
                        UnreadCount = unread,
                    };
                }
            }

            var result = conversations.Values.OrderByDescending(c => c.LastTime).ToList();
            return ResultDto<List<ConversationDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<ConversationDto>>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>获取与某用户的聊天记录</summary>
    [HttpPost("GetMessages")]
    public async Task<ResultDto<ListPageResultDto<MessageItemDto>>> GetMessagesAsync([FromBody] GetMessagesDto dto)
    {
        try
        {
            var pageIndex = dto.PageIndex < 1 ? 1 : dto.PageIndex;
            var pageSize = dto.PageSize < 1 ? 50 : dto.PageSize;

            var total = new RefAsync<int>();
            var items = await _db.Queryable<SysMessage>()
                .Where(m =>
                    (m.SenderId == dto.UserId && m.ReceiverId == dto.OtherUserId) ||
                    (m.SenderId == dto.OtherUserId && m.ReceiverId == dto.UserId))
                .OrderByDescending(m => m.SendTime)
                .ToPageListAsync(pageIndex, pageSize, total);

            var result = items.Select(m => new MessageItemDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                MsgType = m.MsgType ?? "text",
                FileName = m.FileName,
                FileUrl = m.FileUrl,
                IsRead = m.IsRead,
                SendTime = m.SendTime,
                IsMine = m.SenderId == dto.UserId,
            }).Reverse().ToList();

            return ResultDto<ListPageResultDto<MessageItemDto>>.Success(new ListPageResultDto<MessageItemDto>
            {
                TotalCount = total.Value,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Values = result,
            });
        }
        catch (Exception ex)
        {
            return ResultDto<ListPageResultDto<MessageItemDto>>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>标记与某用户的消息为已读</summary>
    [HttpPost("MarkRead")]
    public async Task<ResultDto<bool>> MarkReadAsync([FromBody] MarkReadDto dto)
    {
        try
        {
            await _db.Updateable<SysMessage>()
                .SetColumns(m => m.IsRead == "1")
                .Where(m => m.SenderId == dto.OtherUserId && m.ReceiverId == dto.UserId && m.IsRead == "0")
                .ExecuteCommandAsync();
            return ResultDto<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"操作失败: {ex.Message}");
        }
    }

    /// <summary>获取未读消息总数</summary>
    [HttpPost("GetUnreadCount")]
    public async Task<ResultDto<int>> GetUnreadCountAsync([FromBody] string userId)
    {
        try
        {
            var count = await _db.Queryable<SysMessage>()
                .Where(m => m.ReceiverId == userId && m.IsRead == "0")
                .CountAsync();
            return ResultDto<int>.Success(count);
        }
        catch (Exception ex)
        {
            return ResultDto<int>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>获取所有可发送消息的用户列表（排除自己）</summary>
    [HttpPost("GetUserList")]
    public async Task<ResultDto<List<SimpleUserDto>>> GetUserListAsync([FromBody] string userId)
    {
        try
        {
            var users = await _permissionApi.GetActiveUsersAsync(userId);
            return ResultDto<List<SimpleUserDto>>.Success(users);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SimpleUserDto>>.Error($"查询失败: {ex.Message}");
        }
    }
}
