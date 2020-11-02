using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackParse.Core.FPDA
{
    class FPDAParser
    {
        public string GetVersion(IHtmlDocument document)
        {
            string searchstr = "Версия";
            var tags = document.QuerySelectorAll("b")
                .Where(m => m.TextContent.Contains(searchstr)).ToArray();
            if (tags.Length == 0) {
                Console.WriteLine("Tags is empty");
                return null;
        } 
            string first = tags[0].TextContent;
            string version;
            if(first.Contains('('))
            version = first[(searchstr.Length+1)..first.IndexOf('(')].Replace(" ","");
            else
                version = first.Substring(searchstr.Length + 1);
            return version;
        }
    }
}
