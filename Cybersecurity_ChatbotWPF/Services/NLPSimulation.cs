using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cybersecurity_ChatbotWPF.Services
{
    public class NLPSimulator
    {
        private Dictionary<string, string> intentPatterns;

        public NLPSimulator()
        {
            InitializeIntentPatterns();
        }

        private void InitializeIntentPatterns()
        {
            intentPatterns = new Dictionary<string, string>
            {
                // Task Intents
                { "add_task", @"(add|create|new).*(task|todo|item)" },
                { "delete_task", @"(delete|remove|clear).*(task|todo|item)" },
                { "complete_task", @"(complete|finish|done).*(task|todo|item)" },
                { "list_tasks", @"(show|list|view|display).*(task|todo|item|list)" },

                // Reminder Intents
                { "set_reminder", @"(remind|set|create).*(reminder|remind)" },
                { "view_reminders", @"(show|view|display).*(reminder|remind|alert)" },

                // Quiz Intents
                { "start_quiz", @"(start|begin|play).*(quiz|game|test)" },
                { "answer_quiz", @"^(?<answer>\d+)$" }, // Number answers

                // Log Intents
                { "show_log", @"(show|view|display).*(log|history|activity)" },
                { "show_activity", @"(activity|what.*done|what.*do)" },

                // Help Intent
                { "help", @"(help|support|assist|what can you do|how to use)" },

                // Cybersecurity Topics
                { "password", @"(password|passcode|2fa|two-factor|authentication)" },
                { "phishing", @"(phish|phishing|scam|fraud|deception)" },
                { "privacy", @"(privacy|private|permission|share|data)" },
                { "malware", @"(malware|virus|ransomware|trojan|worm|spyware)" },
                { "browsing", @"(browse|browsing|internet|online|web|site|url)" },
                { "social_engineering", @"(social.*engineer|manipulate|trick|deceive)" }
            };
        }

        public string DetectIntent(string input)
        {
            input = input.ToLower().Trim();

            foreach (var pattern in intentPatterns)
            {
                if (Regex.IsMatch(input, pattern.Value, RegexOptions.IgnoreCase))
                {
                    return pattern.Key;
                }
            }

            return "unknown";
        }

        public string ExtractTaskInfo(string input)
        {
            // Extract task title
            var taskMatch = Regex.Match(input, @"(?:add|create|new)\s+(?:task\s+)?['""]?(.+?)['""]?(?:\s+(?:with|and)\s+reminder)?$", RegexOptions.IgnoreCase);
            if (taskMatch.Success)
                return taskMatch.Groups[1].Value.Trim();

            // Try alternative pattern
            var altMatch = Regex.Match(input, @"(?:task|todo|item)\s+['""]?(.+?)['""]?(?:\s+reminder)?$", RegexOptions.IgnoreCase);
            if (altMatch.Success)
                return altMatch.Groups[1].Value.Trim();

            return "";
        }

        public string ExtractReminderDays(string input)
        {
            var match = Regex.Match(input, @"(\d+)\s*(?:day|d|days|day's)", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value;

            // Check for "week" or "weeks"
            var weekMatch = Regex.Match(input, @"(\d+)?\s*(?:week|weeks)", RegexOptions.IgnoreCase);
            if (weekMatch.Success)
            {
                int weeks = string.IsNullOrEmpty(weekMatch.Groups[1].Value) ? 1 : int.Parse(weekMatch.Groups[1].Value);
                return (weeks * 7).ToString();
            }

            // Check for "month" or "months"
            var monthMatch = Regex.Match(input, @"(\d+)?\s*(?:month|months)", RegexOptions.IgnoreCase);
            if (monthMatch.Success)
            {
                int months = string.IsNullOrEmpty(monthMatch.Groups[1].Value) ? 1 : int.Parse(monthMatch.Groups[1].Value);
                return (months * 30).ToString();
            }

            return "";
        }

        public string ExtractTopic(string input)
        {
            string[] topics = { "password", "phishing", "scam", "privacy", "malware", "browsing", "2fa", "authentication", "social engineering" };
            foreach (var topic in topics)
            {
                if (input.Contains(topic))
                    return topic;
            }
            return "";
        }

        public bool IsQuestion(string input)
        {
            return input.EndsWith("?") ||
                   input.StartsWith("what") ||
                   input.StartsWith("how") ||
                   input.StartsWith("why") ||
                   input.StartsWith("when") ||
                   input.StartsWith("where") ||
                   input.StartsWith("who") ||
                   input.StartsWith("can") ||
                   input.StartsWith("do") ||
                   input.StartsWith("does") ||
                   input.StartsWith("is") ||
                   input.StartsWith("are");
        }

        public string ExtractQuizAnswer(string input)
        {
            var match = Regex.Match(input, @"^(\d+)$");
            if (match.Success)
                return match.Groups[1].Value;
            return "";
        }
    }
}
