using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace qvzhongren.Message.Application.Hubs;

public class ChatHub : Hub
{
    // userId -> set of connectionIds
    private static readonly ConcurrentDictionary<string, HashSet<string>> OnlineUsers = new();

    public async Task Register(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);

        OnlineUsers.AddOrUpdate(userId,
            _ => new HashSet<string> { Context.ConnectionId },
            (_, set) => { lock (set) { set.Add(Context.ConnectionId); } return set; });

        await Clients.All.SendAsync("UserOnline", userId);
    }

    /// <summary>通知对方正在输入</summary>
    public async Task Typing(string toUserId)
    {
        await Clients.Group(toUserId).SendAsync("UserTyping", Context.ConnectionId);
    }

    /// <summary>停止输入</summary>
    public async Task StopTyping(string toUserId)
    {
        await Clients.Group(toUserId).SendAsync("UserStopTyping", Context.ConnectionId);
    }

    /// <summary>获取当前所有在线用户ID</summary>
    public Task<List<string>> GetOnlineUsers()
    {
        return Task.FromResult(OnlineUsers.Where(kv =>
        {
            lock (kv.Value) { return kv.Value.Count > 0; }
        }).Select(kv => kv.Key).ToList());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string? disconnectedUserId = null;
        foreach (var kv in OnlineUsers)
        {
            lock (kv.Value)
            {
                if (kv.Value.Remove(Context.ConnectionId) && kv.Value.Count == 0)
                {
                    disconnectedUserId = kv.Key;
                }
            }
        }

        if (disconnectedUserId != null)
        {
            OnlineUsers.TryRemove(disconnectedUserId, out _);
            await Clients.All.SendAsync("UserOffline", disconnectedUserId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
