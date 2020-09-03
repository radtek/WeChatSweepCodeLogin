using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Core
{
    public class SystemCore
    {

        #region 系统初始化

        public bool Init()
        {
            try
            {
                CP.Core.SYS.sysStatic.SysInitState = Common.SysInit.Initializing;

                bool result = false;

                #region 系统参数
                result = CP.Core.SystemCore.SetCurrentRunParameter();

                if (!result)
                {
                    return false;
                }
                #endregion

                #region 读取权限

                result = SetCurrentPermission();

                if (!result)
                {
                    return false;
                }

                #endregion

                #region 读取菜单

                result = SetCurrentMenu();

                if (!result)
                {
                    return false;
                }
                #endregion

                #region 读取企业资助申报的批次号

                //result = SetFundingBatchNo();

                //if (!result)
                //{
                //    return false;
                //}
                #endregion

                CP.Core.SYS.sysStatic.SysInitState = Common.SysInit.Normal;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region 系统设置函数
        /// <summary>
        /// 存放系统运行参数
        /// </summary>
        /// <returns></returns>
        public static bool SetCurrentRunParameter()
        {
            try
            {
                Dictionary<string, string> currentRunParameter = new Dictionary<string, string>();

                currentRunParameter.Clear();

                CP.Entiry.sys_RunParameter role = new Entiry.sys_RunParameter();
                List<CP.Entiry.sys_RunParameter> rolelist = role.QueryToList<CP.Entiry.sys_RunParameter>();


                foreach (CP.Entiry.sys_RunParameter item in rolelist)
                {
                    System.Reflection.FieldInfo f_key = typeof(CP.Common.sysConst).GetField(item.Key);
                    if (f_key != null)
                    {
                        object obj = item.Value;

                        switch (item.DataType)
                        {
                            case "int":
                                obj = Convert.ToInt32(item.Value);
                                break;
                        }

                        f_key.SetValue(null, obj);
                    }

                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 存放当前权限
        /// </summary>
        /// <returns></returns>
        public static bool SetCurrentPermission()
        {
            try
            {
                Dictionary<int, CurrentPermission> currentPermission = new Dictionary<int, CurrentPermission>();

                CP.Entiry.sys_Roles role = new Entiry.sys_Roles();
                List<CP.Entiry.sys_Roles> rolelist = role.QueryToList<CP.Entiry.sys_Roles>();

                if (rolelist.Count > 0)
                {
                    foreach (CP.Entiry.sys_Roles item in rolelist)
                    {
                        if (item.ID != null)
                        {
                            #region 获取权限
                            CP.Entiry.sys_Permissions perr = new Entiry.sys_Permissions();
                            CP.EntityBase.QueryParams perrparam = new CP.EntityBase.QueryParams();
                            perrparam.addWhere("FK_Role_ID=@FK_Role_ID", "@FK_Role_ID", item.ID);

                            List<CP.Entiry.sys_Permissions> _permisslist = perr.QueryToList<CP.Entiry.sys_Permissions>(perrparam);

                            StringBuilder menubuffer = new StringBuilder();

                            for (int i = 0; i < _permisslist.Count; i++)
                            {
                                int _fk_menubutton_id = _permisslist[i].FK_MenuButton_ID.Value;

                                if (_fk_menubutton_id == CP.Common.sysConst.C_Permission_PerViewID)
                                    continue;
                                else
                                {
                                    menubuffer.Append(_fk_menubutton_id);
                                    if (i < _permisslist.Count - 1)
                                        menubuffer.Append(",");
                                }
                            }
                            #endregion

                            #region 获取按钮
                            CP.Entiry.sys_MenuButton button = new Entiry.sys_MenuButton();
                            CP.EntityBase.QueryParams buttonparam = new CP.EntityBase.QueryParams();
                            buttonparam.addWhere("ID IN (SELECT COL FROM SPLIT(@FK_Menu_ID,','))", "@FK_Menu_ID", menubuffer.ToString());

                            List<CP.Entiry.sys_MenuButton> _buttonlist = button.QueryToList<CP.Entiry.sys_MenuButton>(buttonparam);
                            #endregion

                            CurrentPermission current = new CurrentPermission(_permisslist, _buttonlist);

                            currentPermission.Add(item.ID.Value, current);
                        }
                    }
                }



                CP.Common.Common.CurrentHttpContext.Application[CP.Common.sysConst.C_Permission_ApplicationName] = currentPermission;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 存放当前系统菜单
        /// </summary>
        /// <returns></returns>
        public static bool SetCurrentMenu()
        {
            try
            {
                CP.Entiry.sys_Menu menu = new Entiry.sys_Menu();
                List<CP.Entiry.sys_Menu> list = menu.QueryToList<CP.Entiry.sys_Menu>();

                CP.Common.Common.CurrentHttpContext.Application[CP.Common.sysConst.C_Menu_ApplicationName] = list;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        /// <summary>
        /// 根据角色ID获取权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static CurrentPermission GetCurrentPermission(int roleID)
        {
            try
            {

                if (CP.Core.SYS.sysStatic.SysInitState != Common.SysInit.Normal)
                {
                    SetCurrentPermission();
                }

                Dictionary<int, CurrentPermission> currentPermission = CP.Common.Common.CurrentHttpContext.Application[CP.Common.sysConst.C_Permission_ApplicationName] as Dictionary<int, CurrentPermission>;

                List<CP.Entiry.sys_Permissions> permisslist = new List<Entiry.sys_Permissions>();

                if (currentPermission != null)
                {
                    if (currentPermission.ContainsKey(roleID))
                        return currentPermission[roleID];
                }

                return new CurrentPermission();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前系统菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static List<CP.Entiry.sys_Menu> GetCurrentMenu()
        {
            try
            {
                return CP.Common.Common.CurrentHttpContext.Application[CP.Common.sysConst.C_Menu_ApplicationName] as List<CP.Entiry.sys_Menu>;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region 数据查询

        public DataTable RunSQL(string[] array)
        {
            try
            {
                string SYSConnStrName = CP.Common.Common.AppSetting(array[0]);
                string sql = "";
                for (int i = 1; i < array.Length; i++)
                {
                    sql += array[i] + " ";
                }

                DataTable dt = CP.Common.DBHelper.ExecuteDataTable(SYSConnStrName, sql);

                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region 数据新增、修改、删除

        public int RunSQLToTable(string[] array)
        {
            try
            {
                string SYSConnStrName = CP.Common.Common.AppSetting(array[0]);
                string sql = "";
                for (int i = 1; i < array.Length; i++)
                {
                    sql += array[i] + " ";
                }

                int result = CP.Common.DBHelper.ExecuteNonQuery(SYSConnStrName, sql);

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region 停止服务

        public bool Stop()
        {
            try
            {
                CP.Core.SYS.sysStatic.SysInitState = Common.SysInit.Uninitialized;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public class CurrentPermission
        {
            public CurrentPermission()
            { }

            public CurrentPermission(List<CP.Entiry.sys_Permissions> permisslist, List<CP.Entiry.sys_MenuButton> buttonlist)
            {
                this.Permisslist = permisslist;
                this.Buttonlist = buttonlist;
            }

            public List<CP.Entiry.sys_Permissions> Permisslist = new List<Entiry.sys_Permissions>();
            public List<CP.Entiry.sys_MenuButton> Buttonlist = new List<Entiry.sys_MenuButton>();
        }
    }
}
