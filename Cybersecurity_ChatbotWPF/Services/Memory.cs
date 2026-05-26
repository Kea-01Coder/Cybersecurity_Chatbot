using System;
using System.Collections.Generic;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Services
{
        public class MemoryManager
        {
            private string userName;
            private string favoriteTopic;
            private Dictionary<string, string> userPreferences;
            private List<string> conversationHistory;
            private int messageCount;

            public MemoryManager()
            {
                userPreferences = new Dictionary<string, string>();
                conversationHistory = new List<string>();
                messageCount = 0;
            }

            public void SetUserName(string name)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    userName = name.Trim();
                    userPreferences["name"] = userName;
                }
            }

            public string GetUserName()
            {
                return userName ?? "friend";
            }

            public bool HasUserName()
            {
                return !string.IsNullOrEmpty(userName);
            }

            public void RememberFavoriteTopic(string topic)
            {
                if (string.IsNullOrEmpty(favoriteTopic))
                {
                    favoriteTopic = topic;
                    userPreferences["favorite_topic"] = topic;
                }
            }

            public string GetFavoriteTopic()
            {
                return favoriteTopic;
            }

            public bool HasFavoriteTopic()
            {
                return !string.IsNullOrEmpty(favoriteTopic);
            }

            public void AddToHistory(string userMessage, string botResponse)
            {
                conversationHistory.Add($"User: {userMessage}");
                conversationHistory.Add($"Bot: {botResponse}");
                messageCount += 2;

                // Keep only last 100 messages to save memory
                if (conversationHistory.Count > 100)
                {
                    conversationHistory.RemoveRange(0, 20);
                }
            }

            public List<string> GetConversationHistory()
            {
                return new List<string>(conversationHistory);
            }

            public int GetMessageCount()
            {
                return messageCount;
            }

            public string GetPersonalizedGreeting()
            {
                if (HasUserName() && HasFavoriteTopic())
                {
                    return $"Welcome back, {userName}! Ready to learn more about {favoriteTopic}?";
                }
                else if (HasUserName())
                {
                    return $"Good to see you again, {userName}!";
                }
                return "Hello! Welcome to CyberGuardian AI!";
            }

            public void ClearMemory()
            {
                userName = null;
                favoriteTopic = null;
                userPreferences.Clear();
                conversationHistory.Clear();
                messageCount = 0;
            }
        } 
}

