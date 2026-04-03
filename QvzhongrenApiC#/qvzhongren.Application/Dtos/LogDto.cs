using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos
{
    /// <summary>
    /// 日志数据传输对象
    /// </summary>
    public class LogDto
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 日志名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
