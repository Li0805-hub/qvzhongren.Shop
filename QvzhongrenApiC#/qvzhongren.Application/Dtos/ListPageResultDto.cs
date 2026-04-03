namespace qvzhongren.Application.Dtos
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public class ListPageResultDto<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 当页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Values { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ListPageResultDto()
        {
            Values = new List<T>();
        }
    }
}