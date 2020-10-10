using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BackParse.TelegramBot.Commands
{
    class StartCommand : Command
    {
        public override string Name => @"/start";

        public async override void Execute(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(message.Chat.Id, $"Commands:\n" +
                $"/games\n" +
                $"/programs");
        }
    }
}
