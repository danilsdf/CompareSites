using System;
using System.Collections.Generic;
using System.Text;

namespace BackParse.Core.Google
{
    class GoogleSettings
    {
        public string BaseUrl { get; set; } = @"https://google-search3.p.rapidapi.com/api/v1/search/q=*Name*+4pda";
        public string Rapidapi_host { get; set; } = "google-search3.p.rapidapi.com";
        public string Rapidapi_key { get; set; } = "ApiKey";
    }
}
