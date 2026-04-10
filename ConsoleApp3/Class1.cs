using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Class1
    {
        public static class UI
        {
            public static void ShowBanner()
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(@"
   ██████╗██╗   ██╗██████╗ ███████╗██████╗ 
  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝

        🔐 CYBERSECURITY AWARENESS BOT 🔐
        ");

                Console.ResetColor();
            }

            public static void Divider()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n==================================================\n");
                Console.ResetColor();
            }

            public static string GetUserName()
            {
                Console.Write("Enter your name: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    return "User";

                return name;
            }

            public static void GreetUser(string name)
            {
                TypeText($"\nWelcome, {name}! 👋\n");
                TypeText("I am your Cybersecurity Awareness Bot.\n");
                TypeText("Ask me about passwords, phishing, scams, or safe browsing.\n");
                TypeText("Type 'exit' to quit.\n");
            }

            public static void TypeText(string message)
            {
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(15);
                }
            }
        }
    }
}
