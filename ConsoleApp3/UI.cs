using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace ConsoleApp3
{
    internal class UI
    {
    
    
        public static void PlayWelcome()
        {
            SoundPlayer player = new SoundPlayer("welcome.wav");
            player.PlaySync(); // waits until done
        }

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

                                         CYBERSECURITY AWARENESS BOT 
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
                ===========================================
                         Cybersecurity Awareness Bot
                ===========================================
                                
                              ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            TypeText($"\nWelcome, {name}! \n");
            TypeText("I am your Cybersecurity Awareness Bot.\n");
            Console.WriteLine();

            Menu();

        }

        public static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            String Menu = " 1. Passwords \n" +
            " 2. Phishing \n" +
            " 3. Scams \n" +
            " 4. Safe browsing.\n" +
            " 5. Malware\n" +
            "Type 'exit' to quit.\n";
            Console.WriteLine(Menu);
        }

        public static void TypeText(string message, bool speak = true)
        {
            if (speak)
            {
                new Thread(() => Speech.SpeakAndWait(message)).Start();
            }

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(15);
            }
        }
    }
}
