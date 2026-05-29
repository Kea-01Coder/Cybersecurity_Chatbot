using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Cybersecurity_ChatbotWPF.Models;
using Cybersecurity_ChatbotWPF.Services;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
            // Chat messages collection
            private ObservableCollection<ChatMessage> chatMessages = new ObservableCollection<ChatMessage>();

            // Services
            private KeywordRecognizer keywordRecognizer;
            private SentimentAnalyzer sentimentAnalyzer;
            private ResponseManagers responseManager;
            private MemoryManager memoryManager;
            private SpeechServices speechService;

        // Custom voice recording player
        private MediaPlayer welcomePlayer;

        // State variables
        private string lastTopic = null;

        public MainWindow()
        {
            InitializeComponent();
            ChatListBox.ItemsSource = chatMessages;

            // Initialize all services
            InitializeServices();

            // Initialize your custom voice recordings
            InitializeVoiceRecordings();

            // Hook up speech events
            if (speechService != null)
            {
                speechService.SpeechRecognized += OnSpeechRecognized;
            }
            Welcome();
        }
          
           private void InitializeServices()
            {
                try
                {
                    keywordRecognizer = new KeywordRecognizer();
                    sentimentAnalyzer = new SentimentAnalyzer();
                    responseManager = new ResponseManagers();
                    memoryManager = new MemoryManager();
                    speechService = new SpeechServices(Dispatcher);
                }
                catch (Exception ex)
                {
                    AddMessage("System", $"Error initializing services: {ex.Message}", Brushes.Red);
                }
            }

            private void InitializeVoiceRecordings()
            {
                welcomePlayer = new MediaPlayer();
            }

            private void PlayCustomVoice(string audioFilePath, string fallbackText = null)
            {
                try
                {
                    welcomePlayer.Open(new Uri(audioFilePath));
                    welcomePlayer.Play();
                    // Waits for audio to finish
                    while (welcomePlayer.Position < welcomePlayer.NaturalDuration)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
                    // Fallback to text-to-speech if recording fails
                    if (!string.IsNullOrEmpty(fallbackText))
                    {
                        speechService?.Speak(fallbackText);
                    }
                }
            }

            private void OnSpeechRecognized(string command)
            {
                Dispatcher.Invoke(() =>
                {
                    AddMessage("Voice", command, Brushes.LightYellow);
                    ProcessInput(command.ToLower());
                });
            }

            private void Welcome()
            {
            // FIRST: Play your custom welcome voice recording
            PlayCustomVoice("welcome.wav","Welcome to CyberGuardian AI!");

            // THIRD: Ask for name using text-to-speech
            AddMessage("Bot", "I am your cybersecurity awareness bot. What's your name?", Brushes.LightGreen);
                speechService?.Speak("I am your cybersecurity awareness bot. What's your name?");
            }

            private void AddMessage(string sender, string message, Brush color)
            {
                Dispatcher.Invoke(() =>
                {
                    chatMessages.Add(new ChatMessage
                    {
                        Sender = sender,
                        Message = message,
                        SenderColor = color
                    });
                    ChatListBox.ScrollIntoView(chatMessages[chatMessages.Count - 1]);
                });
            }

            private void ProcessInput(string input)
            {
                // Update status
                var statusText = FindName("StatusText") as System.Windows.Controls.TextBlock;
                if (statusText != null)
                {
                    statusText.Text = "Thinking...";
                    statusText.Foreground = Brushes.Yellow;
                }

                // Handle exit
                if (input.Contains("exit") || input == "quit" || input == "bye")
                {
                    AddMessage("Bot", $"Goodbye {memoryManager.GetUserName()}! Stay safe online! ", Brushes.LightGreen);
                    speechService?.Speak($"Goodbye {memoryManager.GetUserName()}! Stay safe online!");
                    if (statusText != null)
                    {
                        statusText.Text = "Conversation ended";
                    }
                    return;
                }

                // Handle name (first interaction)
                if (!memoryManager.HasUserName() && !input.Contains("name") && !string.IsNullOrWhiteSpace(input))
                {
                    memoryManager.SetUserName(input);
                    AddMessage("Bot", $"Nice to meet you, {memoryManager.GetUserName()}! I'm your Cybersecurity Bot. Ask me about passwords, phishing, scams, or privacy!", Brushes.LightGreen);
                    speechService?.Speak($"Nice to meet you, {memoryManager.GetUserName()}! I'm your cybersecurity bot. Ask me about passwords, phishing, scams, or privacy.");
                    if (statusText != null)
                    {
                        statusText.Text = "Ready!";
                    }
                    return;
                }

                // Conversation flow: follow-up handling
                if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
                {
                    HandleFollowUp();
                    if (statusText != null)
                    {
                        statusText.Text = "Ready!";
                    }
                    return;
                }

                // Sentiment detection
                string sentiment = sentimentAnalyzer?.DetectSentiment(input);
                if (sentiment != null)
                {
                    RespondWithSentiment(sentiment);
                    if (statusText != null)
                    {
                        statusText.Text = "Ready!";
                    }
                    return;
                }

                // Keyword recognition
                string keywordResponse = keywordRecognizer?.GetKeywordResponse(input);
                if (keywordResponse != null)
                {
                    AddMessage("Bot", keywordResponse, Brushes.LightGreen);
                    speechService?.Speak(keywordResponse);
                    lastTopic = keywordRecognizer.GetKeywordTopic(input);
                    memoryManager?.RememberFavoriteTopic(lastTopic);
                    if (statusText != null)
                    {
                        statusText.Text = "Ready!";
                    }
                    return;
                }

                // Topic-based responses
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
                    // Error handling for unknown input
                    string errorMsg = "I'm not sure I understand. Try 'password', 'phishing', 'scam', 'privacy', or 'help'.";
                    AddMessage("Bot", errorMsg, Brushes.LightGreen);
                    speechService?.Speak(errorMsg);
                }

                if (statusText != null)
                {
                    statusText.Text = "Ready!";
                }
            }

            private void RespondWithSentiment(string sentiment)
            {
                string sentimentResponse = sentimentAnalyzer?.GetSentimentResponse(sentiment);
                if (!string.IsNullOrEmpty(sentimentResponse))
                {
                    AddMessage("Bot", sentimentResponse, Brushes.LightGreen);
                    speechService?.Speak(sentimentResponse);

                    if (sentiment == "worried")
                    {
                        string randomTip = responseManager?.GetRandomPhishingTip();
                        if (!string.IsNullOrEmpty(randomTip))
                        {
                            AddMessage("Bot", $"💡 Tip: {randomTip}", Brushes.LightGreen);
                            speechService?.Speak(randomTip);
                        }
                    }
                }
            }

            private void HandleFollowUp()
            {
                string followUp = responseManager?.GetFollowUpResponse(lastTopic);
                if (!string.IsNullOrEmpty(followUp))
                {
                    AddMessage("Bot", followUp, Brushes.LightGreen);
                    speechService?.Speak(followUp);
                }
            }

            private void TopicPassword()
            {
                string response = keywordRecognizer?.GetKeywordResponse("password");
                if (!string.IsNullOrEmpty(response))
                {
                    AddMessage("Bot", response, Brushes.LightGreen);
                    speechService?.Speak(response);
                    lastTopic = "password";
                    memoryManager?.RememberFavoriteTopic("password");
                }
            }

            private void TopicPhishing()
            {
                string randomTip = responseManager?.GetRandomPhishingTip();
                if (!string.IsNullOrEmpty(randomTip))
                {
                    AddMessage("Bot", $"PHISHING TIP: {randomTip}", Brushes.LightGreen);
                    speechService?.Speak(randomTip);
                    lastTopic = "phishing";
                    memoryManager?.RememberFavoriteTopic("phishing");
                }
            }

            private void TopicScams()
            {
                string response = keywordRecognizer?.GetKeywordResponse("scam");
                if (!string.IsNullOrEmpty(response))
                {
                    AddMessage("Bot", response, Brushes.LightGreen);
                    speechService?.Speak(response);
                    lastTopic = "scam";
                    memoryManager?.RememberFavoriteTopic("scam");
                }
            }

            private void TopicPrivacy()
            {
                string response = keywordRecognizer?.GetKeywordResponse("privacy");
                if (!string.IsNullOrEmpty(response))
                {
                    AddMessage("Bot", response, Brushes.LightGreen);
                    speechService?.Speak(response);
                    lastTopic = "privacy";
                    memoryManager?.RememberFavoriteTopic("privacy");
                }
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
                    • 'exit' - End conversation";

                AddMessage("Bot", help, Brushes.LightGreen);
                speechService?.Speak("Here's help. You can ask about passwords, phishing, scams, or privacy.");
            }

            // Quick Tip Button Handlers
            private void QuickTipPassword_Click(object sender, RoutedEventArgs e)
            {
                AddMessage("You", "Tell me about password safety", Brushes.LightBlue);
                TopicPassword();
            }

            private void QuickTipPhishing_Click(object sender, RoutedEventArgs e)
            {
                AddMessage("You", "Give me a phishing tip", Brushes.LightBlue);
                TopicPhishing();
            }

            private void QuickTipScam_Click(object sender, RoutedEventArgs e)
            {
                AddMessage("You", "Tell me about scams", Brushes.LightBlue);
                TopicScams();
            }

            private void QuickTipPrivacy_Click(object sender, RoutedEventArgs e)
            {
                AddMessage("You", "Tell me about privacy", Brushes.LightBlue);
                TopicPrivacy();
            }

            private void QuickTipHelp_Click(object sender, RoutedEventArgs e)
            {
                AddMessage("You", "Help", Brushes.LightBlue);
                ShowHelp();
            }

            // Event Handlers
            private void SendButton_Click(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrWhiteSpace(UserInputTextBox.Text))
                {
                    string input = UserInputTextBox.Text;
                    AddMessage("You", input, Brushes.LightBlue);
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

            private void SpeakOutputButton_Click(object sender, RoutedEventArgs e)
            {
                // Speak the last bot message
                for (int i = chatMessages.Count - 1; i >= 0; i--)
                {
                    if (chatMessages[i].Sender == "Bot" || chatMessages[i].Sender == "System")
                    {
                        speechService?.Speak(chatMessages[i].Message);
                        var statusText = FindName("StatusText") as System.Windows.Controls.TextBlock;
                        if (statusText != null)
                        {
                            statusText.Text = "Speaking...";
                        }
                        break;
                    }
                }
            }
        }
    }

