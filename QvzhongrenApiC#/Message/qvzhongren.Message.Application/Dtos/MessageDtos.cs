namespace qvzhongren.Message.Application.Dtos;

public class SendMessageDto
{
    public string SenderId { get; set; } = "";
    public string ReceiverId { get; set; } = "";
    public string Content { get; set; } = "";
    public string? MsgType { get; set; } = "text";
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
}

public class GetMessagesDto
{
    public string UserId { get; set; } = "";
    public string OtherUserId { get; set; } = "";
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class MarkReadDto
{
    public string UserId { get; set; } = "";
    public string OtherUserId { get; set; } = "";
}

public class ConversationDto
{
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string LastMessage { get; set; } = "";
    public DateTime LastTime { get; set; }
    public int UnreadCount { get; set; }
}

public class MessageItemDto
{
    public string Id { get; set; } = "";
    public string SenderId { get; set; } = "";
    public string ReceiverId { get; set; } = "";
    public string Content { get; set; } = "";
    public string MsgType { get; set; } = "text";
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string IsRead { get; set; } = "0";
    public DateTime SendTime { get; set; }
    public bool IsMine { get; set; }
}

