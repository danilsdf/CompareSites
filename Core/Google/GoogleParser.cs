using AngleSharp.Html.Dom;
using BackParse.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace BackParse.Core.Google
{
    class GoogleParser
    {
        readonly GoogleSettings settings;
        public GoogleParser(GoogleSettings setting)
        {
            settings = setting;
        }
        public async Task<SearchResult> GetResultAsync(string AppName)
        {
            string changedName = AppName.Replace(" ", "+");

            var currentUrl = settings.BaseUrl.Replace("*Name*", changedName);


            var client = new RestClient(currentUrl);
            client.UserAgent = "User";
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", settings.Rapidapi_host);
            request.AddHeader("x-rapidapi-key", settings.Rapidapi_key);
            IRestResponse response = await client.ExecuteAsync(request);

            JObject search = JObject.Parse(response.Content);
            ArrayResult results = JsonConvert.DeserializeObject<ArrayResult>(search.ToString());
            if (results.Results is null) return null;
            return results.Results.FirstOrDefault();
        }
        public async Task<string> GetResult(string AppName)
        {
            string uriString = "https://www.google.com/search?client=opera&hs=1Do&sxsrf=ALeKk00HgXFqHWi4nh0Esk5_kTVk4rEZcA%3A1604762557103&ei=vbumX9HwBciFrwS2np_wBA&q=*Name*&oq=*Name*&gs_lcp=CgZwc3ktYWIQAzoHCCMQsQIQJzoFCAAQywE6CAgAEAgQDRAeOgQIIxAnOgYIABAWEB46BggAEAcQHlDiPFjKiMcBYO6cxwFoAnAAeACAAcUBiAGFCpIBBDEwLjOYAQCgAQGgAQKqAQdnd3Mtd2l6wAEB&sclient=psy-ab&ved=0ahUKEwiRw6ix3vDsAhXIwosKHTbPB04Q4dUDCAw&uact=5";
            string changedName = AppName.Replace(" ", "+").Replace(":", "%3A");

            var currentUrl = uriString.Replace("*Name*", changedName);

            HttpClient client = new HttpClient();

            var response = client.GetAsync(currentUrl).Result;
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
        public string GetAppUrl(IHtmlDocument document, string name)
        {
            var tags = document.QuerySelectorAll("div")
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("yuRUbf"));

            string appstring = null;

            return appstring;
        }
        public async Task<string> GetTranslateAsync(string text)
        {
            var client = new RestClient($"https://microsoft-azure-translation-v1.p.rapidapi.com/translate?from=ru&to=en&text={text}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "microsoft-azure-translation-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "f3c3fd7010msh12f3c95b0b41b52p12dbd8jsnf07fab2c9479");
            request.AddHeader("accept", "application/json");
            IRestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        public async Task<string> GetEngNameAsync(string name)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(await GetTranslateAsync(name));

            string json = JsonConvert.SerializeXmlNode(doc);
            JObject search = JObject.Parse(json);
            TranslateResult Str = JsonConvert.DeserializeObject<TranslateResult>(search.ToString());
            return Str.String.Text;
        }
    }
}
