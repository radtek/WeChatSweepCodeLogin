using Sgml;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace CP.Common
{
    public class HttpHepler
    {
        #region POST 
        public static string PostRequest(string postData, string requestUrlString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            //向服务端请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            myRequest.Headers.Add("X-Requested-With:XMLHttpRequest");

            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            //将请求的结果发送给客户端(界面、应用)
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }
        public static string PostRequest(string requestUrlString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            //向服务端请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            myRequest.Headers.Add("X-Requested-With:XMLHttpRequest");

            Stream newStream = myRequest.GetRequestStream();

            newStream.Close();
            //将请求的结果发送给客户端(界面、应用)
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }

        #region Get请求 崔萌 2020-3-6 17:36:46
        /// <remarkse>崔萌 2020-3-6 17:36:46</remarkse>
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="requestUrlString">请求地址</param>
        /// <returns></returns>
        public static string GetRequest(string requestUrlString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            //向服务端请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);

            myRequest.Method = "Get";
            myRequest.Timeout = -1;
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            Stream newStream = myResponse.GetResponseStream();

            StreamReader reader = new StreamReader(newStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        #endregion
        #endregion


        #region GET
        public static string GetRequestByCookieString(string requestUrlString)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
            myRequest.Method = "Get";
            //myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Timeout = -1;
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";


            System.Net.HttpWebResponse myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
            System.IO.Stream myStream = myResponse.GetResponseStream();
            System.Text.Encoding myencoding = System.Text.Encoding.GetEncoding("GBK");
            System.IO.StreamReader mystreamreader = new System.IO.StreamReader(myStream, myencoding);


            string data = mystreamreader.ReadToEnd();

            using (StringWriter sw = new StringWriter()) //写入字符串 
            {
                SgmlReader reader = null;//sgml读取方法
                XmlTextWriter writer = null;//生成xml数据流

                reader = new SgmlReader();
                //reader.DocType = "HTML";
                reader.InputStream = new StringReader(data);

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
        }

        #endregion
    }
}
