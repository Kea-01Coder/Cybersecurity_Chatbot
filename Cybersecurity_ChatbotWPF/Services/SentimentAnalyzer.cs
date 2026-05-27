using System;
using System.Collections.Generic;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Services
{
    public class SentimentAnalyzer
    {
        private Dictionary<string, string> sentimentResponses;

        public SentimentAnalyzer()
        {
            sentimentResponses = new Dictionary<string, string>
            {
                { "worried", "It's completely understandable to feel that way. Cybersecurity can feel overwhelming at first. Let me give you a simple, actionable tip to help you feel more secure." },
                { "curious", "That's great! Curiosity is the first step to becoming cybersecurity aware! Let me share something useful that will help you stay safe online." },
                { "frustrated", "I understand this can be frustrating. Technology can be complicated. Let's take it step by step. Here's something straightforward that might help." }
            };
        }

        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") || input.Contains("anxious") || input.Contains("afraid"))
                return "worried";

            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to learn") || input.Contains("tell me more") || input.Contains("fascinated"))
                return "curious";

            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("confusing") || input.Contains("difficult") || input.Contains("hard") || input.Contains("complicated"))
                return "frustrated";

            return null;
        }

        public string GetSentimentResponse(string sentiment)
        {
            if (sentimentResponses.ContainsKey(sentiment))
                return sentimentResponses[sentiment];
            return null;
        }

    }
}
