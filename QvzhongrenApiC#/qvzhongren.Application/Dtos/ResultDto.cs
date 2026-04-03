namespace qvzhongren.Application.Dtos

{
    /// <summary>
    /// 通用返回结果类
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回代码
        /// 200：成功
        /// 400：客户端错误
        /// 500：服务器错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        /// <returns>结果对象</returns>
        public static ResultDto<T> Success(T data = default, string message = "操作成功", int code = 200)
        {
            return new ResultDto<T>
            {
                IsSuccess = code == 200,
                Code = code,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>结果对象</returns>
        public static ResultDto<T> Error(string message = "操作失败", int code = 500)
        {
            return new ResultDto<T>
            {
                IsSuccess = false,
                Code = code,
                Message = message,
                Data = default
            };
        }

        /// <summary>
        /// 创建客户端错误结果
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>结果对象</returns>
        public static ResultDto<T> BadRequest(string message = "请求参数错误")
        {
            return new ResultDto<T>
            {
                IsSuccess = false,
                Code = 400,
                Message = message,
                Data = default
            };
        }
    }

}
