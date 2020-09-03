<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using CP.Entiry;
using CP.EntityBase;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.SessionState;
using CP.Core.Web;

public class Upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string _action = HttpContext.Current.Request.Params["action"];

            switch (_action.ToLower())
            {
                case "verifycode":
                    VerifyCode(context);
                    break;
                case "p_bandorganization"://省市区
                    BandOrganization(context);
                    break;
                case "p_loadcountry"://国家
                    Country(context);
                    break;
                case "editorfile":
                    EditorFile(context);
                    break;
                case "managerfile"://浏览文件处理 文件空间
                    ManagerFile(context);
                    break;
                case "uploadfile":
                    UpLoadFile(context);
                    break;
                default: //普通上传
                    break;
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // Response.End() 引发此异常. 无需处理.
        }
        catch (Exception ex)
        {
            throw new CP.Core.Web.UIException(ex.ToString());
        }
    }



    /// <summary>
    /// 验证码
    /// </summary>
    /// <param name="context"></param>
    public void VerifyCode(HttpContext context)
    {
        int codeW = 80;
        int codeH = 22;
        int fontSize = 16;
        string chkCode = string.Empty;
        //颜色列表，用于验证码、噪线、噪点 
        System.Drawing.Color[] color = { System.Drawing.Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
        //字体列表，用于验证码 
        string[] font = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
        //验证码的字符集，去掉了一些容易混淆的字符 
        char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
        Random rnd = new Random();
        //生成验证码字符串 
        for (int i = 0; i < CP.Common.sysConst.C_RegisterCode_Length; i++)
        {
            chkCode += character[rnd.Next(character.Length)];
        }
        //写入Session
        CP.Common.Common.Session[CP.Common.sysConst.C_Register_SessionName] = chkCode;
        //创建画布
        Bitmap bmp = new Bitmap(codeW, codeH);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        //画噪线 
        for (int i = 0; i < 1; i++)
        {
            int x1 = rnd.Next(codeW);
            int y1 = rnd.Next(codeH);
            int x2 = rnd.Next(codeW);
            int y2 = rnd.Next(codeH);
            Color clr = color[rnd.Next(color.Length)];
            g.DrawLine(new Pen(clr), x1, y1, x2, y2);
        }
        //画验证码字符串 
        for (int i = 0; i < chkCode.Length; i++)
        {
            string fnt = font[rnd.Next(font.Length)];
            Font ft = new Font(fnt, fontSize);
            Color clr = color[rnd.Next(color.Length)];
            g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18 + 2, (float)0);
        }
        //画噪点 
        for (int i = 0; i < 100; i++)
        {
            int x = rnd.Next(bmp.Width);
            int y = rnd.Next(bmp.Height);
            Color clr = color[rnd.Next(color.Length)];
            bmp.SetPixel(x, y, clr);
        }
        //清除该页输出缓存，设置该页无缓存 
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
        context.Response.Expires = 0;
        context.Response.CacheControl = "no-cache";
        context.Response.AppendHeader("Pragma", "No-Cache");
        //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
        MemoryStream ms = new MemoryStream();
        try
        {
            bmp.Save(ms, ImageFormat.Png);
            context.Response.ClearContent();
            context.Response.ContentType = "image/Png";
            context.Response.BinaryWrite(ms.ToArray());
        }
        finally
        {
            //显式释放资源 
            bmp.Dispose();
            g.Dispose();
        }
    }



    #region 省 市 区

    private void BandOrganization(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            string _type = context.Request.Params["type"];
            string _provinc = context.Request.Params["provinc"];
            string _city = context.Request.Params["city"];
            string _district = context.Request.Params["district"];

            string textfield, valfield;

            CP.EntityBase.Entity entity = new CP.EntityBase.Entity();
            CP.EntityBase.QueryParams parmas = new CP.EntityBase.QueryParams();

            switch (_type)
            {
                case "province":
                    entity = new CP.Entiry.sys_Province();
                    textfield = "ProvinceName";
                    valfield = "ProvinceID";
                    break;
                case "city":
                    entity = new CP.Entiry.sys_City();
                    parmas.addWhere("provinceID=@provinceID", "@provinceID", _provinc);
                    textfield = "CityName";
                    valfield = "CityID";
                    break;
                case "district":
                    entity = new CP.Entiry.sys_District();
                    parmas.addWhere("cityID=@cityID", "@cityID", _city);
                    textfield = "DistrictName";
                    valfield = "DistrictID";
                    break;
                default:
                    throw new CP.Core.Web.UIException("无效的类型。");
            }

            DataTable dt = entity.QueryToDataTable(parmas);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(dt));
            json.Add("textField", textfield);
            json.Add("valField", valfield);
        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion


    #region 国家

    private void Country(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {

            CP.Entiry.sys_Dictionaries Dictionaries = new sys_Dictionaries();
            QueryParams param = new QueryParams();
            param.addWhere(CP.Entiry.sys_DictionariesAttr.Type, "=", "@Country", "Country");
            DataTable dt = Dictionaries.QueryToDataTable(param);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(dt));

        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion


    #region Helper
    public class NameSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.FullName.CompareTo(yInfo.FullName);
        }
    }

    public class SizeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.Length.CompareTo(yInfo.Length);
        }
    }

    public class TypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.Extension.CompareTo(yInfo.Extension);
        }
    }
    #endregion
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region 编辑器上传处理 崔萌 2019-11-13 08:54:43
    /// <remarks>崔萌 2019-11-13 08:54:43</remarks>
    /// <summary>
    /// 编辑器上传处理
    /// </summary>
    /// <param name="context"></param>
    private void EditorFile(HttpContext context)
    {
        string json = string.Empty;
        try
        {
            //1.校验上传的文件
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                throw new UIException("请选择要上传文件。");
            }

            //2.校验新闻类型参数
            string type = context.Request.Params["type"];
            if (string.IsNullOrEmpty(type))
            {
                throw new UIException("不正确的新闻类型参数");
            }

            string errMsg = string.Empty;
            string oldPath = string.Empty;
            string filePath = string.Empty;

            //type=recommend(中介推荐-介绍)，其它的就是新闻资讯的 崔萌 2020-2-18 16:01:52
            if (type.Equals("recommend"))
            {
                //CP.Core.IntermediaryRecommendCore core = new CP.Core.IntermediaryRecommendCore();
                //filePath = core.GetImgPath(imgFile, oldPath, out errMsg);
            }
            else
            {
                //CP.Core.NewsCore core = new CP.Core.NewsCore();
                //filePath = core.GetNewsImgPath(imgFile, type, oldPath, out errMsg);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new UIException(errMsg);
            }

            json = "{\"error\":0,\"url\":\"" + filePath.Replace("~", CP.Common.Common.GetUrlAuthorityPath()) + "\"}";
        }
        catch (Exception ex)
        {
            json = "{\"error\":1,\"url\":\"" + ex.ToString() + "\"}";
        }
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(json);
        context.Response.End();
    }
    #endregion

    #region 浏览文件处理 崔萌 2019-11-13 09:12:41
    /// <remarks>崔萌 2019-11-13 09:12:41</remarks>
    /// <summary>
    /// 浏览文件处理
    /// </summary>
    /// <param name="context"></param>
    private void ManagerFile(HttpContext context)
    {
        try
        {
            //根目录路径，相对路径
            String rootPath = CP.Common.sysConst.B_News_Directory + "/"; //站点目录+上传目录
                                                                         //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = CP.Common.sysConst.B_News_Directory.Replace("~", CP.Common.Common.GetUrlAuthorityPath()) + "/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            String dirPath = HttpContext.Current.Server.MapPath(rootPath);
            String dirName = context.Request.QueryString["dir"];

            //根据path参数，设置各路径和URL
            String path = context.Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = context.Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                context.Response.Write("Access is not allowed.");
                context.Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                context.Response.Write("Parameter is not valid.");
                context.Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                context.Response.Write("Directory does not exist.");
                context.Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            //switch (order)
            //{
            //    case "size":
            //        Array.Sort(dirList, new NameSorter());
            //        Array.Sort(fileList, new SizeSorter());
            //        break;
            //    case "type":
            //        Array.Sort(dirList, new NameSorter());
            //        Array.Sort(fileList, new TypeSorter());
            //        break;
            //    case "name":
            //    default:
            //        Array.Sort(dirList, new NameSorter());
            //        Array.Sort(fileList, new NameSorter());
            //        break;
            //}

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }

            context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            context.Response.Write(CP.Common.JSONHelper.ObjectToJSON(result));
        }
        catch (Exception ex)
        {
            //showError(context, ex.ToString());
        }
        context.Response.End();
    }
    #endregion

    #region 上传文件处理 崔萌 2019-11-13 14:56:32
    /// <remarks>崔萌 2019-11-13 14:56:32</remarks>
    /// <summary>
    /// 上传文件处理
    /// </summary>
    /// <param name="context"></param>
    private void UpLoadFile(HttpContext context)
    {
        string json = string.Empty;
        try
        {
            string errMsg = string.Empty;
            string oldPath = string.Empty;
            string filePath = string.Empty;

            string _delfile = context.Request.Params["DelFilePath"];

            HttpPostedFile upfile = context.Request.Files["Filedata"];
            if (upfile == null)
            {
                throw new UIException("请选择要上传文件。");
            }

            int modulartype = 0;
            if (!int.TryParse(context.Request.Params["modulartype"], out modulartype))
            {
                throw new UIException("不正确的模块类型参数");
            }

            //-1.中介推荐，1.公司新闻，2.行业新闻，3.科技政策，4.知识百科，5.会员权益管理
            string newsType = context.Request.Params["newstype"];

            if (string.IsNullOrEmpty(newsType))
            {
                throw new UIException("不正确的类型参数");
            }

            if (newsType.Equals("-1"))
            {
                //CP.Core.IntermediaryRecommendCore core = new CP.Core.IntermediaryRecommendCore();

                if (!string.IsNullOrEmpty(_delfile))
                {
                    oldPath = _delfile.Replace(",", "~");
                }

                //上传图片
                //filePath = core.GetImgPath(upfile, oldPath, out errMsg);

                if (string.IsNullOrEmpty(filePath))
                {
                    throw new UIException(errMsg);
                }

                json = "{\"status\":1,\"path\":\"" + filePath.Replace("~", "") + "\"}";
            }
            else if (newsType.Equals("5"))
            {
                //CP.Core.MemberIntegralsCore core = new CP.Core.MemberIntegralsCore();

                if (!string.IsNullOrEmpty(_delfile))
                {
                    oldPath = _delfile.Replace(",", "~");
                }

                //上传图片
                //filePath = core.GetImgPath(upfile, oldPath, out errMsg);

                if (string.IsNullOrEmpty(filePath))
                {
                    throw new UIException(errMsg);
                }

                json = "{\"status\":1,\"path\":\"" + filePath.Replace("~", "") + "\"}";
            }
            else
            {
                //CP.Core.NewsCore core = new CP.Core.NewsCore();

                if (!string.IsNullOrEmpty(_delfile))
                {
                    oldPath = _delfile.Replace(",", "~");
                }

                //上传图片
                //filePath = core.GetNewsImgPath(upfile, newsType, oldPath, out errMsg);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new UIException(errMsg);
            }

            json = "{\"status\":1,\"path\":\"" + filePath.Replace("~", "") + "\"}";

        }
        catch (Exception ex)
        {
            json = "{\"error\":1,\"url\":\"" + ex.ToString() + "\"}";
        }
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(json);
        context.Response.End();
    }


    #endregion
}