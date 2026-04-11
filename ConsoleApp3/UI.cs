using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class UI
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

            public static string GetUserName()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter your name: ");
                    string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    return "User";

                return name;
            }

            public static void GreetUser(string name)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                TypeText($"\nWelcome, {name}! \n");
                TypeText("I am your Cybersecurity Awareness Bot.\n");
                TypeText("Ask me about : \n" +
                    "1. Passwords \n 2. Phishing \n 3. Scams \n 4. Safe browsing.\n 5. Malware");
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

