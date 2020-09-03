using System.Collections.Generic;

/// <summary>
/// 接口对象
/// </summary>
namespace CP.Entiry
{
    public class Interface
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, string> Dic { get; set; }
    }
}
