using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// UrlRewriter 的摘要说明
/// </summary>

public class UrlRewriter : IHttpHandler, System.Web.SessionState.IRequiresSessionState //实现“IHttpHandler”接口  
{
    public UrlRewriter()
    {
        //  
        // TODO: 在此处添加构造函数逻辑  
        //  
    }
    public void ProcessRequest(HttpContext Context)
    {
        try
        {
            //取得原始URL屏蔽掉参数  
            string Url = Context.Request.RawUrl;
            if (string.IsNullOrEmpty(Url.Replace("/", "")))
            {
                if (Context.Request.UrlReferrer != null)
                {
                    Url = Context.Request.UrlReferrer.LocalPath;
                }
            }


            string suburl = Url.ToString().Substring(Url.LastIndexOf("/"));

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(CP.Common.sysConst.C_UrlRewriter_XMLFilePath));

            System.Xml.XmlNodeList nodelist = xmlDoc.SelectNodes("mapping/node");

            foreach (System.Xml.XmlNode node in nodelist)
            {
                System.Xml.XmlElement element = (System.Xml.XmlElement)node;

                var _nl = element.GetElementsByTagName("regex");

                if (_nl.Count <= 0)
                    throw new Exception("伪静态路径配置文件有错误, 有数据行缺少必要字段regex");
                string regex = _nl[0].InnerText;

                _nl = element.GetElementsByTagName("parms");

                if (_nl.Count <= 0)
                    throw new Exception("伪静态路径配置文件有错误, 有数据行缺少必要字段parms");
                string parms = _nl[0].InnerText;

                _nl = element.GetElementsByTagName("url");

                if (_nl.Count <= 0)
                    throw new Exception("伪静态路径配置文件有错误, 有数据行缺少必要字段url");
                string url = _nl[0].InnerText;


                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex);

                if (r.IsMatch(suburl))
                {
                    string[] arr = Url.ToString().Substring(Url.LastIndexOf("/")).Split('-');
                    string[] parmsarr = parms.Split(';');

                    string urlparms = string.Empty;

                    for (int i = 0; i < parmsarr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(parmsarr[i]))
                            continue;

                        string[] _arr = parmsarr[i].Split('-');
                        int _index = int.Parse(_arr[1]);

                        urlparms += string.Format("{0}={1}", _arr[0], arr[_index]);

                        if (i < parmsarr.Length - 1)
                            urlparms += "&";
                    }

                    string realpath = string.Format("{0}{1}",
                        url,
                        string.IsNullOrEmpty(urlparms) ? "" : "?" + urlparms);

                    Context.RewritePath(realpath);//(RewritePath 用在无 Cookie 会话状态中。)  
                    Context.Server.Execute(realpath);

                    break;
                }
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            // Response.End() 引发此异常. 无需处理.
        }
        catch
        {
            Context.Response.Redirect(Context.Request.Url.ToString());
        }
    }
    /// <summary>  
    /// 实现“IHttpHandler”接口所必须的成员  
    /// </summary>  
    /// <value></value>   
    public bool IsReusable
    {
        get { return false; }
    }
}
