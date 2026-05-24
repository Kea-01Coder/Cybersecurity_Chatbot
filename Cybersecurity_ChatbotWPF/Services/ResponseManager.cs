using System;
using System.Collections.Generic;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Services
{
    internal class ResponseManager
    {
        public class ResponseManager
        {
            private List<string> phishingTips;
            private List<string> generalTips;
            private Random random;

            public ResponseManager()
            {
                random = new Random();

                phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like your bank or PayPal.",
                "Always check the sender's email address carefully. One letter difference (like 'arnazon.com' instead of 'amazon.com') can mean a fake.",
                "Never click on links in suspicious emails. Hover over them first to see the real URL - it often reveals the scam.",
                "If an email creates urgency ('Your account will be closed in 24 hours!'), it's almost certainly a phishing attempt.",
                "Legitimate companies will NEVER ask for your password, credit card number, or 2FA codes via email.",
                "Look for spelling and grammar mistakes - these are common in phishing emails from non-native speakers.",
                "Don't download attachments from unknown senders. They often contain malware or ransomware.",
                "When in doubt, go directly to the company's website by typing the URL yourself, not clicking the email link."
            };

                generalTips = new List<string>
            {
                "Enable two-factor authentication (2FA) on all your important accounts - it adds an extra layer of security.",
                "Keep your software, operating system, and apps updated regularly. Updates often include security patches.",
                "Use a VPN when connecting to public Wi-Fi networks like coffee shops or airports.",
                "Back up your important files to an external drive or cloud storage. Ransomware can't hold you hostage if you have backups.",
                "Be careful what you share on social media - oversharing helps scammers answer your security questions.",
                "Use a different password for every account. If one gets hacked, the rest stay safe.",
                "Check your bank and credit card statements regularly for unauthorized transactions.",
                "Be skeptical of 'too good to be true' offers - lottery wins, free vacations, or miracle cures are almost always scams."
            };
            }

            public string GetRandomPhishingTip()
            {
                return phishingTips[random.Next(phishingTips.Count)];
            }

            public string GetRandomGeneralTip()
            {
                return generalTips[random.Next(generalTips.Count)];
            }

            public string GetFollowUpResponse(string lastTopic)
            {
                if (string.IsNullOrEmpty(lastTopic))
                {
                    return "Sure! What topic would you like more tips on? Try 'password', 'scam', 'phishing', or 'privacy'.";
                }

                if (lastTopic.Contains("phishing"))
                {
                    return $"Another phishing tip: {GetRandomPhishingTip()}";
                }

                if (lastTopic.Contains("password"))
                {
                    return $"Here's another password tip: {GetRandomGeneralTip()}";
                }

                if (lastTopic.Contains("scam"))
                {
                    return $"Another scam awareness tip: {GetRandomGeneralTip()}";
                }

                if (lastTopic.Contains("privacy"))
                {
                    return $"Another privacy tip: {GetRandomGeneralTip()}";
                }

                return $"Here's another cybersecurity tip: {GetRandomGeneralTip()}";
            }
        }

    }
}
