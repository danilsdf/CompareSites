using BackParse.Core.FPDA;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BackParse.Core.HtmlLoaders
{
    class FHtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public FHtmlLoader(FPDASettings setting)
        {
            client = new HttpClient();
            url = $@"{setting.BaseUrl}/{setting.Prefix}";
        }
    }
}
