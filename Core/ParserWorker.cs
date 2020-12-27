using AngleSharp.Html.Parser;
using AngleSharp.Text;
using BackParse.Core.APK;
using BackParse.Core.DropGame;
using BackParse.Core.FPDA;
using BackParse.Core.Google;
using BackParse.Core.GooglePlay;
using BackParse.Core.HtmlLoaders;
using BackParse.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BackParse.Core
{
    class ParserWorker
    {
        #region Filds
        GooglePlayParser gparser;
        GooglePlaySettings gparserSetting;

        DropGameParser dparser;
        DropGameSettings dparserSetting;

        GoogleParser googleparser;
        GoogleSettings googleparserSetting;

        FPDAParser fpdaparser;
        FPDASettings fpdaparserSetting;

        ApkParser apkParser;
        ApkSetting apkSetting;

        GPHtmlLoader GPloader;
        DHtmlLoader Dloader;
        FHtmlLoader Floader;
        ApkHtmlLoader ApkLoader;
        readonly string[] Games = new string[] { "Minecraft Android", "reBrawl", "Grim Fandango Remastered",
            "Кейс Standoff 2 симулятор", "Angry Birds Rio (Секреты + МОД)", "2048","Dead Island: Survivors","Durango: Wild Lands","InstaXtreme (InstaMOD)",
        "Celebrity Voice Changer PRO","CoronaVirus Outbreak","ВКоннект","Sova Lite","WhatsApp DELTA","Central for DayZ (Pro Unlocker + MOD)","Rectangle for KWGT","Andromeda for KWGT",
        "LDoE Bases","VK iMod"};
        readonly string[] MissGames = new string[] { "DOOM android", "S.T.A.L.K.E.R. Android (Stalker Mobile)",
            "Tom Clancy's H.A.W.X","Angry Birds Rio","Adobe Photoshop Touch"};
        readonly List<string> strs = new List<string>();

        #endregion

        #region Properties
        public GooglePlayParser GParser
        {
            get
            {
                return gparser;
            }
            set
            {
                gparser = value;
            }
        }
        public GooglePlaySettings GParserSetting
        {
            get
            {
                return gparserSetting;
            }
            set
            {
                gparserSetting = value;
                GPloader = new GPHtmlLoader(value);
            }
        }
        public DropGameParser DParser
        {
            get
            {
                return dparser;
            }
            set
            {
                dparser = value;
            }
        }
        public DropGameSettings DParserSetting
        {
            get
            {
                return dparserSetting;
            }
            set
            {
                dparserSetting = value;
                Dloader = new DHtmlLoader(value);
            }
        }
        public GoogleParser GoogleParser
        {
            get
            {
                return googleparser;
            }
            set
            {
                googleparser = value;
            }
        }
        public GoogleSettings GoogleParserSetting
        {
            get
            {
                return googleparserSetting;
            }
            set
            {
                googleparserSetting = value;
            }
        }
        public FPDAParser FParser
        {
            get
            {
                return fpdaparser;
            }
            set
            {
                fpdaparser = value;
            }
        }
        public FPDASettings FParserSetting
        {
            get
            {
                return fpdaparserSetting;
            }
            set
            {
                fpdaparserSetting = value;
                Floader = new FHtmlLoader(value);
            }
        }
        public ApkParser ApkParser
        {
            get
            {
                return apkParser;
            }
            set
            {
                apkParser = value;
            }
        }
        public ApkSetting ApkSetting
        {
            get
            {
                return apkSetting;
            }
            set
            {
                apkSetting = value;
                ApkLoader = new ApkHtmlLoader(value);
            }
        }
        #endregion

        public ParserWorker(GooglePlayParser gparser, DropGameParser dparser, FPDAParser fpdaparser)
        {
            this.gparser = gparser;
            this.dparser = dparser;
            this.fpdaparser = fpdaparser;
        }
        public async void Start(TelegramBotClient client, long chatId)
        {
             await Worker(client,chatId);
        }
        private async Task Worker(TelegramBotClient client, long chatId)
        {
            bool forloop = true;
            string GoogleVersion = string.Empty, FpdaVersion = null;
            for (int i = 1; forloop; i++)
            {
                var source = await Dloader.GetSourceById(i);
                if (source == default) forloop = false;
                var domParser = new HtmlParser();

                var document = await domParser.ParseDocumentAsync(source);
                var urls = dparser.GetAppUrls(document);

                foreach (var url in urls)
                {
                    string DropGameVersion = null, DropGameName = null, ApkVersion = "0";
                    try
                    {
                        source = await GPloader.GetSourceBylink(url);
                        document = await domParser.ParseDocumentAsync(source);
                        DropGameName = dparser.GetName(document);
                        DropGameVersion = dparser.GetVersion(document);
                        if (MissGames.Contains(DropGameName)) continue;
                        if (Games.Contains(DropGameName))
                        {
                            await client.SendTextMessageAsync(chatId, $"Солнце, эту игру нужно посмотреть самостоятельно\n{url}");
                            continue;
                        }
                        //string engname = await GoogleParser.GetEngNameAsync(DropGameName);
                        string engname = DropGameName;
                        SearchResult result = await GoogleParser.GetResultAsync(engname);
                        source = await GoogleParser.GetResult(engname);
                        document = await domParser.ParseDocumentAsync(source);
                        string urlstr = googleparser.GetAppUrl(document,engname);
                        try
                        {
                            string appUrl = await GetUrlApp(DropGameName);
                            GoogleVersion = await GetVersionAsync(appUrl);
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("NullReferenceException");
                        }

                        if (result != null && result.Title.StartsWith(engname))
                        {
                            source = await GPloader.GetSourceBylink(result.Link.ToString());
                            document = await domParser.ParseDocumentAsync(source);
                            string version = fpdaparser.GetVersion(document);
                            if (version is null) break;
                            FpdaVersion = version;
                        }
                        source = await ApkLoader.GetSourceByName(DropGameName);
                        document = await domParser.ParseDocumentAsync(source);
                        string apkurl = apkParser.GetUrl(document, DropGameName);
                        if (apkurl is null) { Console.WriteLine("url is null"); }
                        else
                        {
                            source = await GPloader.GetSourceBylink("https://apkcombo.com" + apkurl);
                            document = await domParser.ParseDocumentAsync(source);
                            ApkVersion = apkParser.GetVersion(document);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message );
                        Console.WriteLine("With name " + DropGameName);
                    }

                    string str = string.Empty;
                    DropGameVersion = ChangeVersion(DropGameVersion);
                    GoogleVersion = ChangeVersion(GoogleVersion);
                    ApkVersion = ChangeVersion(ApkVersion);
                    Console.WriteLine(DropGameName);
                    Console.WriteLine($"{DropGameVersion} {GoogleVersion} {ApkVersion} {FpdaVersion}");
                    string Version;
                    if (FpdaVersion == null)
                    {
                        Version = CompareVersion(ApkVersion, GoogleVersion);
                    }
                    else
                    {
                        FpdaVersion = ChangeVersion(FpdaVersion);
                        Version = CompareVersion(FpdaVersion, GoogleVersion);
                        Version = CompareVersion(Version, ApkVersion);
                    }

                    if (DropGameVersion == Version)
                    {
                        Console.WriteLine(Version);
                        Console.WriteLine(new string('-', 30));
                    }
                    else
                    {
                        str = $"{DropGameName} {DropGameVersion} -- {Version}\n{url}";
                    }
                    if (str != string.Empty)
                    {
                        Console.WriteLine(Version);
                        Console.WriteLine(new string('-',30));
                        await Send_NewData(client, chatId, str);
                    }
                }
            }
            await Parser_OnCompleted(client, chatId);
        }
        private async Task Parser_OnCompleted(TelegramBotClient client, long chatId)
        {
            await client.SendTextMessageAsync(chatId, "It is all for now!)\nHave a nice time working on site!*");
        }
        private async Task Send_NewData(TelegramBotClient client, long chatId, string s)
        {
            if (!strs.Contains(s))
            {
                strs.Add(s);
                await client.SendTextMessageAsync(chatId, s);
            }
        }
        private async Task<string> GetUrlApp(string appname)
        {
            var source = await GPloader.GetSourceByName(appname);
            var domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            return gparser.GetAppString(document, appname);
        }
        private async Task<string> GetVersionAsync(string appstr)
        {
            var source = await GPloader.GetSourceBylink(appstr);
            var domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            return gparser.GetVersion(document);
        }
        private static string ChangeVersion(string version)
        {
            version.Replace('a','а').Replace('а', 'a').Replace(',', '.');
            return version;
        }
        private string CompareVersion(string version1,string version2)
        {
            if (version1 == "Varieswithdevice")
                return version2;
            if (version2 == "Varieswithdevice")
                return version1;

            for (int i = 0; i < Math.Min(version1.Length,version2.Length); i++)
            {
                if (version1[i] < version2[i])
                    return version2;
            }
            return version1;
        }
    }
}
