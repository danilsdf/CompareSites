using AngleSharp.Html.Parser;
using BackParse.Core.DropGame;
using BackParse.Core.Google;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BackParse.Core
{
    class ParserWorker
    {
        #region Filds
        GoogleParser gparser;
        GoogleSettings gparserSetting;

        DropGameParser dparser;
        DropGameSettings dparserSetting;

        GHtmlLoader Gloader;
        DHtmlLoader Dloader;

        #endregion

        #region Properties
        public GoogleParser GParser
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
        public GoogleSettings GParserSetting
        {
            get
            {
                return gparserSetting;
            }
            set
            {
                gparserSetting = value;
                Gloader = new GHtmlLoader(value);
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
        #endregion

        public ParserWorker(GoogleParser gparser, DropGameParser dparser)
        {
            this.gparser = gparser;
            this.dparser = dparser;
        }
        public async void Start(TelegramBotClient client, long chatId)
        {
             await Worker(client,chatId);
        }
        private async Task Worker(TelegramBotClient client, long chatId)
        {
            bool forloop = true;
            for (int i = 1; forloop; i++)
            {
                var source = await Dloader.GetSourceById(i);
                if (source == default) forloop = false;
                var domParser = new HtmlParser();

                var document = await domParser.ParseDocumentAsync(source);
                var urls = dparser.GetAppUrls(document);

                foreach (var url in urls)
                {
                    source = await Gloader.GetSourceBylink(url);
                    document = await domParser.ParseDocumentAsync(source);
                    var version = dparser.GetVersion(document);

                    Tuple<string, string> Data_Version;
                    try
                    {
                        string appUrl = await GetUrlApp(version.Item1);
                        Data_Version = await GetDate_Version(appUrl);
                    }
                    catch (NullReferenceException)
                    {
                        Data_Version = new Tuple<string, string>(version.Item1, "Did not find"); 
                    }
                    string str = string.Empty;
                    if (Data_Version.Item2 == "Varieswithdevice" || Data_Version.Item2 == "Did not find") {
                        str = version.Item1 + $" must be checked one more time\n{url}";
                        await Send_NewData(client, chatId, str);
                    }
                    else if (version.Item2 == Data_Version.Item2) {
                    }
                    else { 
                        str = $"{version.Item1} {version.Item2} -- {Data_Version.Item2}\n{url}";
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
            await client.SendTextMessageAsync(chatId, s);
        }
        private async Task<string> GetUrlApp(string name)
        {
            var source = await Gloader.GetSourceByName(name);
            var domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            return gparser.GetAppString(document);
        }
        private async Task<Tuple<string,string>> GetDate_Version(string appstr)
        {
            var source = await Gloader.GetSourceBylink(appstr);
            var domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            return gparser.GetDataVersion(document);
        }
    }
}
