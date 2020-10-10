using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackParse.Core.DropGame
{
    class DropGameParser
    {
        public string[] GetAppUrls(IHtmlDocument document)
        {
            List<string> urls = new List<string>();
            var tags = document.QuerySelectorAll("a")
                .OfType<IHtmlAnchorElement>()
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("short-link"));

            foreach (var tag in tags)
            {
                urls.Add(tag.Href);
            }

            return urls.ToArray();
        }
        public Tuple<string,string> GetVersion(IHtmlDocument document)
        {
            List<string> urls = new List<string>();
            string findstr = "Версия";
            var version = document.QuerySelectorAll("li")
                .Select(s=>s.TextContent)
                .Where(w => w.Contains(findstr))
                .Select(s=>s.Substring(findstr.Length+6))
                .FirstOrDefault()
                .Replace(" ", "");

            var name = document.QuerySelectorAll("div")
                .Where(w => w.ClassName != null && w.ClassName.Contains("fheader fx-1"))
                .Select(s=>s.TextContent)
                .FirstOrDefault();

            return new Tuple<string, string>(name,version);
        }
    }
}
