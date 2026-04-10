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
    }
}
