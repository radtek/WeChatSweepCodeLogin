using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Xml;

namespace CP.Common
{
    /// <summary>
    /// Common 的摘要说明。
    /// </summary>
    public class Common
    {
        #region 框架参数及函数
        #region 数据类型。
        /// <summary>
        /// string
        /// </summary>
        public const int AppString = 1;
        /// <summary>
        /// int
        /// </summary>
        public const int AppInt = 2;
        /// <summary>
        /// float
        /// </summary>
        public const int AppFloat = 3;
        /// <summary>
        /// AppBoolean
        /// </summary>
        public const int AppBoolean = 4;
        /// <summary>
        /// AppDouble
        /// </summary>
        public const int AppDouble = 5;
        /// <summary>
        /// AppDate
        /// </summary>
        public const int AppDate = 6;
        /// <summary>
        /// AppDateTime
        /// </summary>
        public const int AppDateTime = 7;
        /// <summary>
        /// AppMoney
        /// </summary>
        public const int AppMoney = 8;
        /// <summary>
        /// 率百分比。
        /// </summary>
        public const int AppRate = 9;
        #endregion

        #region 日期格式
        /// <summary>
        /// 系统定义日期时间格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string SysDataTimeFormat
        {
            get
            {
                return "yyyy-MM-dd HH:mm:ss";
            }
        }
        public static string SysDataFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        public static string SysDataFormatCN
        {
            get
            {
                return "yyyy年MM月dd日";
            }
        }
        public static string SysDatatimeFormatCN
        {
            get
            {
                return "yyyy年MM月dd日 HH时mm分";
            }
        }
        #endregion

        public static bool StringToBoolean(string str)
        {
            if (str == null || str == "" || str == ",nbsp;")
                return false;

            if (str == "0" || str == "1")
            {
                if (str == "0")
                    return false;
                else
                    return true;
            }
            else if (str == "true" || str == "false")
            {
                if (str == "false")
                    return false;
                else
                    return true;

            }
            else if (str == "是" || str == "否")
            {
                if (str == "否")
                    return false;
                else
                    return true;
            }
            else
                throw new Exception("@要转换的[" + str + "]不是bool 类型");
        }
        #endregion

        #region HTTP 参数
        /// <summary>
        /// 当前请求是否为 Ajax发送的请求 如果是为: True
        /// </summary>
        public static bool IsAjaxRequest
        {
            get
            {
                //如果Http头中不包含 X-Requested-With 则为非AJAX请求
                if (string.IsNullOrEmpty(CurrentHttpContext.Request.Headers["X-Requested-With"]))
                    return false;
                else
                    return true;
            }
        }

        public static HttpSessionState Session
        {
            get { return CurrentHttpContext.Session; }
        }

        public static HttpContext CurrentHttpContext
        {
            get { return HttpContext.Current; }
        }

        public static string IPAddress
        {
            get { return CurrentHttpContext.Request.UserHostAddress; }
        }
        #endregion

        #region IO 处理
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="file">路径</param>
        /// <param name="Doc">内容</param>
        public static void WriteFile(string file, string Doc)
        {
            System.IO.StreamWriter sr = null;

            try
            {
                //sr = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("GB2312"));
                sr = new System.IO.StreamWriter(file, true, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                //throw new Exception("@文件：" + file + ",错误:" + ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Write(Doc);
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file">路径</param>
        /// <returns>内容</returns>
        public static string ReadTextFile(string file)
        {

            return ReadTextFile(file, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file">路径</param>
        /// <returns>内容</returns>
        public static string ReadTextFile(string file, System.Text.Encoding encode)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(file, encode); // 文件流.
            string doc = read.ReadToEnd();  //读取完毕。
            read.Close(); // 关闭。
            return doc;
        }

        /// <summary>
        /// 将内容保存为文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="doc">内容</param>
        /// <returns>执行结果</returns>
        public static bool SaveAsFile(string filePath, string doc)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false);
            sw.Write(doc);
            sw.Close();
            return true;
        }





        /// <summary>
        /// 移动目录内的文件到另一目录   
        /// 返回 以后后的文件路径 如果返回 string.Empty 则源文件不存在
        /// </summary>
        /// <param name="filename">需要移动的文件路径</param>
        ///<param name="newFolder">移动到的文件夹路径 不包含文件名</param>
        /// <returns></returns>
        public static string MoveFile(string filename, string newFolder)
        {

            try
            {
                if (!File.Exists(filename))
                {
                    return string.Empty;
                }
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(newFolder)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(newFolder));
                }

                FileInfo MoveFile = new FileInfo(filename);
                MoveFile.MoveTo(System.Web.HttpContext.Current.Server.MapPath(newFolder) + "\\" + MoveFile.Name);

                return newFolder + "/" + MoveFile.Name;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region 配置文件
        public static string AppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #region 读写 ini 配置文件
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        #endregion
        #endregion

        /// <summary>
        /// 生成GUID
        /// </summary>  
        /// <returns></returns>
        public static String CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取随机生成的数字
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static String CreateRandom(int length)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                ///System.Threading.Thread.Sleep(1000);
                Random rd = new Random(Guid.NewGuid().GetHashCode());
                int rdnum = rd.Next(0, 10);
                buffer.Append(rdnum);
            }
            return buffer.ToString();
        }
        /// <summary>
        /// 获取随机生成的字符串
        /// </summary> 
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static String CreateRandomStr(int length)
        {
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode());
                int rdnum = rd.Next(0, str.Length - 1);
                buffer.Append(str[rdnum]);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 根据设定区间获取随机数
        /// </summary>  
        /// <returns></returns>
        public static int CreateRandomByRange(int min, int max)
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int rdnum = rd.Next(min, max);
            return rdnum;
        }

