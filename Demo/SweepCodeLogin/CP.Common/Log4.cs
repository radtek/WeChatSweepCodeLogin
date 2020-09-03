using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Common
{
    public class Log4
    {
        /// <summary>
        /// 使用log4 记录错误日志 日志将以 txt文档存储
        /// </summary>
        /// <param name="msg"></param>
        public static void Log4Error(string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Error(msg);
        }

        /// <summary>
        /// 使用log4 记录日志 日志将以 txt文档存储
        /// </summary>
        /// <param name="msg"></param>
        public static void Log4Info(string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Info(msg);
        } 
    }
}
