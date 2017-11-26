using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeamonTool.Helper;

namespace DeamonTool
{
    public class HttpRequestHelper
    {
        public static string PostHttp(string url, string postData)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.Method = "post";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                System.IO.Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
                return responseContent;
            }
            catch(Exception ex)
            {
                Log.LogError(ex.Message);
            }
            return string.Empty;

        }



        public static string GetHttp(string url)
        {

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 50000;
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                var result = httpWebResponse.StatusCode == HttpStatusCode.OK;
                httpWebResponse.Close();
                streamReader.Close();

                return responseContent;
            }
            catch(Exception ex)
            {
                Log.LogError(ex.Message);
            }
            return string.Empty;
        }

    }
}