        /// <summary>
        /// 判断是否全部是汉字
        /// </summary>
        /// <param name="htmlstr"></param>
        /// <returns></returns>
        public static bool CheckIsChinese(string htmlstr)
        {
            char[] chs = htmlstr.ToCharArray();
            foreach (char c in chs)
            {
                int i = c.ToString().Length;
                if (i == 1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptToSHA1(string str)
        {
            try
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                byte[] str1 = Encoding.UTF8.GetBytes(str);
                byte[] str2 = sha1.ComputeHash(str1);
                sha1.Clear();
                (sha1 as IDisposable).Dispose();
                return Convert.ToBase64String(str2);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前请求的基础路径 不含 /APP/ /MANAGER/
        /// http://localhost:3172
        /// </summary>
        /// <returns></returns>
        public static string GetUrlAuthorityPath()
        {
            try
            {
                return CurrentHttpContext.Request.Url.GetLeftPart(UriPartial.Scheme) + CurrentHttpContext.Request.Url.Authority;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前请求的基础路径 含 /APP/ /MANAGER/
        /// http://localhost:3172/APP 或 http://localhost:3172/MANAGER
        /// </summary>
        /// <returns></returns>
        public static string GetUrlBasePath()
        {
            try
            {
                string[] _s = CurrentHttpContext.Request.Url.AbsolutePath.Split('/');

                string siteName = string.Empty;
                if (_s.Length >= 3)
                    siteName = "/" + _s[1];
                else
                    siteName = CurrentHttpContext.Request.Url.AbsolutePath.Substring(0, CurrentHttpContext.Request.Url.AbsolutePath.LastIndexOf('/'));

                return CurrentHttpContext.Request.Url.GetLeftPart(UriPartial.Scheme) + CurrentHttpContext.Request.Url.Authority + siteName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前请求的基础路径 不含 /APP/ /MANAGER/
        /// http://localhost:3172/ 或 http://localhost:3172/
        /// </summary>
        /// <returns></returns>
        public static string GetUrlWebSite()
        {
            try
            {
                return CurrentHttpContext.Request.Url.GetLeftPart(UriPartial.Scheme) + CurrentHttpContext.Request.Url.Authority;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 根据url地址获取返回的数据
        /// </summary>
        /// <returns></returns>
        public static string GetContentByUrl(string Url, Encoding encoding)
        {
            string strBuff = "";
            Uri httpURL = new Uri(Url);
            ///HttpWebRequest类继承于WebRequest，并没有自己的构造函数，需通过WebRequest的Creat方法 建立，并进行强制的类型转换   
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            ///通过HttpWebRequest的GetResponse()方法建立HttpWebResponse,强制类型转换   
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
            ///GetResponseStream()方法获取HTTP响应的数据流,并尝试取得URL中所指定的网页内容   
            ///若成功取得网页的内容，则以System.IO.Stream形式返回，若失败则产生ProtoclViolationException错 误。在此正确的做法应将以下的代码放到一个try块中处理。这里简单处理   
            Stream respStream = httpResp.GetResponseStream();
            ///返回的内容是Stream形式的，所以可以利用StreamReader类获取GetResponseStream的内容，并以   
            //StreamReader类的Read方法依次读取网页源程序代码每一行的内容，直至行尾（读取的编码格式：UTF8）   
            StreamReader respStreamReader = new StreamReader(respStream, encoding);
            strBuff = respStreamReader.ReadToEnd();
            return strBuff;

        }


        /// <summary> 
        /// GET请求与获取结果 
        /// </summary> 
        public static bool HttpGetDownLoad(string url, string savePath, out string errMsg)
        {
            bool result = true;
            errMsg = string.Empty;
            string tempPath = System.IO.Path.GetDirectoryName(savePath);
            if (!Directory.Exists(tempPath))
                System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(savePath) + ".temp"; //临时文件
            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);    //存在则删除
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                FileStream fs = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = stream.Read(bArr, 0, (int)bArr.Length);
                if (size <= 0)
                {
                    fs.Close();
                    stream.Close();
                    return false;
                }

                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = stream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                stream.Close();

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                System.IO.File.Move(tempFile, savePath);
            }
            catch (Exception ex)
            {
                errMsg = ex.ToString();
                result = false;
                //if (((System.Net.WebException)(ex)).Status == WebExceptionStatus.ProtocolError)
                //{
                //    Stream myResponseStream = ((System.Net.WebException)(ex)).Response.GetResponseStream();
                //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                //    result = myStreamReader.ReadToEnd();
                //}
            }
            return result;
        }
        /// <summary>
        /// 验证文件格式是否正确
        /// </summary>
        /// <param name="FormFile">文件</param>
        /// <param name="_FileExtension">格式限制数组</param>
        /// <param name="_fileType">文件格式</param>
        /// <returns></returns>
        public static bool ValidationFileFormat(HttpPostedFile _formFile, string[] _FileExtension, out string _fileType)
        {
            if (_formFile == null)
            {
                _fileType = "";
                return false;
            }

            string _fileName = _formFile.FileName;//获取文件名
            _fileType = Path.GetExtension(_fileName).ToLower();//获取文件后缀
            if (_fileName.IndexOf(' ') > -1)
            {
                _fileName = _fileName.Substring(_fileName.LastIndexOf(' ') + 1);
            }
            else if (_fileName.IndexOf('/') > -1)
            {
                _fileName = _fileName.Substring(_fileName.LastIndexOf('/') + 1);
            }
            bool _upFile = string.Join(",", _FileExtension).Contains(_fileType);//是否允许上传
            return _upFile;
        }

        /// <summary>
        /// 上传文件 
        /// </summary>
        /// <param name="FormFile">文件</param>
        /// <param name="Url">路径</param>
        /// <param name="FileName">文件名称</param>
        /// <param name="_FileExtension">文件允许类型</param>
        /// <returns></returns>
        public static bool UpLoadFile(HttpPostedFile formFile, string fileName, string dict, out string uploadPath, out string errMsg)
        {
            try
            {
                HttpPostedFile postedFile = formFile;
                if (postedFile == null)
                {
                    errMsg = "文件不能为空";
                    uploadPath = "";
                    return false;
                }
                #region 拼接文件路径
                string uploadDir = dict + "/";
                uploadPath = uploadDir + fileName;//拼接文件的完整路径
                #endregion
                #region 判断路径是否存在
                if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadDir)) == false)
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadDir));
                }
                #endregion
                #region Save
                postedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                #endregion
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                uploadPath = "";
                return false;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="toaddress"></param>
        /// <param name="content"></param>
        /// <param name="IsBodyHtml"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool PostEmail(string title, string toaddress, string content, bool IsBodyHtml, out string errMsg)
        {
            try
            {
                string formAddress = CP.Common.sysConst.Email_Interface_Key + "@" + CP.Common.sysConst.Email_Interface_PostFix;

                if (CP.Common.sysConst.Email_Interface_SSL == CP.Common.IsFlag.Y)
                {
                    System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
                    mail.To = toaddress;
                    mail.From = formAddress;// "lanyi0306@126.com";
                    mail.Subject = title;// "test";
                    mail.BodyFormat = System.Web.Mail.MailFormat.Html;
                    mail.Body = content;// "test222";


                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //身份验证 
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mail.From); //邮箱登录账号，这里跟前面的发送账号一样就行 
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", CP.Common.sysConst.Email_Interface_Password); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码 
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", CP.Common.sysConst.Email_Interface_Port);//端口 
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SSL加密 
                    System.Web.Mail.SmtpMail.SmtpServer = CP.Common.sysConst.Email_Interface_Host;// "smtp.126.com"; //企业账号用smtp.exmail.qq.com 
                    System.Web.Mail.SmtpMail.Send(mail);
                }
                else
                {
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress(formAddress);
                    mailMsg.To.Add(toaddress);
                    mailMsg.Subject = title;
                    mailMsg.Body = content;
                    mailMsg.IsBodyHtml = IsBodyHtml;

                    //发件方服务器地址 
                    NetworkCredential credetial = new NetworkCredential();
                    credetial.UserName = CP.Common.sysConst.Email_Interface_Key;
                    credetial.Password = CP.Common.sysConst.Email_Interface_Password;

                    SmtpClient client = new SmtpClient();
                    client.Host = CP.Common.sysConst.Email_Interface_Host;
                    client.Port = CP.Common.sysConst.Email_Interface_Port;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = false;
                    client.Credentials = credetial;
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    client.Send(mailMsg);
                }

                errMsg = "发送成功";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;

                Log4.Log4Error("发送邮件失败，" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="content">发送的内容</param> 
        /// <returns></returns>
        public static bool PostSNS(string phone, string content)
        {
            try
            {
                string param = "action=send&userid=" + sysConst.SNS_Interface_Enterprise + "&account=" + sysConst.SNS_Interface_Key + "&password=" + sysConst.SNS_Interface_Password + "&content="
                        + content + "&mobile=" + phone;


                byte[] bs = Encoding.UTF8.GetBytes(param);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(CP.Common.sysConst.SNS_Interface_Host);
                req.Method = CP.Common.sysConst.SNS_Interface_Method;
                req.ContentType = CP.Common.sysConst.SNS_Interface_ContentType;
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.UTF8);
                    string xml = sr.ReadToEnd().Trim();
                    //byte[] buffer = Encoding.GetEncoding("GBK").GetBytes(xml);
                    //string _xml = Encoding.UTF8.GetString(buffer);

                    //System.IO.StreamReader xmlStreamReader = sr;
                    //xmlDoc.Load(xmlStreamReader); 
                    xmlDoc.LoadXml(xml);
                }

                if (xmlDoc == null)
                {
                    Log4.Log4Error("短信接口，请求发生异常,无任何返回值。");
                    return false;
                }
                else
                {
                    String message = xmlDoc.GetElementsByTagName("message").Item(0).InnerText.ToString();
                    if (message != "ok")
                    {
                        Log4.Log4Error("短信接口，" + message);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log4.Log4Error("短信接口，" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 生成序列   
        /// </summary>
        /// <returns></returns>
        public static string CreateSequence(string Type)
        {
            try
            {
                Dictionary<string, object> ProcType = new Dictionary<string, object>();
                ProcType.Add("Type", Type);

                DataTable dt = DBHelper.ExecuteScalarProc(sysConst.ReadConnStrName, "P_Sequence", ProcType);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["Code"].ToString();
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 获取DLL版本号和DLL名称
        /// </summary>
        /// <param name="dllPath">DLL路径</param>
        /// <returns></returns>
        public static string DLLVersionInfo(string dllPath)
        {
            try
            {
                string info = "";
                System.Diagnostics.FileVersionInfo _VersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Web.HttpContext.Current.Server.MapPath(dllPath));
                info += _VersionInfo.ProductVersion + ",";
                info += _VersionInfo.ProductName;
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取文件最后更新时间
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string FileUpdateTime(string filePath)
        {
            try
            {
                System.IO.FileInfo timeInfo = new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(filePath));
                string info = timeInfo.LastWriteTime.ToString();
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 创建xml文件和xml文件的上层文件夹
        /// </summary>
        /// <param name="directory">创建xml文件的路径</param>
        /// <param name="XmlElement">xml根节点的名称</param>
        /// <returns></returns>
        public static bool CreateXmlAndFolder(string directory, string XmlElement, out string errMsg)
        {
            try
            {
                string folderDir = directory.Substring(0, directory.LastIndexOf("/"));

                XmlDocument xml = new XmlDocument();

                XmlElement rootEle;
                //判断文件夹是否存在
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(folderDir)))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(folderDir));
                    directoryInfo.Create();
                }
                //创建一行声明信息，添加到xml文档顶部
                System.Xml.XmlDeclaration decl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                xml.AppendChild(decl);

                //创建根节点
                rootEle = xml.CreateElement(XmlElement);
                xml.AppendChild(rootEle);
                xml.Save(System.Web.HttpContext.Current.Server.MapPath(directory));

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                directory = "";
                return false;
            }
        }


        #region 清除HTML标记且返回相应的长度
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring.Replace("&emsp;", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        #endregion

        /// <summary>
        /// 金额转大写
        /// </summary>
        /// <param name="LowerMoney"></param>
        /// <returns></returns>
        public static string MoneyToUpper(double dou)
        {

            // 大写数字数组
            string[] num = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            // 数量单位数组，个位数为空
            string[] unit = { "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆" };
            string d = Math.Round(dou, 2).ToString(); ;
            string zs = string.Empty;// 整数
            string xs = string.Empty;// 小数
            int i = d.IndexOf(".");
            string str = string.Empty;
            if (i > -1)
            {
                // 仅考虑两位小数
                zs = d.Substring(0, i);
                xs = d.Substring(i + 1, d.Length - i - 1);
                str = "元";
                if (xs.Length == 1)
                    str = str + xs + "角";
                else if (xs.Length == 2)
                    str = str + xs.Substring(0, 1) + "角" + xs.Substring(1, 1) + "分";
            }
            else
            {
                zs = d;
                str = "元整";//元整
            }
            // 处理整数部分
            if (!string.IsNullOrEmpty(zs))
            {
                i = 0;
                // 从整数部分个位数起逐一添加单位
                foreach (char s in zs.Reverse())
                {
                    str = s.ToString() + unit[i] + str;
                    i++;
                }
            }
            // 将阿拉伯数字替换成中文大写数字
            for (int m = 0; m < 10; m++)
            {
                str = str.Replace(m.ToString(), num[m]);
            }
            // 替换零佰、零仟、零拾之类的字符
            str = Regex.Replace(str, "[零]+仟", "零");
            str = Regex.Replace(str, "[零]+佰", "零");
            str = Regex.Replace(str, "[零]+拾", "零");
            str = Regex.Replace(str, "[零]+亿", "亿");
            str = Regex.Replace(str, "[零]+万", "万");
            str = Regex.Replace(str, "[零]+", "零");
            str = Regex.Replace(str, "亿[万|仟|佰|拾]+", "亿");
            str = Regex.Replace(str, "万[仟|佰|拾]+", "万");
            str = Regex.Replace(str, "仟[佰|拾]+", "仟");
            str = Regex.Replace(str, "佰拾", "佰");
            str = Regex.Replace(str, "[零]+元整", "元整");
            return str;


        }

        /// <summary>
        /// 年+日期（MM-dd....）日期转化
        /// </summary>
        /// <param name="year"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FormarteDateTime(int year, string time)
        {
            return DateTime.Parse(year + "-" + time);
        }

        /// <summary>
        /// 校验手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool CheckMobile(string mobile)
        {
            Regex mobileRegex = new Regex("^(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$");
            if (mobileRegex.IsMatch(mobile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 校验QQ号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool CheckQQ(string qq)
        {
            Regex mobileRegex = new Regex("[1-9][0-9]{4,14}");
            if (mobileRegex.IsMatch(qq))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 通过ip获取城市 崔萌 2019-12-23 14:32:38
        /// <remarkse>崔萌 2019-12-23 14:32:38</remarkse>
        /// <summary>
        /// 通过ip获取城市
        /// </summary>
        /// <param name="ip">ip</param>
        public static DataTable GetCityByIP(string ip)
        {
            try
            {
                //1.组装URL
                string url = "http://ip.ws.126.net/ipquery?ip=" + ip;//使用126接口获取物理地址

                //2.获取请求到的信息
                string content = HttpHepler.GetRequestByCookieString(url);

                //3.将json串转为datatable
                DataTable dt = null;
                if (content != null)
                {
                    int start = content.IndexOf('{');
                    int end = content.Length - 1 - start;

                    string jsondata = content.Substring(start, end);//截取json串

                    dt = JSONHelper.JsonToDataTable(jsondata);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("通过ip获取城市异常：" + ex.Message);
            }
        }
        #endregion
    }
}
