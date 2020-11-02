using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BackParse.Core.APK
{
    class ApkParser
    {
        Dictionary<string, string> changename = new Dictionary<string, string>();
        readonly HttpClient client;
        readonly string url;

        public ApkParser(ApkSetting setting)
        {
            client = new HttpClient();
            url = $@"{setting.BaseUrl}";
            AddToDict();
        }
        public string GetUrl(IHtmlDocument document,string name)
        {

            if (changename.ContainsKey(name))
                name = changename[name];

            var url = document.Links
                .OfType<IHtmlAnchorElement>()
                .Where(w=>w.Title != null && w.Title.ToLower().Replace(" ","").StartsWith(name.ToLower().Replace(" ", "")))
                .Select(s => s.Href.Substring(8));

            return url.FirstOrDefault();
        }
        public string GetVersion(IHtmlDocument document)
        {
            string str = "Version";
            var version = document.QuerySelectorAll("table")
                .Where(w => w.ClassName != null && w.ClassName.Contains("atable is-striped"))
                .Select(s => s.TextContent)
                .Select(s => s[(s.IndexOf(str)+str.Length)..s.IndexOf("(")].Replace(" ", ""))
                .FirstOrDefault();

            return version;
        }
        private void AddToDict()
        {
            changename.Add("Math | Пазлы и математическая игра", "Math | Riddles and Puzzles Math Games");
            changename.Add("RuneScape", "RuneScape Mobile");
            changename.Add("Zooba: Битва животных", "Zooba: Free-for-all Zoo Combat Battle Royale Games");
            changename.Add("Найти различия 300 уровней", "Find the difference 300 level Spot the differences");
            changename.Add("Call of Duty: Mobile", "Call of Duty®: Mobile");
            changename.Add("NITRO NATION™ 6", "Nitro Nation Drag & Drift");
            changename.Add("Day R Premium", "Day R Survival – Apocalypse, Lone Survivor and RPG");
            changename.Add("Dead By Daylight", "DEAD BY DAYLIGHT MOBILE - Silent Hill Update");
            changename.Add("Южный Парк: Разрушитель Мобил", "South Park: Phone Destroyer™ - Battle Card Game");
            changename.Add("Free Fire", "Garena Free Fire: BOOYAH Day");
            changename.Add("Lords Mobile: Война Королевств", "Lords Mobile Kingdom Wars");
        }

    }
}
