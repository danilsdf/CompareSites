using BackParse.Core;
using BackParse.Core.DropGame;
using BackParse.Core.Google;
using System;

namespace BackParse
{
    class Program
    {
        static ParserWorker parser;

        static void Main()
        {
            parser = new ParserWorker(new GoogleParser(), new DropGameParser());
            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNewData += Parser_OnNewData;

            Console.WriteLine("Write a number of type:\n1.Games\n2.Programs ");
            ConsoleKey key;
            string type = string.Empty;
            while (type == string.Empty)
            {
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        type = "games";
                        break;
                    case ConsoleKey.D2:
                        type = "program";
                        break;
                    default:
                        Console.Write("Write one more time! ");
                        break;
                }
            }
            Console.WriteLine();
            parser.GParserSetting = new GoogleSettings();
            parser.DParserSetting = new DropGameSettings(type);
            parser.Start();
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
