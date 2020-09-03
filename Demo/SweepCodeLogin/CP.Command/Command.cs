using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CP.Command
{
    public class Command
    {
        public bool Run(string command, out string msg, out DataTable dt, out int affected)
        {

            try
            {
                string _command = string.Empty;

                #region 参数校验
                if (string.IsNullOrEmpty(command))
                {
                    msg = "请传入指令。";
                    dt = null;
                    affected = 0;
                    return false;
                }
                string[] array = command.Split(' ');
                if (array.Length <= 0)
                {
                    msg = "指令格式无效，无法获取到指令名称。";
                    dt = null;
                    affected = 0;
                    return false;
                }
                #endregion

                _command = array[0];

                List<string> _list = new List<string>();

                for (int i = 1; i < array.Length; i++)
                    _list.Add(array[i]);

                string[] _array = _list.ToArray();

                #region 执行命令

                Assembly s = Assembly.Load("CP.Command");
                Type tpe = s.GetType("CP.Command.Command");
                //调用GetName方法
                MethodInfo method = tpe.GetMethod(_command, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Public);

                if (method == null)
                {
                    msg = "无效的指令";
                    dt = null;
                    affected = 0;
                    return false;
                }

                string _msg = string.Empty;

                DataTable _dt = new DataTable();
                int _affected = 0;

                //设置参数
                object[] invokeArgs = new object[] { _array, _msg, _dt, _affected };
                //接受返回值
                object result = method.Invoke(this, invokeArgs);
                // 接受out 引用参数返回值
                msg = invokeArgs[1].ToString();
                dt = (DataTable)invokeArgs[2];
                affected = Convert.ToInt32(invokeArgs[3]);

                if (result.GetType() == typeof(Boolean))
                {
                    bool _result = (bool)result;

                    if (string.IsNullOrEmpty(msg))
                    {
                        if (_result)
                            msg = "执行成功:";
                        else
                            msg = "执行失败:";
                    }

                    return _result;
                }
                else
                {
                    throw new Exception("命令" + command + "处理程序结构异常。");
                }

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 系统初始化命令
        /// </summary> 
        private bool init(string[] array, out string msg, out DataTable dt, out int affected)
        {
            try
            {
                CP.Core.SystemCore core = new Core.SystemCore();
                bool result = core.Init();
                dt = null;
                affected = 0;
                if (result)
                    msg = "系统初始化成功。";
                else
                    msg = "系统初始化失败";

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///// <summary>
        ///// 数据查询命令
        ///// </summary>
        //private bool RunSQL(string[] array, out string msg, out DataTable dt, out int affected)
        //{
        //    try
        //    {
        //        //CP.Core.SystemCore core = new Core.SystemCore();
        //        //dt = core.RunSQL(array);
        //        //affected = 0;
        //        //msg = "数据查询成功。";
        //        return true;

        //    }
        //    catch (Exception)
        //    {
        //        msg = "数据查询失败";
        //        dt = null;
        //        affected = 0;
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 数据新增、修改、删除命令
        ///// </summary>
        //private bool RunSQLToTable(string[] array, out string msg, out DataTable dt, out int affected)
        //{
        //    try
        //    {
        //        //CP.Core.SystemCore core = new Core.SystemCore();
        //        //affected = core.RunSQLToTable(array);
        //        //dt = null;
        //        ////msg = "命令执行成功,有" + affected + "行数据受影响。";
        //        //return true;

        //    }
        //    catch (Exception)
        //    {
        //        msg = "命令执行失败,有0行数据受影响";
        //        affected = 0;
        //        dt = null;
        //        return false;
        //    }
        //}


        /// <summary>
        /// 停止服务命令
        /// </summary>
        private bool Stop(string[] array, out string msg, out DataTable dt, out int affected)
        {
            try
            {
                CP.Core.SystemCore core = new Core.SystemCore();
                bool result = core.Stop();
                affected = 0;
                dt = null;
                if (result)
                    msg = "系统停止服务命令执行成功。";
                else
                    msg = "系统停止服务命令执行失败";

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
