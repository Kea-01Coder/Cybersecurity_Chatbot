using System;
using System.Collections.Generic;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Services
{
        public class KeywordRecognizer
        {
            private Dictionary<string, string> keywordResponses;

            public KeywordRecognizer()
            {
                keywordResponses = new Dictionary<string, string>
            {
                { "password", "Make sure to use strong, unique passwords for each account. Avoid using personal details like your birthday or name. Consider using a password manager to generate and store complex passwords!" },
                { "scam", "Scammers often create fake urgency to make you act without thinking. Never share personal information or send money to someone you haven't met in person. Report scams to local authorities immediately." },
                { "privacy", "Protect your privacy by reviewing app permissions regularly, using encrypted messaging apps like Signal or WhatsApp, and limiting what you share on social media. Remember: once online, it's hard to remove!" }
            };
            }

            public string GetKeywordResponse(string input)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (input.Contains(keyword))
                    {
                        return keywordResponses[keyword];
                    }
                }
                return null;
            }

            public string GetKeywordTopic(string input)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (input.Contains(keyword))
                    {
                        return keyword;
                    }
                }
                return null;
            }

            public List<string> GetAllKeywords()
            {
                return new List<string>(keywordResponses.Keys);
            }
        }
}

