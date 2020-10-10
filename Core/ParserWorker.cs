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
            for (int i = 1; i <= dparserSetting.MaxPage; i++)
            {
                var source = await Dloader.GetSourceById(i);
                var domParser = new HtmlParser();

                var document = await domParser.ParseDocumentAsync(source);
                var urls = dparser.GetAppUrls(document);

                foreach (var url in urls)
                {
                    source = await Gloader.GetSourceBylink(url);
                    document = await domParser.ParseDocumentAsync(source);
                    //var version = dparser.GetVersion(document);
                    var result = dparser.GetVersion(document);
                    
                    OnNewData?.Invoke(this, result);
                    Tuple<string, string> Data_Version = new Tuple<string, string>(result.Item1, "Did not find");
                    try
                    {
                        string appUrl = await GetUrlApp(result.Item1);
                        Data_Version = await GetDate_Version(appUrl);
                    }
                    catch (NullReferenceException)
                    {
                       
                    }

                    OnNewData?.Invoke(this, Data_Version);
                    Console.WriteLine(new string('-',30));
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
