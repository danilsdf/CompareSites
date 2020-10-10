using BackParse.TelegramBot;
using BackParse.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace BackParse
{
    class Program
    {
        private static TelegramBotClient BotClient;
        public static IReadOnlyList<Command> commands;
        static bool IsWorking = false;
        static void Main()
        {

            BotClient = Bot.Get();
            commands = Bot.Commands;

            BotClient.OnMessage += BotClient_OnMessage;
            BotClient.OnMessageEdited += BotClient_OnMessage;

            BotClient.StartReceiving();
            Console.ReadKey();
        }
        private async static void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Chat.Id);
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;
            //return;
            if (e.Message.Chat.Id != 381714929 && e.Message.Chat.Id != 386219611)
            {
                await BotClient.SendTextMessageAsync(e.Message.Chat.Id, $"Stop writing to this bot\n" +
                    $"This bot was created for comfortable work with a site");return;
            }
            //if (IsWorking)
            //{
            //    await BotClient.SendTextMessageAsync(e.Message.Chat.Id, $"I am working!\n Please, stop spaming"); return;
            //}
            var message = e.Message;

            foreach (var command in commands)
            {
                if (command.Contains(message.Text))
                {
                    //IsWorking = true;
                    command.Execute(message, BotClient);
                    //IsWorking = false;
                    break;
                }
            }
        }
    }
}
