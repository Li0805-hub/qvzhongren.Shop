using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 菜单表
/// </summary>
[SugarTable("SYS_MENU")]
public class SysMenu : BaseAuditEntity
{
    /// <summary>菜单ID</summary>
    [SugarColumn(ColumnName = "MENU_ID", IsPrimaryKey = true)]
    public string MenuId { get; set; }

    /// <summary>菜单名称</summary>
    [SugarColumn(ColumnName = "MENU_NAME")]
    public string MenuName { get; set; }

    /// <summary>父级菜单ID（顶级为0）</summary>
    [SugarColumn(ColumnName = "PARENT_ID")]
    public string ParentId { get; set; } = "0";

    /// <summary>路由路径</summary>
    [SugarColumn(ColumnName = "PATH")]
    public string? Path { get; set; }

    /// <summary>组件路径</summary>
    [SugarColumn(ColumnName = "COMPONENT")]
    public string? Component { get; set; }

    /// <summary>图标</summary>
    [SugarColumn(ColumnName = "ICON")]
    public string? Icon { get; set; }

    /// <summary>菜单类型（M=目录 C=菜单 F=按钮）</summary>
    [SugarColumn(ColumnName = "MENU_TYPE")]
    public string MenuType { get; set; } = "C";

    /// <summary>权限标识</summary>
    [SugarColumn(ColumnName = "PERMS")]
    public string? Perms { get; set; }

    /// <summary>排序</summary>
    [SugarColumn(ColumnName = "SORT_NO")]
    public int? SortNo { get; set; }

    /// <summary>状态（1=显示 0=隐藏）</summary>
    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";
}
