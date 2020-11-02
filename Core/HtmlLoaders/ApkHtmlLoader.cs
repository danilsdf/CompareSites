using BackParse.Core.APK;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BackParse.Core.HtmlLoaders
{
    class ApkHtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public ApkHtmlLoader(ApkSetting setting)
        {
            client = new HttpClient();
            url = $@"{setting.BaseUrl}";
        }
        public async Task<string> GetSourceByName(string AppName)
        {
            string changedName = AppName.Replace(" ", "+");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            var currentUrl = url.Replace("*AppName*", changedName);
            var response = client.GetAsync(currentUrl).Result;

            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
