using BackParse.Core;
using BackParse.Core.DropGame;
using BackParse.Core.Google;
using System;

namespace BackParse
{
    class Program
    {
        static ParserWorker parser;

        static void Main(string[] args)
        {
            parser = new ParserWorker(new GoogleParser(), new DropGameParser() );
            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNewData += Parser_OnNewData;

            while (true)
            {
                //Console.Write("Write an app name: ");
               // string appname = Console.ReadLine();
                parser.GParserSetting = new GoogleSettings("Mario Kart Tour");
                parser.DParserSetting = new DropGameSettings("games",4);
                parser.Start();

                Console.ReadKey();
            }
        }

        private static void Parser_OnNewData(object arg1, Tuple<string, string> arg2)
        {
            //foreach (var item in arg2)
            //{
                Console.WriteLine($"{arg2.Item1} {arg2.Item2}");
            //}
        }

        private static void Parser_OnCompleted(object obj)
        {
            Console.WriteLine("All works done");
        }
    }
}
