using D3NsCore.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace D3NsCore.Tools
{
    internal class HttpAccess
    {
        private const string UserAgent = "DirectDDNS";

        public string ProxyServer { get; set; }

        public string GetString(string url, params HttpHeader[] headers)
        {
            var wc = new WebClient { Headers = { ["User-Agent"] = UserAgent } };
            wc.Proxy = new WebProxy();
            if (ProxyServer != null) wc.Proxy = new WebProxy(new Uri(ProxyServer));
            foreach (var header in headers) wc.Headers.Add(header.Name, header.Value);
            var str = wc.DownloadString(url);
            return str;
        }

        public string PutString(string url, string content, string contentType = "application/x-www-form-urlencoded;charset=UTF-8", params HttpHeader[] headers)
        {
            var wc = new WebClient { Headers = { ["User-Agent"] = UserAgent, ["Content-Type"] = contentType } };
            if (ProxyServer != null) wc.Proxy = new WebProxy(new Uri(ProxyServer));
            foreach (var header in headers) wc.Headers.Add(header.Name, header.Value);

            try
            {
                var str = wc.UploadString(url, "PUT", content);
                return str;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var str = Encoding.UTF8.GetString(e.Response.GetResponseStream().ToBytes());
                    throw new WebException(str, e);
                }
                throw;
            }
        }

        public T GetJsonAnon<T>(T anon, string url, params HttpHeader[] headers)
        {
            return JsonConvert.DeserializeAnonymousType(GetString(url, headers), anon);
        }
    }
}