using BackParse.Core.GooglePlay;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackParse.Core.HtmlLoaders
{
    class GPHtmlLoader
    {
        readonly HttpClient client;
        readonly string url;
        public GPHtmlLoader(GooglePlaySettings setting)
        {
            client = new HttpClient();
            url = $@"{setting.BaseUrl}/{setting.Prefix}";
        }
        public async Task<string> GetSourceByName(string AppName)
        {
            string changedName = AppName.Replace(" ","%20");

            var currentUrl = url.Replace("AppName", changedName);
            var response =  client.GetAsync(currentUrl).Result;
            string source = null;

            if(response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
        public async Task<string> GetSourceBylink(string link)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            var response = client.GetAsync(link).Result;
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
