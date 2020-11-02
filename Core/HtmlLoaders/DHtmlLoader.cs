using BackParse.Core.DropGame;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BackParse.Core.HtmlLoaders
{
    class DHtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public DHtmlLoader(DropGameSettings setting)
        {
            client = new HttpClient();
            url = $@"{setting.BaseUrl}/{setting.TypeApp}/{setting.Prefix}";
        }
        public async Task<string> GetSourceById(int pageid)
        {
            var currentUrl = url.Replace("Number", $"{pageid}");
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
