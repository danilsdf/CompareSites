namespace BackParse.Core.Google
{
    class GoogleSettings
    {
        public string BaseUrl { get; set; } = @"https://play.google.com";
        public string Prefix { get; set; } = @"store/search?q=AppName&c=apps";
        public string AppName { get; set; }
        public GoogleSettings(string appName)
        {
            AppName = appName;
        }
    }
}
