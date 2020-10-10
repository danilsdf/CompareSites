using BackParse.Core;
using BackParse.Core.DropGame;
using BackParse.Core.Google;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BackParse.TelegramBot.Commands
{
    class GamesCommand : Command
    {
        public override string Name => @"/games";

        public override void Execute(Message message, TelegramBotClient client)
        {
            ParserWorker parser = new ParserWorker(new GoogleParser(), new DropGameParser());

            parser.GParserSetting = new GoogleSettings();
            parser.DParserSetting = new DropGameSettings("games");
            parser.Start(client, message.Chat.Id);
        }
    }
}
