using BackParse.Core;
using BackParse.Core.APK;
using BackParse.Core.DropGame;
using BackParse.Core.FPDA;
using BackParse.Core.Google;
using BackParse.Core.GooglePlay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BackParse.TelegramBot.Commands
{
    class ProgramCommand : Command
    {
        public override string Name => @"/programs";

        public override void Execute(Message message, TelegramBotClient client)
        {

            ParserWorker parser = new ParserWorker(new GooglePlayParser(), new DropGameParser(), new FPDAParser())
            {
                GoogleParser = new GoogleParser(new GoogleSettings()),
                GParserSetting = new GooglePlaySettings(),
                FParserSetting = new FPDASettings(),
                ApkSetting = new ApkSetting(),
                DParserSetting = new DropGameSettings("program"),
                ApkParser = new ApkParser(new ApkSetting())
            };

            parser.Start(client, message.Chat.Id);
        }
    }
}
