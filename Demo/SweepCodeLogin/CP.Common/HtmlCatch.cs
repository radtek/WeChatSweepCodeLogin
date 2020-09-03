using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using System.Net;
using Sgml;

namespace CP.Common
{
    public class HtmlCatch
    {

        public static HtmlCatch Instancel
        {
            get { return new HtmlCatch(); }
        }

        #region 读取html页面内容
        /// <summary>
        /// 读取html页面内容
        /// </summary>
        /// <param name="uri">网址</param> 
        /// <returns></returns> 
        public string GetWellFormedHTML(string uri)
        {
            using (StreamReader sReader = null)//读取字节流
            {
                StringWriter sw = null;//写入字符串
                SgmlReader reader = null;//sgml读取方法
                XmlTextWriter writer = null;//生成xml数据流
                try
                {
                    if (uri == String.Empty)
                        throw new Exception("关键参数 uri 为空");

                    WebClient webclient = new WebClient();
                    webclient.Encoding = Encoding.UTF8;

                    //页面内容
                    string strWebContent = webclient.DownloadString(uri);


                    reader = new SgmlReader();
                    reader.DocType = "HTML";
                    reader.InputStream = new StringReader(strWebContent);


                    sw = new StringWriter();
                    writer = new XmlTextWriter(sw);
                    writer.Formatting = Formatting.Indented;
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Whitespace)
                        {
                            writer.WriteNode(reader, true);
                        }
                    }

                    return sw.ToString();

                }
                catch (Exception exp)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 根据xpath语法抓取网页内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string CatchHTMLByXPath(string html, string xpath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                XPathDocument doc = new XPathDocument(new StringReader(html.ToString()));

                XPathNavigator nav = doc.CreateNavigator();
                XPathNodeIterator nodes = nav.Select(xpath);
                while (nodes.MoveNext())
                {
                    sb.Append(nodes.Current.Value + " ");
                }
                return sb.ToString().Replace(" ", "");
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取html原文件中 所有指定标签集合
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public List<string> GetTags(string html, string tag)
        {
            StringReader strReader = new StringReader(html);
            int lowerThanCharCounter = 0;
            int lowerThanCharPos = 0;
            Stack<int> tagPos = new Stack<int>();
            List<string> taglist = new List<string>();
            int i = 0;
            while (true)
            {
                try
                {
                    int intCharacter = strReader.Read();
                    if (intCharacter == -1) break;

                    char convertedCharacter = Convert.ToChar(intCharacter);

                    if (lowerThanCharCounter > 0)
                    {
                        if (convertedCharacter == '>')
                        {
                            lowerThanCharCounter--;

                            string biaoqian = html.Substring(lowerThanCharPos, i - lowerThanCharPos + 1);
                            if (biaoqian.StartsWith(string.Format("<{0}", tag)))
                            {
                                tagPos.Push(lowerThanCharPos);
                            }
                            if (biaoqian.StartsWith(string.Format("</{0}", tag)))
                            {
                                if (tagPos.Count < 1)
                                    continue;
                                int tempTagPos = tagPos.Pop();
                                string strdiv = html.Substring(tempTagPos, i - tempTagPos + 1);
                                taglist.Add(strdiv);
                            }
                        }
                    }

                    if (convertedCharacter == '<')
                    {
                        lowerThanCharCounter++;
                        lowerThanCharPos = i;
                    }
                }
                finally
                {
                    i++;
                }
            }
            return taglist;
        }

        #endregion
    }
}
