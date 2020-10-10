using AngleSharp.Html.Dom;
using System;
using System.Linq;

namespace BackParse.Core.Google
{
    class GoogleParser 
    {
        public string GetAppString(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("a")
                .OfType<IHtmlAnchorElement>()
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("poRVub"));

            string appUrl = tags.FirstOrDefault().Href;

            return appUrl;
        }

        public Tuple<string,string> GetDataVersion(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("div")
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("hAyfc"))
                .Select(s => s.TextContent)
                .Distinct()
                .ToArray();

            string date = string.Empty, 
                version = string.Empty;

            foreach (var tag in tags)
            {
                if (tag.Contains("Updated"))
                    date = tag.Substring(7);
                else if(tag.Contains("Current Version"))
                    version = tag.Substring(15).Replace(" ", "");
            }
            return new Tuple<string, string>(date, version);
        }
    }
}
