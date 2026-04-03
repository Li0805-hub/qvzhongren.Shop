using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Platform.Model;

/// <summary>
/// 服务配置表 — 存储各微服务的地址信息
/// </summary>
[SugarTable("SYS_SERVICE_CONFIG")]
public class SysServiceConfig : BaseAuditEntity
{
    [SugarColumn(ColumnName = "CONFIG_ID", IsPrimaryKey = true)]
    public string ConfigId { get; set; }

    /// <summary>服务名称，如 PermissionService, GatewayService</summary>
    [SugarColumn(ColumnName = "SERVICE_NAME")]
    public string ServiceName { get; set; }

    /// <summary>服务地址，如 http://localhost:5001</summary>
    [SugarColumn(ColumnName = "SERVICE_URL")]
    public string ServiceUrl { get; set; }

    /// <summary>描述</summary>
    [SugarColumn(ColumnName = "DESCRIPTION", IsNullable = true)]
    public string? Description { get; set; }

    /// <summary>状态 1=启用 0=禁用</summary>
    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";

    /// <summary>排序</summary>
    [SugarColumn(ColumnName = "SORT_NO")]
    public int? SortNo { get; set; }
}
