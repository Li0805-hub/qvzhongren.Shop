using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 商品分类
    /// </summary>
    [SugarTable("SHOP_CATEGORY")]
    public class ShopCategory : BaseAuditEntity
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [SugarColumn(ColumnName = "CATEGORY_ID", IsPrimaryKey = true)]
        public string CategoryId { get; set; }

        /// <summary>
        /// 父级分类ID
        /// </summary>
        [SugarColumn(ColumnName = "PARENT_ID", IsNullable = true)]
        public string? ParentId { get; set; } = "0";

        /// <summary>
        /// 分类名称
        /// </summary>
        [SugarColumn(ColumnName = "CATEGORY_NAME")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类图标URL
        /// </summary>
        [SugarColumn(ColumnName = "ICON", IsNullable = true)]
        public string? Icon { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [SugarColumn(ColumnName = "SORT_NO", IsNullable = true)]
        public int? SortNo { get; set; }

        /// <summary>
        /// 状态（1=启用，0=禁用）
        /// </summary>
        [SugarColumn(ColumnName = "STATUS")]
        public string Status { get; set; } = "1";
    }
}
