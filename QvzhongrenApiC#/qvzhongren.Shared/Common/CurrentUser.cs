using System;
using System.Collections.Generic;

namespace qvzhongren.Shared.Common
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 科室编码
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 医院编码
        /// </summary>
        public string HosUnitCode { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 用户额外信息（可以存储任何键值对）
        /// </summary>
        public Dictionary<string, string> AdditionalData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; }
    }
} 