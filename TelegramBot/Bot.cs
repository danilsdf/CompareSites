using BackParse.TelegramBot.Commands;
using System.Collections.Generic;
using Telegram.Bot;

namespace BackParse.TelegramBot
{
    public static class Bot
    {
        public static TelegramBotClient client;
        public static string token = "YourToken";
        public static string Name = "kristinahelper_bot";

        private static List<Command> commandslist;
        public static IReadOnlyList<Command> Commands { get => commandslist.AsReadOnly(); }

        public static TelegramBotClient Get()
        {
            commandslist = new List<Command>();
            commandslist.Add(new StartCommand());
            commandslist.Add(new GamesCommand());
            commandslist.Add(new ProgramCommand());


            client = new TelegramBotClient(token);
            return client;
        }

    }
}
