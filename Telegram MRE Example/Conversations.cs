using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Telegram_MRE_Example
{
    class Conversations
    {
        public static async Task StarGetUserInformationsAsync(global::Telegram.Bot.Types.Message msg)
        {
            var step = 1;
            var userInfos = new List<string>();

            var mre = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> mHandler = (sender, e) =>
            {
                if (msg.From.Id != e.Message.From.Id) return;
                if (msg.Chat.Id != e.Message.Chat.Id) return;

                if (step == 1)
                {
                    userInfos.Add(e.Message.Text);
                    step++;
                    TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"Nice! How old are you, {e.Message.Text}?");
                }
                else if (step == 2)
                {
                    if (e.Message.Text.IsInt())
                    {
                        userInfos.Add(e.Message.Text);
                        step++;
                        TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, "Where do you live?");
                    }
                    else
                    {
                        TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, "Only numbers are allowed!");
                        return;
                    }
                }
                else if (step == 3)
                {
                    userInfos.Add(e.Message.Text);
                    step++;
                    TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, "Is this example helpful?");
                }

                else if (step == 4)
                {
                    userInfos.Add(e.Message.Text);
                    TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"Okay! Your entered informations:\n\nYour Name: *{userInfos.ElementAt(0)}*\nYour Age: *{userInfos.ElementAt(1)}*\nYou live in: *{userInfos.ElementAt(2)}*\nHelpful example? *{userInfos.ElementAt(3)}*", ParseMode.Markdown);
                }
            };

            await TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"Hello!\nLet me ask you a few questions\n\nWhat is your name?", ParseMode.Markdown);

            TelegramBot.TeleBot.OnMessage += mHandler;
            mre.WaitOne();
            TelegramBot.TeleBot.OnMessage -= mHandler;
        }

        public static async Task StartRandomConversationAsync(global::Telegram.Bot.Types.Message msg)
        {
            var options = new List<string>();

            var mre = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> mHandler = (sender, e) =>
            {
                if (msg.From.Id != e.Message.From.Id) return;
                if (msg.Chat.Id != e.Message.Chat.Id) return;

                if (e.Message.Text == "/done")
                {
                    TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"I choose...", ParseMode.Markdown).Wait();
                    Thread.Sleep(3000);
                    TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"*{options.GetRandom()}*!", ParseMode.Markdown).Wait();
                    mre.Set();
                }
                else
                {
                    options.Add(e.Message.Text);
                }
            };

            await TelegramBot.TeleBot.SendTextMessageAsync(msg.Chat.Id, $"Okay *{msg.From.FirstName}*, now send some options and i will choose a random one.\n\nType /done if you are done.", ParseMode.Markdown);

            TelegramBot.TeleBot.OnMessage += mHandler;
            mre.WaitOne();
            TelegramBot.TeleBot.OnMessage -= mHandler;
            TelegramBot.UserIdsAsked.Remove(msg.From.Id);

        }

    }

    public static class ListStringExtensions
    {
        public static string GetRandom(this List<string> list)
        {
            var max = list.Count;
            var rnd = new Random();
            var rndNmb = rnd.Next(0, max);
            return list[rndNmb];
        }

        public static bool IsInt(this string s)
        {
            var x = 0;
            return int.TryParse(s, out x);
        }

    }
}
