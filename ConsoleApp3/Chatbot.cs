using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace ConsoleApp3
{
    internal class Chatbot
    {
        public static string GetUserName()
        {
            Console.Write("\nEnter your name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "User";
            }

            return name;
        }

        public static void GreetUser(string name)
        {
            Console.WriteLine($"\nWelcome, {name}! ");
            Console.WriteLine("I'm your Cybersecurity Awareness Bot.");
           
        }

        public static void StartChat(string name)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n>> ");
                Console.ResetColor();

                string input = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    UI.TypeText("I didn’t quite understand that. Could you rephrase?\n");
                    continue;
                }

                if (input.Contains("exit"))
                {
                    UI.TypeText($"\nGoodbye {name}, stay safe online! \n");
                    break;
                }

                Respond(input);
            }
        }

        private static void Respond(string input)
        {
            // General
            if (input.Contains("how are you"))
            {
                UI.TypeText("I'm fully operational and ready to protect you from cyber threats! 🤖\n");
            }
            else if (input.Contains("your purpose"))
            {
                UI.TypeText("My purpose is to educate users about cybersecurity and safe online practices.\n");
            }

            // Passwords
            else if (input.Contains("password"))
            {
                UI.TypeText("Use strong passwords with at least 12 characters, including symbols and numbers.\n");
                UI.TypeText("Avoid using names or birthdays.\n");
            }

            // Phishing
            else if (input.Contains("phishing"))
            {
                UI.TypeText("Phishing is when attackers trick you into revealing personal info.\n");
                UI.TypeText("Never click suspicious links or emails.\n");
            }

            // Safe browsing
            else if (input.Contains("browsing") || input.Contains("internet safety"))
            {
                UI.TypeText("Always check for HTTPS and avoid downloading unknown files.\n");
            }

            // Malware
            else if (input.Contains("malware") || input.Contains("virus"))
            {
                UI.TypeText("Malware is harmful software.\nUse antivirus software and keep your system updated.\n");
            }

            // Scams
            else if (input.Contains("scam"))
            {
                UI.TypeText("Be cautious of offers that seem too good to be true.\n");
            }

            // Default
            else
            {
                UI.TypeText("I'm not sure about that. Try asking about cybersecurity topics.\n");
            }
        }
    }
}
}
