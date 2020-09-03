using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace CP.Core.Web
{
    public class SuperPage : System.Web.UI.Page
    {
        public SuperPage()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region 属性

        //当前用户是否是管理员
        public bool IsAdmin
        {
            get
            {

                if (!string.IsNullOrEmpty(CP.Common.sysConst.AdminRoleIDList))
                {
                    string[] array = CP.Common.sysConst.AdminRoleIDList.Split(',');

                    foreach (var item in array)
                    {
                        if (CP.Entiry.Web.WebUser.RoleId + "" == item)
                            return true;
                    }
                }

                return false;
            }
        }

        public string Action
        {
            get { return Request.Params["action"]; }
        }

        public string MenuID
        {
            get
            {
                string currentUrl = "~" + Request.Url.AbsolutePath;
                List<CP.Entiry.sys_Menu> menus = SystemCore.GetCurrentMenu();

                List<CP.Entiry.sys_Menu> list = menus.Where(T => T.LinkAddress.ToLower() == currentUrl.ToLower()).ToList();

                if (list.Count == 1)
                    return list[0].ID.ToString();
                if (list.Count > 1)
                {
                    string _msg = "当前浏览的页面链接存在重复注册,请检查页面注册信息。";
                    string _log = string.Format(_msg + "\r\n请求链接:[{0}]\r\n登陆账号:[{1}]\r\n角色ID:[{3}]\r\n", Request.Url.AbsolutePath, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.RoleId);

                    CP.Common.Log4.Log4Error(_log);

                    GoTo("#8001", null, _msg);

                    return string.Empty;
                }
                else
                    return string.Empty;
            }
        }
        #endregion

        public string RootPath
        {
            get
            {

                if (Request.Url.Port == 80)
                    return Request.Url.Scheme + "://" + Request.Url.Host;
                else
                    return Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;

            }
        }
        #region Page Event
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                #region 校验系统状态
                if (CP.Core.SYS.sysStatic.SysInitState != Common.SysInit.Normal)
                {
                    //GoTo("#10000", null, "系统正在维护...");
                }
                #endregion
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                // Response.End() 引发此异常. 无需处理.
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region 处理Aciton
                switch (this.Action)
                {
                    case "P_LoadResource":
                        P_LoadResource();
                        break;
                    case "P_IsLogin":
                        P_IsLogin();
                        break;
                }
                #endregion
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                // Response.End() 引发此异常. 无需处理.
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }

        #endregion

        #region Action处理函数

        /// <summary>
        /// 绑定按钮
        /// </summary>
        /// <param name="id"></param>
        protected void P_LoadResource()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();

            try
            {
                string menuid = this.MenuID;

                CP.Core.SystemCore.CurrentPermission currentPermission = CP.Core.SystemCore.GetCurrentPermission(CP.Entiry.Web.WebUser.RoleId.Value);

                List<CP.Entiry.sys_MenuButton> buttonlist = currentPermission.Buttonlist.Where(T => T.FK_Menu_ID.ToString() == menuid && T.IsFlag == CP.Common.IsFlag.Y).OrderBy(T => T.Sort).ToList();

                json.Clear();
                json.Add("Result", "0");
                json.Add("Data", CP.Common.JSONHelper.ListToJson<CP.Entiry.sys_MenuButton>(buttonlist));
            }
            catch (Exception ex)
            {
                this.Exception(ex);
            }

            Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
            Response.End();
        }


        protected void P_IsLogin()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            try
            {
                bool result = IsLogin();

                json.Clear();
                json.Add("Result", "0");
                json.Add("IsLogin", result.ToString().ToLower());
            }
            catch (Exception ex)
            {
                this.Exception(ex);
            }

            Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
            Response.End();
        }

        #endregion

        #region 自定义函数

        public bool IsLogin()
        {
            if (string.IsNullOrEmpty(CP.Entiry.Web.WebUser.UserCode))
                return false;
            else
                return true;
        }


        /// <summary>
        /// UI层 统一的异常处理函数
        /// </summary>
        /// <param name="ex"></param>
        public void Exception(Exception ex)
        {
            if (Equals(ex.GetType(), typeof(System.Threading.ThreadAbortException)))
                return;
            string guid = DateTime.Now.ToString("yyyyMMddHHmmss") + CP.Common.Common.CreateRandom(4);
            try
            {
                ///记录日志
                string errMsg = guid + " " + ex.StackTrace + "引发了系统异常, 异常描述:" + ex.Message;

                CP.Common.Log4.Log4Error(errMsg);
            }
            catch (Exception)
            {
                ///隐藏异常 保证此模块正常运行
            }
            finally
            {
                string alterMsg = string.Empty;
                if (CP.Common.Common.AppSetting("IsDebug").ToLower() == "true")
                    alterMsg = ex.StackTrace + "引发了系统异常, 异常描述:" + ex.Message;
                else
                    alterMsg = "您触发了一个系统BUG, BUG编号: " + guid;


                //可在此加入异常处理完成后 应如何处理代码
                GoTo("#8000", "", alterMsg);
            }
        }

        #region 跳转

        protected void GoToLogin(string errNo, string errMsg)
        {
            if (CP.Common.Common.GetUrlBasePath().Contains("app"))
            {
                GoTo(errNo, CP.Common.Common.GetUrlAuthorityPath() + "/login", errMsg);
            }
            else
            {
                GoTo(errNo, CP.Common.Common.GetUrlBasePath() + "/Login.aspx", errMsg);
            }
        }

        protected void GoTo(string errNo, string Url, string errMsg)
        {
            if (CP.Common.Common.IsAjaxRequest)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(Url))
                    parms.Add("Skip", Url);
                //parms.Add("Msg", errMsg + "   " + errNo);
                if (!string.IsNullOrEmpty(errMsg))
                    parms.Add("Msg", errMsg);
                parms.Add("sysError", "1");
                string json = CP.Common.JSONHelper.DictionaryToJson(parms);

                Response.Write(json);
                Response.End();
            }
            else
            {
                string script = "<script>";
                if (!string.IsNullOrEmpty(errMsg))
                {
                    script += "var errMsg = '';";

                    foreach (var item in errMsg.Split('\n'))
                        script += "errMsg += '" + item.Replace("\r", "").Replace("\n", "") + "\\n';";

                    script += "alert(errMsg);";
                }
                if (!string.IsNullOrEmpty(Url))
                    script += "top.document.location.href='" + Url + "';";
                script += "</script>";

                Response.Write(script);
                Response.End();
            }
        }

        #endregion

        #endregion



        /// <summary>
        /// 校验可查询字段json 格式是否正确
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckSearchfieldJson(JObject jo, out string errMsg)
        {
            try
            {
                #region 判断json中[key]是否存在
                if (!jo.Properties().Any(p => p.Name == "field"))
                {
                    errMsg = "field不存在。";
                    return false;
                }

                if (!jo.Properties().Any(p => p.Name == "datatype"))
                {
                    errMsg = "datatype不存在。";
                    return false;
                }

                if (!jo.Properties().Any(p => p.Name == "ext"))
                {
                    errMsg = "ext不存在。";
                    return false;
                }
                #endregion

                #region 判断json中[value]是否为空
                string field = jo["field"].ToString();
                if (string.IsNullOrEmpty(field))
                {
                    errMsg = "字段格式错误。";
                    return false;
                }

                string datatype = jo["datatype"].ToString();
                if (string.IsNullOrEmpty(datatype))
                {
                    errMsg = "字段类型格式错误。";
                    return false;
                }
                string ext = jo["ext"].ToString();
                if (string.IsNullOrEmpty(ext))
                {
                    errMsg = "运算符格式错误。";
                    return false;
                }
                #endregion

                #region 运算符是否符合规定、数据类型是否符合规定
                if (!CheckExt(ext))
                {
                    errMsg = "运算符格式不合法。";
                    return false;
                }

                if (!CheckDataType(datatype))
                {
                    errMsg = "数据类型格式不合法。";
                    return false;
                }
                #endregion

                errMsg = string.Empty;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 校验 可查询字段json 是否含有某一字段
        /// </summary>
        /// <param name="jsonData">可查询字段json</param>
        /// <param name="filedName">字段名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool CheckSearchfieldIshasFiled(string jsonData, string filedName, out string errMsg)
        {
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonData);
                string field = jo["field"].ToString();

                #region 校验可查询字段json格式是否正确
                if (!CheckSearchfieldJson(jo, out errMsg))
                {
                    return false;
                }
                #endregion

                #region 判断可查询字段json 是否含有 查询的字段
                if (field != filedName)
                {
                    errMsg = string.Empty;
                    return false;
                }
                #endregion

                errMsg = string.Empty;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 格式化查询参数（bootstraptable下拉框查询显示内容）
        /// </summary>
        /// <param name="jsonData">可查询字段json</param>
        /// <param name="list">实体类集合</param>
        /// <param name="_search">查询的字段</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public CP.EntityBase.QueryParam FormatQueryParameters(string jsonData, List<CP.EntityBase.Entity> list, string _search, out string errMsg)
        {
            try
            {
                CP.EntityBase.QueryParam param = null;

                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonData);
                string field = jo["field"].ToString();
                string datatype = jo["datatype"].ToString();
                string ext = jo["ext"].ToString();

                #region 校验可查询字段json格式是否正确
                if (!CheckSearchfieldJson(jo, out errMsg))
                {
                    return param;
                }
                #endregion

                #region 判断字段是否存在于实体类中
                //如果格式校验正确，返回formatField格式为：表名.字段名
                string formatField = string.Empty;
                if (!IsEntityContainFiled(list, field, out formatField))
                {
                    errMsg = "实体类中不存在该字段。";
                    return param;
                }
                #endregion


                #region 判断datatype是否为datetime类型
                errMsg = string.Empty;
                switch (datatype)
                {
                    case "datetime":

                        #region 判断_search json中[key]是否存在
                        JObject jos = (JObject)JsonConvert.DeserializeObject(_search);

                        if (!jos.Properties().Any(p => p.Name == "begindate"))
                        {
                            errMsg = "begindate不存在。";
                            return param;
                        }

                        if (!jos.Properties().Any(p => p.Name == "enddate"))
                        {
                            errMsg = "enddate不存在。";
                            return param;
                        }

                        string begindate = jos["begindate"].ToString();
                        string enddate = jos["enddate"].ToString();
                        #endregion

                        param = new EntityBase.QueryParam(GetSQLByDataType(formatField, begindate, enddate), null, null);
                        break;
                    default:
                        param = new EntityBase.QueryParam(GetSQLByExt(formatField, ext), "@_search", _search);
                        break;
                }
                #endregion

                return param;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CP.EntityBase.QueryParams FormatQueryParameters(string jsonData, out string errMsg)
        {
            CP.EntityBase.QueryParams parms = new EntityBase.QueryParams();

            JArray ja = JArray.Parse(jsonData);

            foreach (var jo in ja)
            {
                parms.addAnd(); //先添加AND 因为后面添加 需要判断最后一个不添加

                string field = jo["field"].ToString();
                string datatype = jo["datatype"].ToString();


                switch (datatype)
                {
                    case "datetime":
                        string starttime = jo["starttime"].ToString();
                        string endtime = jo["endtime"].ToString();

                        if (!string.IsNullOrEmpty(starttime))
                        {
                            starttime += " 00:00:00";

                            parms.addWhere(GetSQLByExt2(field, ">=", field + "_s"), "@" + field + "_s", starttime);

                            if (!string.IsNullOrEmpty(endtime))
                                parms.addAnd();
                        }
                        if (!string.IsNullOrEmpty(endtime))
                        {
                            endtime += " 23:59:59";

                            parms.addWhere(GetSQLByExt2(field, "<=", field + "_e"), "@" + field + "_e", endtime);
                        }
                        break;
                    default:
                        string ext = jo["ext"].ToString();
                        string val = jo["value"].ToString();

                        parms.addWhere(GetSQLByExt2(field, ext), "@" + field, val);
                        break;
                }
            }

            errMsg = "";
            return parms;
        }

        /// <summary>
        /// 检查运算符是否符合规定
        /// </summary>
        /// <param name="ext">运算符</param>
        /// <returns></returns>
        private bool CheckExt(string ext)
        {
            string[] arr = new string[] { 
                "=", "!=", "like", "in", "not in"
            };

            if (Array.IndexOf<string>(arr, ext.ToLower()) == -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查数据类型是否符合规定
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <returns></returns>
        private bool CheckDataType(string datatype)
        {
            string[] arr = new string[] { 
                "int", "string", "datetime"
            };

            if (Array.IndexOf<string>(arr, datatype.ToLower()) == -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断字段是否存在于实体类中
        /// </summary>
        /// <param name="list">实体类集合</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        private bool IsEntityContainFiled(List<CP.EntityBase.Entity> list, string field, out string formatField)
        {
            foreach (CP.EntityBase.Entity Entity in list)
            {
                if (Entity.EnMap.Attrs.Contains(field))
                {
                    if (field == "Key")
                        formatField = Entity.PhysicsTable + ".[" + field + "]";
                    else
                        formatField = Entity.PhysicsTable + "." + field;
                    return true;
                }
            }
            formatField = string.Empty;
            return false;
        }

        /// <summary>
        /// 获取datatype为datetime类型的SQL
        /// </summary>
        /// <param name="field">检索字段</param>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <returns></returns>
        private string GetSQLByDataType(string field, string begindate, string enddate)
        {
            string sql = string.Empty;

            if (!string.IsNullOrEmpty(begindate))
            {
                sql += string.Format(" {0} >= '{1}'  ", field, begindate);

                if (!string.IsNullOrEmpty(enddate))
                    sql += " AND ";
            }

            if (!string.IsNullOrEmpty(enddate))
                sql += string.Format("{0} <= '{1}' ", field, enddate);


            return sql;
        }

        /// <summary>
        /// 根据运算符，获取SQL
        /// </summary>
        /// <param name="field">检索字段</param>
        /// <param name="ext">运算符</param>
        /// <returns></returns>
        private string GetSQLByExt(string field, string ext)
        {
            string sql = string.Empty;
            switch (ext.ToLower())
            {
                case "like":
                    sql = string.Format("{0} {1} '%'+@_search+'%' ", field, ext);
                    break;
                case "=":
                case "!=":
                    sql = string.Format("{0} {1} @_search ", field, ext);
                    break;
                default:
                    break;
            }
            return sql;
        }
        /// <summary>
        /// 根据运算符，获取SQL
        /// </summary>
        /// <param name="field">检索字段</param>
        /// <param name="ext">运算符</param>
        /// <returns></returns>
        private string GetSQLByExt2(string field, string ext)
        {
            return GetSQLByExt2(field, ext, field);
        }

        /// <summary>
        /// 根据运算符，获取SQL
        /// </summary>
        /// <param name="field">检索字段</param>
        /// <param name="ext">运算符</param>
        /// <returns></returns>
        private string GetSQLByExt2(string field, string ext, string parmname)
        {
            string sql = string.Empty;
            switch (ext.ToLower())
            {
                case "like":
                    sql = string.Format("{0} {1} '%'+@" + parmname + "+'%' ", field, ext);
                    break;
                default:
                    sql = string.Format("{0} {1} @" + parmname, field, ext);
                    break;
            }
            return sql;
        }
    }

    #region UIException
    [Serializable]
    public class UIException : Exception, ISerializable
    {
        //三个共有构造器
        public UIException()
            : base()//调用基类的构造器
        {
        }
        public UIException(string message)
            : base(message)//调用基类的构造器
        {
        }
        public UIException(string message, Exception innerException)
            : base(message, innerException)//调用基类的构造器
        { }
        //定义了一个私有字段
        private string diskpath;
        //顶一个只读属性，改属性返回新定义的字段
        public string DiskPath { get { return diskpath; } }
        //重新共有属性Message，将新定义的字段包含在异常的信息文本中
        public override string Message
        {
            get
            {
                string msg = base.Message;
                if (diskpath != null)
                    msg += Environment.NewLine + "Disk Path:" + diskpath;
                return msg;
            }
        }
        //因为定义了至少新的字段，所以要定义一个特殊的构造器用于反序列化
        //由于该类是密封类，所以该构造器的访问限制被定义为私有方式。否则，该构造器
        //被定义为受保护方式
        private UIException(SerializationInfo info, StreamingContext context)
            : base(info, context)//让基类反序列化其内定义的字段
        {
            diskpath = info.GetString("DiskPath");
        }
        //因为定义了至少一个字段，所以要定义序列化方法
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //序列化新定义的每个字段
            info.AddValue("DiskPath", diskpath);
            //让基类序列化其内定义的字段
            base.GetObjectData(info, context);
        }
        //定义额外的构造器设置新定义的字段
        public UIException(string message, string diskpath)
            : this(message)//调用另外一个构造器
        {
            this.diskpath = diskpath;
        }
        public UIException(string message, string diskpath, Exception innerException)
            : this(message, innerException)
        {
            this.diskpath = diskpath;
        }
    }
    #endregion



}


