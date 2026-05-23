using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        // Simple chat message storage
        private ObservableCollection<string> chatMessages = new ObservableCollection<string>();

        // Memory storage
        private string userName = null;
        private string favoriteTopic = null;
        private string lastTopic = null;

        // Random responses
        private string[] phishingTips = new string[]
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Always check the sender's email address carefully. One letter difference can mean a fake.",
            "Never click on links in suspicious emails. Hover over them first to see the real URL.",
            "If an email creates urgency ('act now!'), it's likely a phishing attempt.",
            "Legitimate companies will never ask for your password via email."
        };

        // Keyword responses
        private System.Collections.Generic.Dictionary<string, string> keywordResponses =
            new System.Collections.Generic.Dictionary<string, string>
        {
            { "password", "Make sure to use strong, unique passwords for each account. Avoid using personal details. Consider using a password manager!" },
            { "scam", "Scammers often create fake urgency. Never share personal information or send money to someone you haven't met in person." },
            { "privacy", "Protect your privacy by reviewing app permissions, using encrypted messaging apps, and limiting what you share on social media." }
        };

        // Speech
        private SpeechSynthesizer synthesizer;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            // Setup chat display
            ChatListBox.ItemsSource = chatMessages;

            // Initialize speech
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Volume = 100;
                synthesizer.Rate = 0;
            }
            catch (Exception ex)
            {
                AddMessage("System", $"Speech not available: {ex.Message}");
            }

            ShowWelcomeBanner();
        }

        private void AddMessage(string sender, string message)
        {
            chatMessages.Add($"[{sender}] {message}");
            ChatListBox.ScrollIntoView(chatMessages[chatMessages.Count - 1]);
        }

        private void ShowWelcomeBanner()
        {
            AddMessage("System", "╔══════════════════════════════════════════════════════════════╗");
            AddMessage("System", "║     CYBERSECURITY AWARENESS CHATBOT - PROTECT YOURSELF     ║");
            AddMessage("System", "╚══════════════════════════════════════════════════════════════╝");
            AddMessage("Bot", "Hello! Welcome to CyberGuardian AI. What's your name?");

            if (synthesizer != null)
            {
                synthesizer.SpeakAsync("Hello! Welcome to CyberGuardian AI. What's your name?");
            }
        }

        private void ProcessInput(string input)
        {
            StatusText.Text = "Thinking...";
            StatusText.Foreground = Brushes.Yellow;

            // Handle exit
            if (input.Contains("exit") || input == "quit" || input == "bye")
            {
                AddMessage("Bot", $"Goodbye {userName ?? "friend"}! Stay safe online!");
                synthesizer?.SpeakAsync($"Goodbye {userName ?? "friend"}! Stay safe online!");
                StatusText.Text = "Conversation ended";
                return;
            }

            // Handle name (first interaction)
            if (userName == null && !input.Contains("name"))
            {
                userName = input;
                AddMessage("Bot", $"Nice to meet you, {userName}! I'm your Cybersecurity Bot. Ask me about passwords, phishing, scams, or privacy!");
                synthesizer?.SpeakAsync($"Nice to meet you, {userName}! I'm your cybersecurity bot.");
                StatusText.Text = "Ready!";
                return;
            }

            // Follow-up handling
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
            {
                HandleFollowUp();
                StatusText.Text = "Ready!";
                return;
            }

            // Sentiment detection
            if (DetectSentiment(input))
            {
                StatusText.Text = "Ready!";
                return;
            }

            // Keyword recognition
            string keywordResponse = GetKeywordResponse(input);
            if (keywordResponse != null)
            {
                AddMessage("Bot", keywordResponse);
                synthesizer?.SpeakAsync(keywordResponse);
                lastTopic = keywordResponse;
                StatusText.Text = "Ready!";
                return;
            }

            // Topic handlers
            if (input.Contains("password"))
            {
                TopicPassword();
            }
            else if (input.Contains("phishing"))
            {
                TopicPhishing();
            }
            else if (input.Contains("scam"))
            {
                TopicScams();
            }
            else if (input.Contains("privacy"))
            {
                TopicPrivacy();
            }
            else if (input.Contains("help"))
            {
                ShowHelp();
            }
            else
            {
                string errorMsg = "I'm not sure I understand. Try 'password', 'phishing', 'scam', 'privacy', or 'help'.";
                AddMessage("Bot", errorMsg);
                synthesizer?.SpeakAsync(errorMsg);
            }

            StatusText.Text = "Ready!";
        }

        private bool DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous"))
            {
                AddMessage("Bot", "It's completely understandable to feel that way. Cybersecurity can feel overwhelming. Let me give you a simple tip to help.");
                synthesizer?.SpeakAsync("It's completely understandable to feel that way. Let me give you a simple tip.");

                string tip = phishingTips[random.Next(phishingTips.Length)];
                AddMessage("Bot", $"Tip: {tip}");
                synthesizer?.SpeakAsync(tip);
                return true;
            }

            if (input.Contains("curious") || input.Contains("interested"))
            {
                AddMessage("Bot", "That's great! Curiosity helps you learn. Let me share something useful about cybersecurity.");
                synthesizer?.SpeakAsync("That's great! Curiosity helps you learn.");
                return true;
            }

            if (input.Contains("frustrated") || input.Contains("annoyed"))
            {
                AddMessage("Bot", "I understand this can be frustrating. Let's take it step by step.");
                synthesizer?.SpeakAsync("I understand this can be frustrating. Let's take it step by step.");
                return true;
            }

            return false;
        }

        private string GetKeywordResponse(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    RememberFavoriteTopic(keyword);
                    return keywordResponses[keyword];
                }
            }
            return null;
        }

        private void HandleFollowUp()
        {
            if (lastTopic == null)
            {
                AddMessage("Bot", "Sure! What topic would you like more tips on? Try 'password', 'scam', or 'phishing'.");
                synthesizer?.SpeakAsync("What topic would you like more tips on?");
                return;
            }

            string randomTip = phishingTips[random.Next(phishingTips.Length)];
            AddMessage("Bot", $"Another tip: {randomTip}");
            synthesizer?.SpeakAsync(randomTip);
        }

        private void RememberFavoriteTopic(string topic)
        {
            if (favoriteTopic == null)
            {
                AddMessage("Bot", $"Great! I'll remember that you're interested in {topic}.");
                synthesizer?.SpeakAsync($"Great! I'll remember you're interested in {topic}.");
                favoriteTopic = topic;
            }
        }

        private void TopicPassword()
        {
            AddMessage("Bot", keywordResponses["password"]);
            synthesizer?.SpeakAsync(keywordResponses["password"]);
            lastTopic = "password";
            RememberFavoriteTopic("password");
        }

        private void TopicPhishing()
        {
            string randomTip = phishingTips[random.Next(phishingTips.Length)];
            AddMessage("Bot", $"PHISHING TIP: {randomTip}");
            synthesizer?.SpeakAsync(randomTip);
            lastTopic = "phishing";
            RememberFavoriteTopic("phishing");
        }

        private void TopicScams()
        {
            AddMessage("Bot", keywordResponses["scam"]);
            synthesizer?.SpeakAsync(keywordResponses["scam"]);
            lastTopic = "scam";
            RememberFavoriteTopic("scam");
        }

        private void TopicPrivacy()
        {
            AddMessage("Bot", keywordResponses["privacy"]);
            synthesizer?.SpeakAsync(keywordResponses["privacy"]);
            lastTopic = "privacy";
            RememberFavoriteTopic("privacy");
        }

        private void ShowHelp()
        {
            string help = @"
            Available Topics:
            • password - Password security tips
            • phishing - Random phishing prevention tips
            • scam - Recognize online scams
            • privacy - Protect personal information

            Commands:
            • 'tell me more' or 'another tip' - Get more advice
            • 'I'm worried' - I'll respond with encouragement
            • 'help' - Show this menu
            • 'exit' - End conversation

            Click the microphone button and speak naturally!";

            AddMessage("Bot", help);
            synthesizer?.SpeakAsync("Here's help. Ask about passwords, phishing, scams, or privacy.");
        }

        // Event Handlers
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserInputTextBox.Text))
            {
                string input = UserInputTextBox.Text;
                AddMessage("You", input);
                ProcessInput(input.ToLower());
                UserInputTextBox.Clear();
            }
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void VoiceInputButton_Click(object sender, RoutedEventArgs e)
        {
            AddMessage("Bot", "Voice input is not available in this simplified version. Please type your message.");
            StatusText.Text = "Type your message instead";
        }

        private void SpeakOutputButton_Click(object sender, RoutedEventArgs e)
        {
            if (synthesizer != null && chatMessages.Count > 0)
            {
                // Speak the last bot message (remove the [Bot] prefix)
                string lastMessage = chatMessages[chatMessages.Count - 1];
                if (lastMessage.StartsWith("[Bot]"))
                {
                    string toSpeak = lastMessage.Substring(6);
                    synthesizer.SpeakAsync(toSpeak);
                    StatusText.Text = "Speaking...";
                }
            }
        }
    }
}