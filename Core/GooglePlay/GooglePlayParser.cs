using AngleSharp.Html.Dom;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackParse.Core.GooglePlay
{
    class GooglePlayParser 
    {
        public string GetAppString(IHtmlDocument document, string name)
        {
            var tags = document.QuerySelectorAll("a")
                .OfType<IHtmlAnchorElement>()
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("poRVub")).Select(s => s.Href);

            string appstring;
            if (name == "DOOM II android" || name == "DOOM android")
                appstring = tags.ToArray()[1];
            else
                appstring = tags.FirstOrDefault();
            return appstring;
        }

        public string GetVersion(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("div")
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("hAyfc"))
                .Select(s => s.TextContent)
                .Distinct()
                .ToArray()
                .Where(w =>w.Contains("Current Version"))
                .Select(s=>s.Substring(15).Replace(" ", ""));

            return tags.FirstOrDefault();
        }
    }
}
