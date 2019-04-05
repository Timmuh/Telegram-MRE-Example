﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_MRE_Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
                try
                {
                    var bot = new Bot();
                    bot.Run();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
        }
    }
}