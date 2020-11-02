using AngleSharp.Html.Dom;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackParse.Core.DropGame
{
    class DropGameParser
    {
        public string[] GetAppUrls(IHtmlDocument document)
        {
            var tags = document.QuerySelectorAll("a")
                .OfType<IHtmlAnchorElement>()
                .Where(tag => tag.ClassName != null && tag.ClassName.Contains("short-link"))
                .Select(s => s.Href);

            return tags.ToArray();
        }
        public string GetName(IHtmlDocument document)
        {
            var name = document.QuerySelectorAll("div")
                .Where(w => w.ClassName != null && w.ClassName.Contains("fheader fx-1"))
                .Select(s=>s.TextContent)
                .FirstOrDefault();
            if (name.Contains("("))
                return name.Substring(0, name.IndexOf("("));
            else return name;
        }
        public string GetVersion(IHtmlDocument document)
        {
            string findstr = "Версия";
            var version = document.QuerySelectorAll("li")
                .Select(s => s.TextContent)
                .Where(w => w.Contains(findstr))
                .Select(s => s.Substring(findstr.Length + 6).Replace(" ", ""));

            return version.FirstOrDefault();
        }
    }
}
