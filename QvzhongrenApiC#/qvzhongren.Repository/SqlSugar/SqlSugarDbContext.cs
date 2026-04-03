using SqlSugar;
namespace qvzhongren.Repository.SqlSugar
{
    public class SqlSugarDbContext
    {
        public ISqlSugarClient Db { get; }

        public SqlSugarDbContext(ISqlSugarClient sqlSugarClient)
        {
            Db = sqlSugarClient;

        }
    }
}
