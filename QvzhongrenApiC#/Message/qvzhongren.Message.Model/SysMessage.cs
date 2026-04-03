using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Message.Model;

[SugarTable("SYS_MESSAGE")]
public class SysMessage : BaseEntity
{
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public string Id { get; set; }

    [SugarColumn(ColumnName = "SENDER_ID")]
    public string SenderId { get; set; }

    [SugarColumn(ColumnName = "RECEIVER_ID")]
    public string ReceiverId { get; set; }

    [SugarColumn(ColumnName = "CONTENT", ColumnDataType = "text")]
    public string Content { get; set; }

    [SugarColumn(ColumnName = "IS_READ")]
    public string IsRead { get; set; } = "0";

    [SugarColumn(ColumnName = "SEND_TIME")]
    public DateTime SendTime { get; set; }

    /// <summary>消息类型: text, image, file</summary>
    [SugarColumn(ColumnName = "MSG_TYPE")]
    public string MsgType { get; set; } = "text";

    [SugarColumn(ColumnName = "FILE_NAME", IsNullable = true)]
    public string? FileName { get; set; }

    [SugarColumn(ColumnName = "FILE_URL", IsNullable = true)]
    public string? FileUrl { get; set; }
}
