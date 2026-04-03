using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos

{
    public class QueryDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary> 
        public DateTime? EndDateTime { get; set; }

    }
}
