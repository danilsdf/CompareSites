namespace BackParse.Core.GooglePlay
{
    class GooglePlaySettings
    {
        public string BaseUrl { get; set; } = @"https://play.google.com";
        public string Prefix { get; set; } = @"store/search?q=AppName&c=apps";
    }
}
