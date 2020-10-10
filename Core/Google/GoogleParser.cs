using AngleSharp.Html.Dom;
using System;
using System.Linq;

namespace BackParse.Core.Google
{
    class GoogleParser 
    {
        public string GetAppString(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().Where(tag => tag.ClassName != null && tag.ClassName.Contains("poRVub"));

            string appUrl = tags.FirstOrDefault().Href;

            return appUrl;
        }

        public Tuple<string,string> GetDataVersion(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("span").Where(tag => tag.ClassName != null && tag.ClassName.Contains("htlgb")).Select(s => s.TextContent).Distinct().ToArray();
            
            string date = tags[0],version;
            if (tags.Contains("Varies with device"))
                version = tags[1];
            else
                version = tags[3];
            return new Tuple<string, string>(date, version);
        }
    }
}
