using System;
using System.Collections.Generic;
using System.Text;

namespace BackParse.Core.DropGame
{
    class DropGameSettings
    {
        public string BaseUrl { get; set; } = @"https://dropgame.ru/android";
        public string TypeApp { get; set; }
        public string Prefix { get; set; } = @"page/Number";
        public int MaxPage { get; set; }
        public DropGameSettings(string typeapp,int max)
        {
            TypeApp = typeapp;
            MaxPage = max;
        }
    }
}
