using AngleSharp.Html.Parser;
using BackParse.Core.DropGame;
using BackParse.Core.Google;
using System;
using System.Threading.Tasks;

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

        bool isActive;
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
        public bool IsActive => isActive;
        #endregion

        public event Action<object, Tuple<string, string>> OnNewData;
        public event Action<object> OnCompleted;
        public ParserWorker(GoogleParser gparser, DropGameParser dparser)
        {
            this.gparser = gparser;
            this.dparser = dparser;
        }
        public void Start()
        {
            isActive = true;
            Worker();
        }
        public void Abort()
        {
            isActive = false;
        }
        private async void Worker()
        {
            if (!isActive)
            {
                OnCompleted?.Invoke(this);
                return;
            }
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
                        Console.WriteLine(new string('-', 30));
                    }
                    if (Data_Version.Item2 == "Varieswithdevice" || Data_Version.Item2 == "Did not find") {
                        Console.WriteLine(version.Item1 + " must be checked one more time");
                    Console.WriteLine(new string('-',30));
                    }
                    else if (version.Item2 == Data_Version.Item2) { 
                        //Console.WriteLine(version.Item1 + " is current version");
                    }
                    else
                    {
                        OnNewData?.Invoke(this, new Tuple<string, string>(version.Item1,$"{version.Item2} -- {Data_Version.Item2}"));
                    Console.WriteLine(new string('-',30));
                    }
                }
            }
            
            OnCompleted?.Invoke(this);
            isActive = false;
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
