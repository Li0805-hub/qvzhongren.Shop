namespace qvzhongren.Application.Dtos
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    public class QueryPageDto: QueryDto
    {
        /// <summary>
        /// 页码，从1开始，
        /// </summary>
        public int PageIndex
        {
            get; set;
        }

        /// <summary>
        /// 每页大小，最小10，最大100
        /// </summary>
        public int PageSize
        {
            get; set;
        }
    }
}