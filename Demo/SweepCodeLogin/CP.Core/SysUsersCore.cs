using CP.Common;
using CP.Core.Web;
using CP.Entiry;
using CP.Entiry.Web;
using CP.EntityBase;
using System;
using System.Data;
using System.Text;

namespace CP.Core
{
    public class SysUsersCore
    {
        #region 判断当前登录用户角色 崔萌 2019-11-20 14:50:11
        /// <remarks>崔萌 2019-11-20 14:50:11</remarks>
        /// <summary>
        /// 判断当前登录用户角色
        /// </summary>
        public string CheckUserRole()
        {
            //1.获取当前登录用户的角色ID及各角色ID
            int userRoleID = Convert.ToInt32(WebUser.RoleId);
            int sypervisorRoleID = sysConst.SupervisorRoleID;//业务主管角色ID
            int directorRoleID = sysConst.DirectorRoleID;//业务总监角色
            string adminRoleID = sysConst.AdminRoleIDList;//管理员角色ID

            //2.判断当前用户角色
            if (directorRoleID.Equals(userRoleID))
            {
                return "";
            }

            //若是业务主管需要获取主管部门下的所有业务员
            if (sypervisorRoleID.Equals(userRoleID))
            {
                StringBuilder ids = new StringBuilder();
                //获取业务员ID所在主管部门下的所有业务员
                DataTable dt = GetAllPeopleByUserID(Convert.ToInt32(WebUser.ID));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids.AppendFormat("{0},", dt.Rows[i]["ID"]);
                }

                return ids.ToString().TrimEnd(',');
            }

            return WebUser.ID.ToString();
        }
        #endregion

        #region 获取业务员ID所在主管部门下的所有业务员 崔萌 2019-11-20 15:10:17
        /// <remarks>崔萌 2019-11-20 15:10:17</remarks>
        /// <summary>
        /// 获取业务员ID所在主管部门下的所有业务员
        /// </summary>
        /// <param name="userID">当前登录用户id</param>
        /// <returns>所有业务员</returns>
        private DataTable GetAllPeopleByUserID(int userID)
        {
            //1.校验参数
            if (string.IsNullOrEmpty(userID.ToString()))
            {
                throw new UIException("当前登录用户ID不能为空。");
            }

            //2.获取所在主管部门下的所有业务员
            QueryParams param = new QueryParams();
            param.addWhere(sys_UsersAttr.IsDelete, "=", "@IsDelete", IsDelete.N);
            param.addAnd();
            param.addWhere(sys_UsersAttr.CheckState, "=", "@CheckState", CheckState.Approved);
            param.addAnd();
            param.addWhere(string.Format(" GroupId = (SELECT GroupId FROM sys_Users WHERE IsDelete='N' AND ID={0})", userID));

            string[] fields = new string[] { sys_UsersAttr.ID };

            sys_Users users = new sys_Users();
            DataTable dt = users.QueryToDataTable(param, fields);

            //3.校验查询的数据
            if (dt == null)
            {
                throw new UIException("根据业务员ID获取数据异常。");
            }

            return dt;
        }
        #endregion
    }
}
