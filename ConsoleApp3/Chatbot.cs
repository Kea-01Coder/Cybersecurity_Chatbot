using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

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
            // PASSWORD
            if (input.Contains("password"))
            {
               TopicPassword();
            }

            // PHISHING
            else if (input.Contains("phishing"))
            {
                    TopicPhishing();
            }

            // MALWARE
            else if (input.Contains("malware") || input.Contains("virus"))
            {
                    TopicMalware();
            }

            else if (input.Contains("Scams"))
            {
               TopicScams();
            }

            else if (input.Contains("Safe Browsing"))
            {
                SafeBrowsing();
            }

            // DEFAULT
            else
            {
                UI.TypeText("I'm not sure about that. Try cybersecurity topics.\n");
            }
        }

       public static void TopicPhishing()
       {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                                    
                    ==============================================
                                   PHISHING AWARENESS             
                    ==============================================
                                    
                                    ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Definition");
                Console.WriteLine("2. Examples");
                Console.WriteLine("3. Prevention Tips");
                Console.WriteLine("4. Back to Main Menu");

                String choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n Phishing is a cyber attack that uses disguised email as a weapon. The goal is to trick the email recipient into believing that the message is something they want or need — a request from their bank, for instance, or a note from someone in their company — and to click a link or download an attachment.\n");
                        break;
                    case "2":
                        Console.WriteLine("\n Examples of phishing include: \n1) An email that appears to be from a reputable company asking you to verify your account information. \n 2) A message that looks like it's from a coworker asking for sensitive information. \n 3) A fake website that mimics a legitimate one to steal your login credentials.\n");
                        break;
                    case "3":
                        Console.WriteLine("\n Prevention tips for phishing include: \n 1) Be cautious of unsolicited emails, especially those that ask for personal information. \n 2) Check the sender's email address carefully. \n 3) Hover over links to see the actual URL before clicking. \n 4) Use two-factor authentication whenever possible.\n");
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                        break;
                }
            }
        }

        public static void TopicPassword()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                                        
                        ==============================================
                                    PASSWORD SECURITY             
                        ==============================================
                                        
                                        ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Importance of Strong Passwords");
                Console.WriteLine("2. How to Create a Strong Password");
                Console.WriteLine("3. Password Management Tips");
                Console.WriteLine("4. Back to Main Menu");
                
                String choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Strong passwords are crucial for protecting your online accounts from unauthorized access. Weak passwords can be easily guessed or cracked, putting your personal information at risk.");
                        break;
                    case "2":
                        Console.WriteLine("To create a strong password, use a combination of uppercase and lowercase letters, numbers, and special characters. Avoid using common words, phrases, or easily guessable information like birthdays.");
                        break;
                    case "3":
                        Console.WriteLine("Password management tips include: 1) Use a password manager to generate and store complex passwords. \n 2) Never reuse passwords across multiple sites. \n 3) Change your passwords regularly, especially if you suspect a breach.");
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                        break;
                }
            }
        }

        public static void TopicMalware()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                                    
                    ==============================================
                                    MALWARE AWARENESS             
                    ==============================================
                                    
                                    ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. What is Malware?");
                Console.WriteLine("2. Common Types of Malware");
                Console.WriteLine("3. How to Protect Against Malware");
                Console.WriteLine("4. Back to Main Menu");
                String choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Malware, short for malicious software, is any software intentionally designed to cause damage to a computer, server, client, or computer network. It can take the form of viruses, worms, trojans, ransomware, spyware, adware, and more.");
                        break;
                    case "2":
                        Console.WriteLine("Common types of malware include: 1) Viruses that attach themselves to clean files and spread throughout a system. \n 2) Ransomware that locks your files and demands payment for their release. \n 3) Spyware that secretly gathers user information without consent.");
                        break;
                    case "3":
                        Console.WriteLine("To protect against malware: 1) Keep your operating system and software up to date. \n 2) Use reputable antivirus and anti-malware software. \n 3) Be cautious when downloading files or clicking on links from unknown sources.");
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                        break;
                }
            }
        }
        public static void TopicScams()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                                    
                    ==============================================
                                    SCAMS AWARENESS             
                    ==============================================
                                    
                                    ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Common Types of Scams");
                Console.WriteLine("2. How to Recognize a Scam");
                Console.WriteLine("3. What to Do if You Encounter a Scam");
                Console.WriteLine("4. Back to Main Menu");
                String choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Common types of scams include lottery scams, romance scams, and tech support scams.");
                        break;
                    case "2":
                        Console.WriteLine("To recognize a scam, be skeptical of unsolicited offers, verify the legitimacy of requests for money or information, and never share sensitive data with unknown parties.");
                        break;
                    case "3":
                        Console.WriteLine("If you encounter a scam, report it to the appropriate authorities and inform others to prevent them from falling victim.");
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                        break;
                }
            }
        }

        public static void SafeBrowsing()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                                    
                    ==============================================
                                    SAFE BROWSING AWARENESS             
                    ==============================================
                                    
                                    ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. What is Safe Browsing?");
                Console.WriteLine("2. Tips for Safe Browsing");
                Console.WriteLine("3. Common Online Threats");
                Console.WriteLine("4. Back to Main Menu");
                String choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Safe browsing involves practices that help protect your personal information and devices from online threats while surfing the internet.");
                        break;
                    case "2":
                        Console.WriteLine("Tips for safe browsing include: 1) Use secure websites (look for HTTPS). \n 2) Avoid clicking on suspicious links. \n 3) Keep your browser and plugins updated.");
                        break;
                    case "3":
                        Console.WriteLine("Common online threats include phishing sites, malicious downloads, and fake news websites.");
                        break;
                    case "4":
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                        break;
                }
            }
        }   
    }
}

