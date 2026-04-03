using System;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using qvzhongren.Shared.Helper;

namespace qvzhongren.Repository.Elasticsearch
{
    /// <summary>
    /// Elasticsearch数据库上下文
    /// </summary>
    public class ElasticsearchDbContext<T> : IDisposable
    {
        private readonly ElasticsearchClient _client;
        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="InvalidOperationException">初始化失败时抛出</exception>
        public ElasticsearchDbContext()
        {
            try
            {
                var settings = new ElasticsearchClientSettings(new Uri(AppSettings.App(["Elasticsearch", "ConnectionString"])))
                    .EnableDebugMode()
                    .DefaultIndex(typeof(T).Name.ToLower())
                    .PrettyJson()
                    .RequestTimeout(TimeSpan.FromSeconds(30)); // 设置默认ID字段

                _client = new ElasticsearchClient(settings);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("初始化Elasticsearch客户端失败", ex);
            }
        }

        /// <summary>
        /// 写入文档
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="id">文档ID,可选</param>
        /// <returns>写入结果</returns>
        public async Task<IndexResponse> IndexAsync(T document, string id = null)
        {

            try
            {
                //var indexName = typeof(T).Name.ToLower();
                //var existsResponse = await _client.Indices.ExistsAsync(indexName);
                //if (!existsResponse.Exists)
                //{
                //    var createIndexResponse = await _client.Indices.CreateAsync(indexName);
                //    if (!createIndexResponse.IsValidResponse)
                //    {
                //        throw new InvalidOperationException($"创建索引失败: {createIndexResponse.DebugInformation}");
                //    }
                //}
                if (string.IsNullOrEmpty(id))
                {
                    return await _client.IndexAsync(document);
                }

                var indexResponse = await _client.IndexAsync(document, idx => idx.Id(id));
                return indexResponse;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("写入文档失败", ex);
            }
        }

        /// <summary>
        /// 读取所有文档
        /// </summary>
        /// <returns>文档对象列表</returns>
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                // 搜索文档
                var searchResponse = await _client.SearchAsync<T>(s => s
                    .Query(q => q
                        .MatchAll(m => m
                            .Boost(1.0f)
                        )
                    )
                );
                if (!searchResponse.IsValidResponse)
                {
                    throw new InvalidOperationException($"读取文档失败: {searchResponse.DebugInformation}");
                }

                return searchResponse.Documents.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("读取文档失败", ex);
            }
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        public ElasticsearchClient Client
        {
            get
            {
                return _client;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                (_client as IDisposable)?.Dispose();
            }

            _disposed = true;
        }

        ~ElasticsearchDbContext()
        {
            Dispose(false);
        }
    }
}
