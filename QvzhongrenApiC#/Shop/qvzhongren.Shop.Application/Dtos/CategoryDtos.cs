namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 分类响应DTO
    /// </summary>
    public class CategoryResponseDto
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 父级分类ID
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类图标URL
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// 状态（1=启用，0=禁用）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 子分类列表
        /// </summary>
        public List<CategoryResponseDto>? Children { get; set; }
    }

    /// <summary>
    /// 分类创建/更新DTO
    /// </summary>
    public class CategoryCreateDto
    {
        /// <summary>
        /// 分类ID（更新时传入）
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// 父级分类ID
        /// </summary>
        public string? ParentId { get; set; } = "0";

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类图标URL
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// 状态（1=启用，0=禁用）
        /// </summary>
        public string Status { get; set; } = "1";
    }
}
