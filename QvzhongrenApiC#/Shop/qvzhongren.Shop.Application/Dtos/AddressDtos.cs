namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 收货地址响应DTO
    /// </summary>
    public class AddressResponseDto
    {
        /// <summary>
        /// 地址ID
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区/县
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 是否默认地址（1=默认，0=非默认）
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// 创建人编码
        /// </summary>
        public string CreateCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// 收货地址创建/更新DTO
    /// </summary>
    public class AddressCreateDto
    {
        /// <summary>
        /// 地址ID（更新时传入）
        /// </summary>
        public string? AddressId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区/县
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 是否默认地址（1=默认，0=非默认）
        /// </summary>
        public string IsDefault { get; set; } = "0";
    }
}
