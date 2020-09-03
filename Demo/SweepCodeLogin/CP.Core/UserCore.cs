using CP.Common;
using CP.Core.Web;
using CP.Entiry;
using CP.Entiry.Web;
using CP.EntityBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CP.Core
{
    public class UserCore
    {
        #region 登陆
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userCode">输入的账号</param>
        /// <param name="passWord">输入的账号</param>
        /// <param name="loginType">前台登陆还是后台登陆</param>
        /// <returns>是否登陆成功</returns>
        public bool Login(string userCode, string passWord, LoginType loginType, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Users user = new Entiry.sys_Users();

                QueryParams Params = new QueryParams();
                Params.addWhere("UserCode=@uname", "@uname", userCode);
                //Params.addAnd();
                //Params.addWhere("IsFlag=@isFlag", "@IsFlag", CP.Common.IsFlag.Y);
                user.Query(Params);

                if (!string.IsNullOrEmpty(user.UserCode))
                {
                    if (user.LoginType.ToString() == Convert.ToInt32(LoginType.Banned).ToString())
                    {
                        errMsg = "您的账号已被禁用。";
                        CP.DA.LoginLog.WriteInfo(userCode, user.UserName, CP.Common.LogEnum.LogRunState.Failure, CP.Common.LoginType.Forepart);
                        return false;
                    }
                    //else if (!Equals(user.IsFlag, CP.Common.IsFlag.Y))
                    //{
                    //    errMsg = "您的账号尚未激活,请激活后再次尝试登陆。";
                    //    CP.DA.LoginLog.WriteInfo(userCode, user.UserName, CP.Common.LogEnum.LogRunState.Failure, CP.Common.LoginType.Forepart);
                    //    return false;
                    //}
                    else
                    {
                        if (Equals(user.LoginType, Convert.ToInt32(loginType)) || Equals(user.LoginType, Convert.ToInt32(CP.Common.LoginType.ALL)))
                        {
                            if (!Equals(user.PassWord, CP.Common.Common.EncryptToSHA1(passWord)))
                            {
                                errMsg = "输入的密码与账户不匹配,请确认后重新输入。";
                                CP.DA.LoginLog.WriteInfo(userCode, user.UserName, CP.Common.LogEnum.LogRunState.Failure, CP.Common.LoginType.Forepart);
                                return false;
                            }
                            else
                            {
                                //添加积分记录
                                //AddIngegralLog(IntegralsSource.DailyLogin, Convert.ToInt32(user.ID));

                                #region 更新用户最后登陆IP 和 时间
                                CP.Entiry.sys_Users Uuser = new Entiry.sys_Users();
                                Uuser.ID = user.ID;
                                Uuser.LastTIme = DateTime.Now;
                                Uuser.LastIP = CP.Common.Common.IPAddress;
                                Uuser.Update();
                                #endregion

                                //将登陆账号放置到 Session中
                                CP.Entiry.Web.WebUser.SetWebUser(user);

                                errMsg = "登陆成功。";
                                CP.DA.LoginLog.WriteInfo(userCode, user.UserName, CP.Common.LogEnum.LogRunState.Succeed, CP.Common.LoginType.Forepart);



                                return true;
                            }
                        }
                        else
                        {
                            errMsg = "您的账号不允许在此登陆。";
                            CP.DA.LoginLog.WriteInfo(userCode, user.UserName, CP.Common.LogEnum.LogRunState.Failure, CP.Common.LoginType.Forepart);
                            return false;
                        }
                    }
                }
                else
                {

                    errMsg = "您输入的账号无效,请确认后重新输入。";
                    CP.DA.LoginLog.WriteInfo(userCode, string.Empty, CP.Common.LogEnum.LogRunState.Failure, CP.Common.LoginType.Forepart);
                    return false;
                }
            }
            catch (Exception ex)
            {
                CP.Common.Log4.Log4Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region 注册

        /// <summary>
        ///  注册
        /// </summary>
        /// <param name="accounttype">注册类型</param>
        /// <param name="essentialinfo_dt">企业、个人信息</param>
        /// <param name="userinfo_dt">用户名密码信息</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Register(string accounttype, DataTable essentialinfo_dt, DataTable userinfo_dt, out string errMsg)
        {
            try
            {
                #region 校验
                if (string.IsNullOrEmpty(accounttype))
                {
                    errMsg = "注册类型错误。";
                    return false;
                }
                if (essentialinfo_dt.Rows.Count <= 0)
                {
                    errMsg = "请填写注册信息。";
                    return false;
                }
                if (userinfo_dt.Rows.Count <= 0)
                {
                    errMsg = "请填写注册信息。";
                    return false;
                }
                #region 企业信息校验
                string company = string.Empty;
                string companyxingzhi = string.Empty;
                string appaynumber = string.Empty;
                //string province = string.Empty;
                string address = string.Empty;
                string shifanqiye = string.Empty;
                string youshiqiye = string.Empty;
                string guanbiaoqiye = string.Empty;
                string email = string.Empty;

                if (accounttype == CP.Common.AccountType.Enterprise)
                {
                    company = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.Company].ToString();
                    companyxingzhi = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.CompanyXingZhi].ToString();
                    appaynumber = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.AppayNumber].ToString();
                    //province = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.Province].ToString();
                    address = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.Address].ToString();

                    shifanqiye = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.ShiFanQiYe].ToString();
                    youshiqiye = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.YouShiQiYe].ToString();
                    guanbiaoqiye = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.GuanBiaoQiYe].ToString();

                    email = essentialinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.EMail].ToString();

                    if (string.IsNullOrEmpty(company))
                    {
                        errMsg = "请输入企业名称。";
                        return false;
                    }
                    if (string.IsNullOrEmpty(companyxingzhi))
                    {
                        errMsg = "请选择企业性质。";
                        return false;
                    }
                    if (string.IsNullOrEmpty(appaynumber))
                    {
                        errMsg = "请输入证件号码。";
                        return false;
                    }
                    //if (string.IsNullOrEmpty(province))
                    //{
                    //    errMsg = "请选择注册地区。";
                    //    return false;
                    //}
                    if (string.IsNullOrEmpty(address))
                    {
                        errMsg = "请输入详细地址。";
                        return false;
                    }
                    if (string.IsNullOrEmpty(email))
                    {
                        errMsg = "请输入邮箱(用于密码找回)。";
                        return false;
                    }
                }
                #endregion

                #region 联系人校验
                string linkman = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.LinkMan].ToString();
                string mobilephone = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.MobilePhone].ToString();
                string qq = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.QQ].ToString();
                if (accounttype == CP.Common.AccountType.Personal)
                {
                    email = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.EMail].ToString();
                    if (string.IsNullOrEmpty(linkman))
                    {
                        errMsg = "请输入联系人姓名。";
                        return false;
                    }
                    if (string.IsNullOrEmpty(mobilephone))
                    {
                        errMsg = "请输入联系人手机号码。";
                        return false;
                    }
                    if (string.IsNullOrEmpty(email))
                    {
                        errMsg = "请输入联系人邮箱。";
                        return false;
                    }
                }
                string postaladdress = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.PostalAddress].ToString();
                if (string.IsNullOrEmpty(postaladdress))
                {
                    errMsg = "请输入通讯地址。";
                    return false;
                }
                string zipcode = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.ZipCode].ToString();
                #endregion

                #region 注册信息校验
                string usercode = userinfo_dt.Rows[0][CP.Entiry.sys_UsersAttr.UserCode].ToString();
                if (string.IsNullOrEmpty(usercode))
                {
                    errMsg = "请输入用户名。";
                    return false;
                }
                string password = userinfo_dt.Rows[0]["PassWord1"].ToString();
                if (string.IsNullOrEmpty(password))
                {
                    errMsg = "请输入密码。";
                    return false;
                }
                string validatecode = userinfo_dt.Rows[0]["VerificationCode"].ToString();
                if (string.IsNullOrEmpty(validatecode))
                {
                    errMsg = "请输入验证码。";
                    return false;
                }

                #endregion
                #endregion

                #region 检查用户是否已存在
                if (IsRegisted(usercode, string.IsNullOrEmpty(company) == true ? string.Empty : company.Trim(), string.IsNullOrEmpty(appaynumber) == true ? string.Empty : appaynumber.Trim(), 0, out errMsg))
                {
                    return false;
                }
                #endregion

                #region 校验验证码

                if (!CheckPictureVerificationCode(validatecode, out errMsg))
                {
                    return false;
                }
                #endregion

                #region 注册用户
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();

                user.UserCode = usercode;
                user.PassWord = CP.Common.Common.EncryptToSHA1(password);
                user.AccountType = accounttype;

                user.LoginType = Convert.ToInt32(CP.Common.LoginType.Forepart);
                user.RoleId = sysConst.RegisterRoleID;
                user.RoleCode = sysConst.RegisterRoleCode;
                user.RoleName = sysConst.RegisterRoleName;
                user.RegIP = CP.Common.Common.IPAddress;
                user.RegTime = DateTime.Now;

                user.QQ = qq;
                user.ZipCode = zipcode;
                user.Company = company;
                user.CompanyXingZhi = companyxingzhi;
                user.AppayNumber = appaynumber;
                //user.Province = province;
                user.Address = address;
                if (accounttype == CP.Common.AccountType.Enterprise)
                {
                    user.UserName = company;//企业用户 用户名为企业名称
                    user.RealName = company;
                }
                else
                {
                    user.UserName = linkman;//个人用户 用户名为联系人名称
                    user.RealName = linkman;
                }

                user.ShiFanQiYe = shifanqiye;
                user.YouShiQiYe = youshiqiye;
                user.GuanBiaoQiYe = guanbiaoqiye;

                user.EMail = email;
                user.MobilePhone = mobilephone;
                user.PostalAddress = postaladdress;
                user.LinkMan = linkman;
                user.IsDelete = CP.Common.IsDelete.N;

                // 写入数据库
                int uid = user.InsertReID();
                #endregion

                #region 记录注册日志
                CP.DA.Log.WriteInfo("0", "注册", CP.Common.LogEnum.LogRunState.Succeed, "注册成功。");
                #endregion

                errMsg = "执行成功";
                return true;
            }
            catch (Exception ex)
            {
                CP.DA.Log.WriteInfo("0", "注册", CP.Common.LogEnum.LogRunState.Failure, ex.Message);

                throw;
            }
        }

        /// <summary>
        /// 校验图片验证码
        /// </summary>
        /// <returns></returns>
        public bool CheckPictureVerificationCode(string snscode, out string errMsg)
        {
            if (string.IsNullOrEmpty(snscode))
            {
                errMsg = "对不起，请输入验证码！。";
                return false;
            }
            if (CP.Common.Common.Session[CP.Common.sysConst.C_Register_SessionName] == null)
            {
                errMsg = "对不起，验证码超时或已过期！。";
                return false;
            }
            if (snscode.ToLower() != (CP.Common.Common.Session[CP.Common.sysConst.C_Register_SessionName]).ToString().ToLower())//判断发送的验证码和提交的验证码是否一致
            {
                errMsg = "您输入的验证码与系统的不一致！。";
                return false;
            }
            CP.Common.Common.Session[CP.Common.sysConst.C_Register_SessionName] = null;

            errMsg = "验证成功";
            return true;
        }

        #endregion

        #region 修改密码
        /// <summary>
        /// 修改当前登陆用户密码
        /// </summary> 
        public bool ChangePassWord(string oldPassWord, string newPassWord, out string errMsg)
        {
            try
            {
                #region 数据校验
                if (CP.Entiry.Web.WebUser.ID == null)
                {
                    errMsg = "用户失效,请先登录。";
                    return false;
                }
                if (CP.Entiry.Web.WebUser.PassWord != CP.Common.Common.EncryptToSHA1(oldPassWord))
                {
                    errMsg = "原密码不正确。";
                    return false;
                }
                if (string.IsNullOrEmpty(newPassWord))
                {
                    errMsg = "请输入新密码。";
                    return false;
                }
                if (newPassWord == oldPassWord)
                {
                    errMsg = "新密码与原密码相同，请重新输入新密码。";
                    return false;
                }
                #endregion

                #region 修改密码
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
                user.ID = CP.Entiry.Web.WebUser.ID;
                user.PassWord = CP.Common.Common.EncryptToSHA1(newPassWord);
                user.Update();
                #endregion

                #region 更新WebUser
                CP.Entiry.sys_Users webuser = new CP.Entiry.sys_Users();
                webuser.Query(CP.Entiry.Web.WebUser.ID.ToString());

                if (webuser.ID != null)
                    CP.Entiry.Web.WebUser.SetWebUser(webuser);
                #endregion

                errMsg = "执行成功。";
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 修改当前登陆用户密码
        /// </summary> 
        public bool ChangePassWord(int userID, string newPassWord, out string errMsg)
        {
            try
            {
                #region 数据校验
                CP.Entiry.sys_Users webuser = new CP.Entiry.sys_Users();
                webuser.Query(userID.ToString());

                if (webuser.ID == null)
                {
                    errMsg = "用户不存在。";
                    return false;
                }

                #endregion

                #region 修改密码
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
                user.ID = userID;
                user.PassWord = CP.Common.Common.EncryptToSHA1(newPassWord);
                user.Update();
                #endregion


                errMsg = "执行成功";
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region 忘记密码

        #region 发送验证码

        /// <summary>
        /// 忘记密码 发送验证码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ForgetPWDPostVerificationCode(string usercode, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
                QueryParams param = new QueryParams();
                param.addWhere(CP.Entiry.sys_UsersAttr.UserCode, "=", "@UC", usercode);

                user.Query(param);

                if (user.ID == null)
                {
                    errMsg = "您输入的用户不存在。";
                    return false;
                }

                return ForgetPWDPostEmail(user.EMail, out errMsg);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 忘记密码 发送邮箱验证码
        /// </summary>
        private bool ForgetPWDPostEmail(string email, out string errMsg)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                if (CP.Common.Common.Session[CP.Common.sysConst.C_ForgetPasswordEmail_SessionName] != null)
                    dict = CP.Common.Common.Session[CP.Common.sysConst.C_ForgetPasswordEmail_SessionName] as Dictionary<string, object>;

                string yzcode = CP.Common.Common.CreateRandom(CP.Common.sysConst.C_ForgetPasswordEmailCode_Length).ToString();

                int SNSIndex = 1; //本email发送次数
                int SNSIndexAll = 1; //总计本次发送次数

                if (dict.Count > 0)
                {
                    if (Equals(email, dict["UserCode"]))
                        SNSIndex = Convert.ToInt32(dict["Index"]) + 1;

                    SNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
                }
                dict["AllIndex"] = SNSIndexAll;
                dict["Index"] = SNSIndex;
                dict["SNSCode"] = yzcode;
                dict["UserCode"] = email;
                CP.Common.Common.Session[CP.Common.sysConst.C_ForgetPasswordEmail_SessionName] = dict;


                if (SNSIndexAll >= CP.Common.sysConst.C_PostEmailCode_MaxCount)
                {
                    errMsg = "发送验证码次数过多，请30分钟之后重试。";
                    return false;
                }

                string imgUrl = CP.Common.Common.GetUrlAuthorityPath() + "/img/header_logo.png";
                StringBuilder contentBuilder = new StringBuilder();
                contentBuilder.Append(@"
                <style>
			        body{margin: 0;font-family: ""微软雅黑"";font-size: 14px;}
			        .emailbox{width: 60%;margin: 20px auto;border: 1px solid #CCCCCC;background: #F7F7F7;padding: 10px;}
			        .emailtit{overflow: hidden;}
			        .emailtit h3{float: left;margin: 0;line-height: 30px;}
			        .emailtit img{float: right;width: 80px;height: auto;}
			        .s_code{display: block;height: 34px;line-height: 34px;text-align: center;color: #FF4300;
			        text-decoration: none;margin-left: 20px;font-weight: bold;font-size: 22px;text-align:left;}
			        .s_code span{font-size: 14px;font-weight: normal;} 
		        </style>
		        <div class=""emailbox"">
			        <div class=""emailtit"">
				        <h3>知聚网交易平台邮箱验证</h3>
				        <img src=""" + imgUrl + @""" />
			        </div>
			        <p>尊敬的用户，您好！</p>
			        <p style=""margin-left: 20px;"">欢迎您使用知聚网交易平台，请复制下列验证码至找回密码页面完成验证。</p>
			        <div class=""s_code""><span>验证码：</span>" + yzcode + @"<span style=""padding-left:15px;"">序列号：</span>" + SNSIndex + @"</div>
			        <p style=""margin-left: 20px;"">如果您在使用过程中有任何需要帮助的地方，请致电400-010-3123，我们将竭诚为您服务！</p>

		        </div>");

                if (!CP.Common.Common.PostEmail("【知聚网交易平台】修改密码验证码", email, contentBuilder.ToString(), true, out errMsg))
                {
                    errMsg = "系统出错：验证码发送失败。";
                    return false;
                }

                errMsg = "执行成功。";
                return true;
            }

            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region 校验验证码

        /// <summary>
        /// 忘记密码 校验验证码
        /// </summary> 
        public bool CheckForgetPWDVerificationCode(string usercode, string snscode, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
                QueryParams param = new QueryParams();
                param.addWhere(CP.Entiry.sys_UsersAttr.UserCode, "=", "@UC", usercode);

                user.Query(param);

                if (user.ID == null)
                {
                    errMsg = "您输入的用户不存在。";
                    return false;
                }

                return CheckForgetPWDEmail(user.EMail, snscode, out errMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 忘记密码 校验邮箱验证码
        /// </summary>
        /// <returns></returns>
        private bool CheckForgetPWDEmail(string email, string snscode, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_ForgetPasswordEmail_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_ForgetPasswordEmail_SessionName] as Dictionary<string, object>;
            #endregion

            if (dict.Count <= 0) //判断是否发送过验证码
            {
                errMsg = "请先获取验证码。";
                return false;
            }
            if (!Equals(email, dict["UserCode"])) //判断发送的验证邮箱和当前提交的邮箱是否一致
            {
                errMsg = "请先获取验证码。";
                return false;
            }
            if (!Equals(snscode, dict["SNSCode"])) //判断发送的验证码和提交的验证码是否一致
            {
                errMsg = "填写的验证码不正确。";
                return false;
            }

            errMsg = "验证成功";
            return true;
        }
        #endregion

        /// <summary>
        /// 忘记密码 重置密码
        /// </summary>
        /// 
        public bool ForgetPWDResetPassword(string password, string usercode, out string errMsg)
        {
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(password))
                {
                    errMsg = "请输入新密码。";
                    return false;
                }
                #endregion

                Dictionary<string, object> dict = new Dictionary<string, object>();

                if (CP.Common.Common.Session[CP.Common.sysConst.C_ForgetPasswordEmail_SessionName] != null)
                    dict = CP.Common.Common.Session[CP.Common.sysConst.C_ForgetPasswordEmail_SessionName] as Dictionary<string, object>;

                #region 获取session数据

                if (dict.Count <= 0)
                {
                    errMsg = "未获取验证码，请重新执行第一步。";
                    return false;
                }

                #endregion

                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
                QueryParams param = new QueryParams();
                param.addWhere(CP.Entiry.sys_UsersAttr.UserCode, "=", "@UserCode", usercode);
                user.Query(param);
                if (user.ID == null)
                {
                    errMsg = "您输入的用户不存在。";
                    return false;
                }

                return ChangePassWord(user.ID.Value, password, out errMsg);
            }

            catch (Exception)
            {
                throw;
            }

        }

        #endregion


        #region 后台 修改邮箱、手机
        /// <summary>
        /// 邮箱绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="email"></param>
        /// <param name="validatecode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool EmailChange(string usercode, string email, string validatecode, out string errMsg)
        {
            try
            {
                #region 校验验证码
                if (!CheckChangeEmail(usercode, validatecode, out errMsg))
                {
                    return false;
                }
                #endregion

                #region 注册用户
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();

                user.ID = CP.Entiry.Web.WebUser.ID;
                user.EMail = email;

                // 写入数据库
                int uid = user.Update();
                #endregion

                #region 记录注册日志
                CP.DA.Log.WriteInfo("0", "邮箱绑定", CP.Common.LogEnum.LogRunState.Succeed, "绑定邮箱成功。");
                #endregion

                errMsg = "执行成功";
                return true;
            }
            catch (Exception ex)
            {
                CP.DA.Log.WriteInfo("0", "邮箱注册", CP.Common.LogEnum.LogRunState.Failure, ex.Message);

                throw;
            }
        }

        /// <summary>
        /// 手机绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="mobilephone"></param>
        /// <param name="validatecode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool MobileChange(string usercode, string mobilephone, string validatecode, out string errMsg)
        {
            try
            {
                #region 校验验证码
                if (!CheckChangeSNS(usercode, validatecode, out errMsg))
                {
                    return false;
                }
                #endregion

                #region 注册用户
                CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();

                user.ID = CP.Entiry.Web.WebUser.ID;
                user.MobilePhone = mobilephone;

                user.Update();
                #endregion

                #region 记录注册日志
                CP.DA.Log.WriteInfo("0", "手机绑定", CP.Common.LogEnum.LogRunState.Succeed, "手机绑定成功。");
                #endregion

                errMsg = "执行成功";
                return true;
            }
            catch (Exception ex)
            {
                CP.DA.Log.WriteInfo("0", "手机注册", CP.Common.LogEnum.LogRunState.Failure, ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 手机注册发送验证码
        /// </summary> 
        /// <returns></returns>
        public bool PostChangeSNS(string phonenum, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] as Dictionary<string, object>;

            int registerSNSIndex = 1; //本手机发送次数
            int registerSNSIndexAll = 1; //总计本次发送次数

            if (dict.Count > 0)
            {
                if (Equals(phonenum, dict["PhoneNum"]))
                    registerSNSIndex = Convert.ToInt32(dict["Index"]) + 1;

                registerSNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }
            #endregion

            string registerSNSCode = CP.Common.Common.CreateRandom(CP.Common.sysConst.C_BgSNSCode_Length);
            string registerMessage = "【知聚网】您正在更改绑定手机号，验证序列号是：" + registerSNSIndex + "验证码是：" + registerSNSCode + "，请于10分钟内正确输入验证码。";

            dict["AllIndex"] = registerSNSIndexAll;
            dict["Index"] = registerSNSIndex;
            dict["SNSCode"] = registerSNSCode;
            dict["PhoneNum"] = phonenum;
            CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] = dict;

            /// 发送短信
            if (!CP.Common.Common.PostSNS(phonenum, registerMessage))
            {
                errMsg = "系统出错：验证码发送失败。";
                return false;
            }

            errMsg = "执行成功。";
            return true;
        }

        /// <summary>
        /// 邮箱注册发送验证码
        /// </summary> 
        /// <returns></returns>
        public bool PostChangeEmail(string email, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] as Dictionary<string, object>;

            int registerEmailIndex = 1; //本手机发送次数
            int registerEmailIndexAll = 1; //总计本次发送次数

            if (dict.Count > 0)
            {
                if (Equals(email, dict["Email"]))
                    registerEmailIndex = Convert.ToInt32(dict["Index"]) + 1;

                registerEmailIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }
            #endregion

            ///创建验证码
            string registerEmailCode = CP.Common.Common.CreateRandom(CP.Common.sysConst.C_BgEmailCode_Length);

            dict["AllIndex"] = registerEmailIndexAll;
            dict["Index"] = registerEmailIndex;
            dict["SNSCode"] = registerEmailCode;
            dict["Email"] = email;
            CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] = dict;

            #region 发送邮件

            #region 创建验证页面
            string imgUrl = CP.Common.Common.GetUrlAuthorityPath() + "/App/img/logo_o.png";
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.Append(@"
                <style>
			        body{margin: 0;font-family: ""微软雅黑"";font-size: 14px;}
			        .emailbox{width: 60%;margin: 20px auto;border: 1px solid #CCCCCC;background: #F7F7F7;padding: 10px;}
			        .emailtit{overflow: hidden;}
			        .emailtit h3{float: left;margin: 0;line-height: 30px;}
			        .emailtit img{float: right;width: 80px;height: auto;}
			        .s_code{display: block;height: 34px;line-height: 34px;text-align: center;color: #FF4300;
			        text-decoration: none;margin-left: 20px;font-weight: bold;font-size: 22px;text-align:left;}
			        .s_code span{font-size: 14px;font-weight: normal;} 
		        </style>
		        <div class=""emailbox"">
			        <div class=""emailtit"">
				        <h3>知聚网邮箱验证</h3>
				        <img src=""" + imgUrl + @""" />
			        </div>
			        <p>尊敬的用户，您好！</p>
			        <p style=""margin-left: 20px;"">欢迎您使用知聚网，请复制下列验证码至页面完成验证。</p>
			        <div class=""s_code""><span>验证码：</span>" + registerEmailCode + @"<span style=""padding-left:15px;"">序列号：</span>" + registerEmailIndex + @"</div>
			        <p style=""margin-left: 20px;"">如果您在使用过程中有任何需要帮助的地方，请致电4006-111-532，我们将竭诚为您服务！</p>
			        <p style=""text-align: right;margin-right: 20px;"">果子团队敬上</p>
		        </div>");


            #endregion


            //发送邮箱验证码
            bool result = CP.Common.Common.PostEmail("果子知识产权平台--用户注册", email, contentBuilder.ToString(), true, out errMsg);

            if (result == false)
            {
                errMsg = "邮件发送失败。";
                return false;
            }
            #endregion

            return true;
        }


        /// <summary>
        /// 校验手机注册时发送短信次数
        /// </summary>
        /// <returns></returns>
        public bool CheckChangePostSNSCount(string phonenum, out int index)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] as Dictionary<string, object>;

            int registerSNSIndex = 1;
            int registerSNSIndexAll = 1;

            if (dict.Count > 0)
            {
                if (Equals(phonenum, dict["PhoneNum"]))
                    registerSNSIndex = Convert.ToInt32(dict["Index"]) + 1;

                registerSNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }
            #endregion

            /// 返回本次序列号
            index = registerSNSIndex;

            if (registerSNSIndexAll > CP.Common.sysConst.C_PostSNSCode_MaxCount)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 校验邮箱注册时发送邮件次数
        /// </summary>
        /// <returns></returns>
        public bool CheckChangePostEmailCount(string phonenum, out int index)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] as Dictionary<string, object>;

            int registerEmailIndex = 1;
            int registerEmailIndexAll = 1;

            if (dict.Count > 0)
            {
                if (Equals(phonenum, dict["Email"]))
                    registerEmailIndex = Convert.ToInt32(dict["Index"]) + 1;

                registerEmailIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }
            #endregion

            /// 返回本次序列号
            index = registerEmailIndex;

            if (registerEmailIndexAll > CP.Common.sysConst.C_PostEmailCode_MaxCount)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 校验手机注册时验证码
        /// </summary>
        /// <returns></returns>
        public bool CheckChangeSNS(string phonenum, string snscode, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgSNS_SessionName] as Dictionary<string, object>;
            #endregion

            if (dict.Count <= 0) //判断是否发送过验证码
            {
                errMsg = "请先获取验证码。";
                return false;
            }
            if (!Equals(snscode, dict["SNSCode"])) //判断发送的验证码和提交的验证码是否一致
            {
                errMsg = "填写的验证码不正确。";
                return false;
            }

            errMsg = "验证成功";
            return true;
        }


        /// <summary>
        /// 校验邮箱注册时验证码
        /// </summary>
        /// <returns></returns>
        public bool CheckChangeEmail(string email, string snscode, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            #region 获取发送序列号
            if (CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] != null)
                dict = CP.Common.Common.Session[sysConst.C_BgEmail_SessionName] as Dictionary<string, object>;
            #endregion

            if (dict.Count <= 0) //判断是否发送过验证码
            {
                errMsg = "请先获取验证码。";
                return false;
            }
            if (!Equals(snscode, dict["SNSCode"])) //判断发送的验证码和提交的验证码是否一致
            {
                errMsg = "填写的验证码不正确。";
                return false;
            }

            errMsg = "验证成功";
            return true;
        }

        #endregion

        #region 其他

        #region 用户名称是否已存在
        /// <summary>
        /// 判断企业用户是否已存在是否已存在 存在返回true 不存在返回false
        /// </summary>
        /// <param name="usercode">用户名（注册时）</param>
        /// <param name="company">公司名称（注册、认证）</param>
        /// <param name="appaynumber">统一信用代码（注册、认证）</param>
        /// <param name="curruserid">当前用户ID（认证时）</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool IsRegisted(string usercode, string company, string appaynumber, int? curruserid, out string errMsg)
        {
            CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
            CP.EntityBase.QueryParams param = new CP.EntityBase.QueryParams();

            param.addWhere("(");
            if (!string.IsNullOrEmpty(usercode))
            {
                //注册时，用户名检索，区分大小写
                param.addWhere(string.Format(" {0}='{1}' collate Chinese_PRC_CS_AI_WS ", CP.Entiry.sys_UsersAttr.UserCode, usercode));
                param.addOr();
            }

            if (!string.IsNullOrEmpty(company))
            {
                param.addWhere(CP.Entiry.sys_UsersAttr.Company, "=", "@company", company);
                param.addOr();
            }

            if (!string.IsNullOrEmpty(appaynumber))
            {
                param.addWhere(CP.Entiry.sys_UsersAttr.AppayNumber, "=", "@appaynumber", appaynumber);
                param.addOr();
            }
            param.addWhere("1!=1)");


            if (curruserid != 0)
            {
                param.addAnd();
                param.addWhere(CP.Entiry.sys_UsersAttr.ID, "!=", "@ID", curruserid);
            }

            DataTable dt = user.QueryToDataTable(param);

            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(usercode))
                {
                    DataRow[] arr = dt.Select(string.Format("{0}='{1}'", CP.Entiry.sys_UsersAttr.UserCode, usercode)).ToArray();

                    if (arr.Length > 0)
                    {
                        errMsg = "用户名已存在，请更换用户名。";
                        return true;
                    }
                }
                if (!string.IsNullOrEmpty(company))
                {
                    DataRow[] arr = dt.Select(string.Format("{0}='{1}'", CP.Entiry.sys_UsersAttr.Company, company)).ToArray();

                    if (arr.Length > 0)
                    {
                        errMsg = "该企业名称已存在。";
                        return true;
                    }
                }
                if (!string.IsNullOrEmpty(appaynumber))
                {
                    DataRow[] arr = dt.Select(string.Format("{0}='{1}'", CP.Entiry.sys_UsersAttr.AppayNumber, appaynumber)).ToArray();

                    if (arr.Length > 0)
                    {
                        errMsg = "该证件号码已存在。";
                        return true;
                    }
                }
            }

            errMsg = string.Empty;
            return false;

        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="colums"></param>
        /// <returns></returns>
        public DataTable GetUserToDataTable(QueryParams param, string[] colums)
        {
            CP.Entiry.sys_Users user = new CP.Entiry.sys_Users();
            DataTable dt = user.QueryToDataTable(param, colums);

            if (dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["TopImage"].ToString()))
                    dt.Rows[0]["TopImage"] = "~/App/img/head.png";
            }

            return dt;
        }
        #endregion

        #endregion

        #region 注册校验发送验证码 崔萌 2019-12-18 09:51:58
        /// <remarkse>崔萌 2019-12-18 09:51:58</remarkse>
        /// <summary>
        /// 注册校验发送验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        public void RegisterSendVerificationCode(string phoneNumber)
        {
            //1.检查手机号是否注册
            bool isExist = IsHasUser(phoneNumber);

            if (isExist)
            {
                throw new UIException("您输入的手机号已被注册了 。");
            }

            //2.校验发送次数
            if (!CheckRegisterPostSNSCount(phoneNumber))
            {
                throw new UIException("短时间内您点击发送短信验证次数过多,请30分钟后重试。");
            }

            //4.发送验证码
            string errMsg = string.Empty;
            bool result = PostRegisterSNS(phoneNumber, out errMsg);

            if (!result)
            {
                throw new UIException(errMsg);
            }
        }
        #endregion

        #region 注册校验手机发送验证码次数 崔萌 2019-12-18 10:04:31
        /// <remarks>崔萌 2019-12-18 10:04:31</remarks>
        /// <summary>
        /// 注册校验手机发送验证码次数
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>true；false</returns>
        private bool CheckRegisterPostSNSCount(string phoneNumber)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //1.获取发送序列号
            if (Common.Common.Session[sysConst.C_RegisterSNS_SessionName] != null)
            {
                dict = Common.Common.Session[sysConst.C_RegisterSNS_SessionName] as Dictionary<string, object>;
            }

            int registerSNSIndex = 1;
            int registerSNSIndexAll = 1;

            if (dict.Count > 0)
            {
                if (Equals(phoneNumber, dict["PhoneNum"]))
                {
                    registerSNSIndex = Convert.ToInt32(dict["Index"]) + 1;
                }

                registerSNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }

            //2、校验发送次数
            if (registerSNSIndexAll > sysConst.C_PostSNSCode_MaxCount)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 注册发送验证码 崔萌 2019-12-18 10:12:36
        /// <remarks>崔萌 2019-12-18 10:12:36</remarks>
        /// <summary>
        /// 注册发送验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="errMsg">错误消息</param>
        /// <returns>true;false</returns>
        private bool PostRegisterSNS(string phoneNumber, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //1.获取发送序列号
            if (Common.Common.Session[sysConst.C_RegisterSNS_SessionName] != null)
            {
                dict = Common.Common.Session[sysConst.C_RegisterSNS_SessionName] as Dictionary<string, object>;
            }

            int registerSNSIndex = 1; //本手机发送次数
            int registerSNSIndexAll = 1; //总计本次发送次数

            if (dict.Count > 0)
            {
                if (Equals(phoneNumber, dict["PhoneNum"]))
                {
                    registerSNSIndex = Convert.ToInt32(dict["Index"]) + 1;
                }

                registerSNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }

            string registerSNSCode = CP.Common.Common.CreateRandom(sysConst.C_BgSNSCode_Length);
            string registerMessage = "【知聚网】您正在申请平台帐号，验证序列号是：" + registerSNSIndex + "验证码是：" + registerSNSCode + "，请于10分钟内正确输入验证码。";

            dict["AllIndex"] = registerSNSIndexAll;
            dict["Index"] = registerSNSIndex;
            dict["SNSCode"] = registerSNSCode;
            dict["PhoneNum"] = phoneNumber;
            Common.Common.Session[sysConst.C_RegisterSNS_SessionName] = dict;

            /// 发送短信
            if (!Common.Common.PostSNS(phoneNumber, registerMessage))
            {
                errMsg = "系统出错：验证码发送失败。";
                return false;
            }

            errMsg = "执行成功。";
            return true;
        }
        #endregion

        #region 注册 崔萌 2019-12-18 15:51:24
        /// <remarkse>崔萌 2019-12-18 15:51:24</remarkse>
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="verificationCode">验证码</param>
        /// <param name="password">密码</param>
        public void Register(string phoneNumber, string verificationCode, string password)
        {
            //1.检查用户是否已存在
            if (IsHasUser(phoneNumber))
            {
                throw new UIException("您输入的手机号已被注册。");
            }

            //2.校验验证码
            CheckSNS(phoneNumber, verificationCode, sysConst.C_RegisterSNS_SessionName);

            //3.新增用户
            sys_Users users = new sys_Users()
            {
                UserCode = phoneNumber,
                UserName = phoneNumber,
                MobilePhone = phoneNumber,
                RoleId = sysConst.RegisterRoleID,
                PassWord = Common.Common.EncryptToSHA1(password),
                LoginType = Convert.ToInt32(LoginType.Forepart),
                Integrals = sysConst.GetIntegrals_Register,
                AccountType = AccountType.Personal
            };

            int userId = users.InsertReID();
        }
        #endregion

        #region 校验验证码 崔萌 2019-12-18 15:59:42
        /// <remarks>崔萌 2019-12-18 15:59:42</remarks>
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="verificationCode">验证码</param>
        /// <param name="SessionName">Session名称</param>
        private void CheckSNS(string phoneNumber, string verificationCode, string SessionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //1. 获取发送序列号
            if (Common.Common.Session[SessionName] != null)
            {
                dict = Common.Common.Session[SessionName] as Dictionary<string, object>;
            }

            //2.判断是否发送过验证码
            if (dict.Count <= 0)
            {
                throw new UIException("请先获取验证码。");
            }

            //3.判断发送的验证的手机和当前提交的手机是否一致
            if (!Equals(phoneNumber, dict["PhoneNum"]))
            {
                throw new UIException("请先获取验证码。");
            }

            //4.判断发送的验证码和提交的验证码是否一致
            if (!Equals(verificationCode, dict["SNSCode"]))
            {
                throw new UIException("填写的验证码不正确。");
            }
        }
        #endregion

        #region 找回密码发送验证码 崔萌 2019-12-17 16:09:30
        /// <remarkse>崔萌 2019-12-17 16:09:30</remarkse>
        /// <summary>
        /// 找回密码发送验证码
        /// </summary>
        /// <param name="mobilePhone"></param>
        public void RetrievePasswordPostSNS(string mobilePhone)
        {
            //1.校验该手机号是否注册
            if (!IsHasUser(mobilePhone))
            {
                throw new UIException("该手机号未注册会员。");
            }

            //2.发送验证码
            string errMsg = string.Empty;
            bool result = RetrievePasswordSNS(mobilePhone, out errMsg);
            if (!result)
            {
                throw new UIException(errMsg);
            }
        }
        #endregion

        #region 检查手机号是否注册 崔萌 2019-12-17 16:50:51
        /// <remarks>崔萌 2019-12-17 16:50:51</remarks>
        /// <summary>
        /// 检查手机号是否注册
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>true:存在;false:不存在</returns>
        private bool IsHasUser(string phoneNumber)
        {
            //1.查询
            QueryParams param = new QueryParams();
            param.addWhere(sys_UsersAttr.IsDelete, "=", "@IsDelete", IsDelete.N);
            param.addAnd();
            param.addWhere(sys_UsersAttr.UserCode, "=", "@UserCode", phoneNumber);

            sys_Users users = new sys_Users();
            users.Query(param);

            return users.ID == null ? false : true;
        }
        #endregion

        #region 找回密码发送验证码 崔萌 2019-12-17 16:23:31
        /// <remarks>崔萌 2019-12-17 16:23:31</remarks>
        /// <summary>
        /// 找回密码发送验证码
        /// </summary> 
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        private bool RetrievePasswordSNS(string phoneNumber, out string errMsg)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //1.获取发送序列号
            if (Common.Common.Session[sysConst.C_RetrievePasswordSNS_SessionName] != null)
            {
                dict = Common.Common.Session[sysConst.C_RetrievePasswordSNS_SessionName] as Dictionary<string, object>;
            }

            int registerSNSIndex = 1; //本手机发送次数
            int registerSNSIndexAll = 1; //总计本次发送次数

            if (dict.Count > 0)
            {
                if (Equals(phoneNumber, dict["PhoneNum"]))
                {
                    registerSNSIndex = Convert.ToInt32(dict["Index"]) + 1;
                }

                registerSNSIndexAll = Convert.ToInt32(dict["AllIndex"]) + 1;
            }

            if (registerSNSIndexAll > sysConst.C_PostSNSCode_MaxCount)
            {
                throw new UIException("短时间内您点击发送短信验证次数过多,请30分钟后重试。");
            }

            string registerSNSCode = CP.Common.Common.CreateRandom(sysConst.C_BgSNSCode_Length);
            string registerMessage = "【知聚网】您正在找回密码，验证序列号是：" + registerSNSIndex + "验证码是：" + registerSNSCode + "，请于10分钟内正确输入验证码。";

            dict["AllIndex"] = registerSNSIndexAll;
            dict["Index"] = registerSNSIndex;
            dict["SNSCode"] = registerSNSCode;
            dict["PhoneNum"] = phoneNumber;
            Common.Common.Session[sysConst.C_RetrievePasswordSNS_SessionName] = dict;

            /// 发送短信
            if (!Common.Common.PostSNS(phoneNumber, registerMessage))
            {
                errMsg = "系统出错：验证码发送失败。";
                return false;
            }

            errMsg = "执行成功。";
            return true;
        }
        #endregion

        #region 确认找回密码 崔萌 2019-12-18 17:01:45
        /// <remarkse>崔萌 2019-12-18 17:01:45</remarkse>
        /// <summary>
        /// 确认找回密码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="verificationCode">验证码</param>
        /// <param name="password">密码</param>
        public void ConfirmRetrievePassword(string phoneNumber, string verificationCode, string password)
        {
            //1.检查用户是否已存在
            if (!IsHasUser(phoneNumber))
            {
                throw new UIException("您输入的手机号未被注册。");
            }

            //2.校验验证码
            CheckSNS(phoneNumber, verificationCode, sysConst.C_RetrievePasswordSNS_SessionName);

            //3.修改密码
            UpdatePassword(phoneNumber, password);
        }
        #endregion

        #region 修改密码 崔萌 2019-12-18 17:10:22
        /// <remarks>崔萌 2019-12-18 17:10:22</remarks>
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="password">密码</param>
        private void UpdatePassword(string userCode, string password)
        {
            QueryParams param = new QueryParams();
            param.addWhere(sys_UsersAttr.UserCode, "=", "@UserCode", userCode);

            sys_Users users = new sys_Users()
            {
                PassWord = Common.Common.EncryptToSHA1(password)
            };

            users.Update(param);
        }
        #endregion

        

        #region 增加用户积分 崔萌 2020-3-18 15:11:04
        /// <remarkse>崔萌 2020-3-18 15:11:04</remarkse>
        /// <summary>
        /// 增加用户积分
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="integrals">积分</param>
        /// <param name="userId">用户主键</param>
        public void AddUserIntegral(DbTransaction tran, int integrals, int userId)
        {
            try
            {
                string sql = string.Format(" UPDATE {0} SET {1}=(ISNULL({1}, 0) +{2}) WHERE {3}={4} ",
                sys_Users.PhysicsTable, sys_UsersAttr.Integrals, integrals, sys_UsersAttr.ID, userId);

                DBHelper.ExecuteNonQuery(tran, sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 当前用户积分余额是否充足 崔萌 2020-3-18 15:36:31
        /// <remarks>崔萌 2020-3-18 15:36:31</remarks>
        /// <summary>
        /// 当前用户积分余额是否充足
        /// </summary>
        /// <param name="source">积分来源</param>
        /// <returns>余额充足:true;余额不足:false</returns>
        public bool IsHaveIngegralBalance(IntegralsSource source)
        {
            //1.获取相应的扣除积分
            int integeals = GetIntegralsBySource(source);

            //2.查询当前用户积分
            sys_Users users = GetUserInfo(Convert.ToInt32(WebUser.ID));

            if (users.Integrals < integeals)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 获取用户信息 崔萌 2020-3-18 15:41:51
        /// <remarks>崔萌 2020-3-18 15:41:51</remarks>
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public sys_Users GetUserInfo(int userId)
        {
            QueryParams param = new QueryParams();

            param.addWhere(sys_UsersAttr.IsDelete, "=", "@IsDelete", IsDelete.N);
            param.addAnd();
            param.addWhere(sys_UsersAttr.ID, "=", "@ID", userId);

            sys_Users users = new sys_Users();
            users.Query(param);

            return users;
        }
        #endregion

        #region 根据积分来源获取积分 崔萌 2020-3-18 15:52:13
        /// <remarkse>崔萌 2020-3-18 15:52:13</remarkse>
        /// <summary>
        /// 根据积分来源获取积分
        /// </summary>
        /// <param name="source">积分来源</param>
        private int GetIntegralsBySource(IntegralsSource source)
        {
            int integrals = 0;

            //1.获取相应的扣除积分
            switch (source)
            {
                case IntegralsSource.ReservePA:
                    integrals = sysConst.GetIntegrals_ReservePA;
                    break;
                case IntegralsSource.SellerInfo:
                    integrals = sysConst.GetIntegrals_SellerInfo;
                    break;
                case IntegralsSource.FeeInfo:
                    integrals = sysConst.GetIntegrals_FeeInfo;
                    break;
                case IntegralsSource.LawState:
                    integrals = sysConst.GetIntegrals_LawState;
                    break;
                default:
                    break;
            }

            return integrals;
        }
        #endregion

        #region 修改密码 崔萌 2020-4-2 09:59:36
        /// <remarks>崔萌 2020-4-2 09:59:36</remarks>
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        public void ModifyPassword(string oldPassword, string newPassword)
        {
            //1.校验数据
            if (WebUser.ID == null)
            {
                throw new UIException("用户失效,请先登录。");
            }
            if (!WebUser.PassWord.Equals(Common.Common.EncryptToSHA1(oldPassword)))
            {
                throw new UIException("原密码不正确。");
            }
            if (WebUser.PassWord.Equals(Common.Common.EncryptToSHA1(newPassword)))
            {
                throw new UIException("新密码与原密码相同，请重新输入新密码。");
            }

            //2.修改密码
            sys_Users user = new sys_Users()
            {
                ID = WebUser.ID,
                PassWord = Common.Common.EncryptToSHA1(newPassword)
            };

            user.Update();

            //3.更新WebUser
            sys_Users webUser = new sys_Users();
            webUser.Query(WebUser.ID.ToString());

            if (webUser.ID != null)
            {
                WebUser.SetWebUser(webUser);
            }
        }
        #endregion

        #region 修改邮箱 崔萌 2020-4-2 14:14:31
        /// <remarks>崔萌 2020-4-2 14:14:31</remarks>
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="code">验证码</param>
        public void ModifyEmail(string email, string code)
        {
            //1.校验邮箱验证码
            string errMsg = string.Empty;
            bool result = CheckChangeEmail(email, code, out errMsg);
            if (!result)
            {
                throw new UIException(errMsg);
            }

            //2.更新邮箱
            sys_Users user = new sys_Users()
            {
                ID = WebUser.ID,
                EMail = email
            };

            user.Update();

            //4.更新WebUser
            sys_Users webUser = new sys_Users();
            webUser.Query(WebUser.ID.ToString());

            if (webUser.ID != null)
            {
                WebUser.SetWebUser(webUser);
            }
        }
        #endregion

        #region 修改手机 崔萌 2020-4-2 15:01:34
        /// <remarks>崔萌 2020-4-2 15:01:34</remarks>
        /// <summary>
        /// 修改手机
        /// </summary>
        /// <param name="mobilePhone">手机号</param>
        /// <param name="code">验证码</param>
        public void ModifyMobilePhone(string mobilePhone, string code)
        {
            //1.校验邮箱验证码
            string errMsg = string.Empty;
            bool result = CheckChangeSNS(mobilePhone, code, out errMsg);
            if (!result)
            {
                throw new UIException(errMsg);
            }

            //2.更新邮箱
            sys_Users user = new sys_Users()
            {
                ID = WebUser.ID,
                MobilePhone = mobilePhone
            };

            user.Update();

            //4.更新WebUser
            sys_Users webUser = new sys_Users();
            webUser.Query(WebUser.ID.ToString());

            if (webUser.ID != null)
            {
                WebUser.SetWebUser(webUser);
            }
        }
        #endregion

        #region 修改QQ 崔萌 2020-4-2 15:29:10
        /// <remarks>崔萌 2020-4-2 15:29:10</remarks>
        /// <summary>
        /// 修改QQ
        /// </summary>
        /// <param name="qq">qq</param>
        //public void ModifyQQ(string qq)
        //{
        //    //1..校验认证的QQ是否存在于b_Transaction_PA_LinkMain
        //    UserAuthenticationCore core = new UserAuthenticationCore();
        //    core.IsHaveQQToLinkMain(qq);

        //    DbTransaction tran = DBHelper.GetTransaction(sysConst.ReadConnStrName);

        //    try
        //    {
        //        //2.校验当前用户是否发布交易
        //        b_UserAuthentication authentication = new b_UserAuthentication()
        //        {
        //            MemberID = WebUser.ID,
        //            MemberCode = WebUser.UserCode + "会员中心-我的资料-安全设置-绑定QQ",
        //            QQ = qq
        //        };
        //        core.CheckQQAndMobilePhone(tran, authentication);

        //        //3.修改QQ
        //        sys_Users users = new sys_Users()
        //        {
        //            ID = WebUser.ID,
        //            QQ = qq
        //        };

        //        users.Update(tran);

        //        tran.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        if (tran != null)
        //        {
        //            tran.Rollback();
        //        }
        //        throw;
        //    }
        //    finally
        //    {
        //        if (tran != null)
        //        {
        //            tran.Dispose();
        //        }
        //    }

        //    //4.更新WebUser
        //    sys_Users webUser = new sys_Users();
        //    webUser.Query(WebUser.ID.ToString());

        //    if (webUser.ID != null)
        //    {
        //        WebUser.SetWebUser(webUser);
        //    }

        //}
        #endregion
    }
}
