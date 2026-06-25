using Cybersecurity_ChatbotWPF;
using Cybersecurity_ChatbotWPF.Models;
using Cybersecurity_ChatbotWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

        // NEW Part 3 Services
        private DatabaseHelper dbHelper;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private NLPSimulator nlpSimulator;
        private ActivityLogger activityLogger;

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
                // Part 2 Services
                keywordRecognizer = new KeywordRecognizer();
                sentimentAnalyzer = new SentimentAnalyzer();
                responseManager = new ResponseManagers();
                memoryManager = new MemoryManager();
                speechService = new SpeechServices(Dispatcher);

                // NEW Part 3 Services
                dbHelper = new DatabaseHelper();
                activityLogger = new ActivityLogger(dbHelper);
                taskManager = new TaskManager(dbHelper, activityLogger);
                quizManager = new QuizManager(dbHelper, activityLogger);
                nlpSimulator = new NLPSimulator();

                // Log application start
                activityLogger.Log("Application Started", "CyberGuardian AI launched", "System");
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
                // Get full path
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", audioFilePath);

                if (System.IO.File.Exists(fullPath))
                {
                    welcomePlayer.Open(new Uri(fullPath));
                    welcomePlayer.Play();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Audio file not found: {fullPath}");
                    if (!string.IsNullOrEmpty(fallbackText))
                    {
                        speechService?.Speak(fallbackText);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
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
                AddMessage("🎤 Voice", command, Brushes.LightYellow);
                ProcessInput(command.ToLower());
            });
        }

        private void Welcome()
        {
            // Play welcome voice
            PlayCustomVoice("welcome.wav", "Welcome to CyberGuardian AI!");

            // Welcome message
            AddMessage("Bot", "I am your cybersecurity awareness bot. What's your name?", Brushes.LightGreen);
            speechService?.Speak("I am your cybersecurity awareness bot. What's your name?");

            // Show help
            ShowHelp();
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

        // =====================================================
        // ============ MAIN PROCESS INPUT METHOD ==============
        // =====================================================

        private void ProcessInput(string input)
        {
            // Update status
            var statusText = FindName("StatusText") as System.Windows.Controls.TextBlock;
            if (statusText != null)
            {
                statusText.Text = "Thinking...";
                statusText.Foreground = Brushes.Yellow;
            }

            // =====================================================
            // ==== NEW: NLP SIMULATION (Part 3 - Task 3) ========
            // =====================================================

            string intent = nlpSimulator?.DetectIntent(input);

            // Handle New Part 3 Intents
            switch (intent)
            {
                // --- HELP ---
                case "help":
                    ShowHelp();
                    if (statusText != null) statusText.Text = "Ready!";
                    return;

                // --- TASK MANAGEMENT (Part 3 - Task 1) ---
                case "add_task":
                    HandleAddTask(input);
                    if (statusText != null) statusText.Text = "Task added!";
                    return;

                case "list_tasks":
                    string taskList = taskManager?.ListTasks() ?? "No tasks found.";
                    AddMessage("Bot", taskList, Brushes.LightGreen);
                    speechService?.Speak("Here are your tasks.");
                    if (statusText != null) statusText.Text = "Ready!";
                    return;

                case "complete_task":
                    HandleCompleteTask(input);
                    if (statusText != null) statusText.Text = "Task completed!";
                    return;

                case "delete_task":
                    HandleDeleteTask(input);
                    if (statusText != null) statusText.Text = "Task deleted!";
                    return;

                // --- QUIZ (Part 3 - Task 2) ---
                case "start_quiz":
                    HandleStartQuiz();
                    if (statusText != null) statusText.Text = "Quiz in progress!";
                    return;

                case "answer_quiz":
                    HandleQuizAnswer(input);
                    if (statusText != null) statusText.Text = "Quiz in progress!";
                    return;

                // --- ACTIVITY LOG (Part 3 - Task 4) ---
                case "show_log":
                case "show_activity":
                    string logDisplay = activityLogger?.GetActivityLogDisplay() ?? "No activity logs found.";
                    AddMessage("Bot", logDisplay, Brushes.LightGreen);
                    speechService?.Speak("Here's your activity log.");
                    if (statusText != null) statusText.Text = "Ready!";
                    return;

                // --- CYBERSECURITY TOPICS (Enhanced with NLP) ---
                case "password":
                case "phishing":
                case "privacy":
                case "malware":
                case "browsing":
                case "social_engineering":
                    HandleCybersecurityTopic(intent);
                    if (statusText != null) statusText.Text = "Ready!";
                    return;

                default:
                    // =====================================================
                    // ==== FALLBACK: ALL YOUR ORIGINAL PART 2 LOGIC ======
                    // =====================================================
                    HandlePart2Logic(input);
                    if (statusText != null) statusText.Text = "Ready!";
                    return;
            }
        }

        // =====================================================
        // =============== NEW: PART 3 HANDLERS ================
        // =====================================================

        private void HandleAddTask(string input)
        {
            string taskTitle = nlpSimulator?.ExtractTaskInfo(input) ?? "";
            string reminderDays = nlpSimulator?.ExtractReminderDays(input) ?? "";

            if (string.IsNullOrEmpty(taskTitle))
            {
                AddMessage("Bot", "What task would you like to add? Please specify a title.", Brushes.LightGreen);
                speechService?.Speak("What task would you like to add?");
                return;
            }

            string result = taskManager?.AddTask(taskTitle, "", reminderDays) ?? "Failed to add task.";
            AddMessage("Bot", result, Brushes.LightGreen);
            speechService?.Speak($"Task added: {taskTitle}");

            // Log NLP interaction
            activityLogger?.Log("NLP: Task Added", $"Title: {taskTitle}, Reminder: {reminderDays ?? "none"}", "NLP");
        }

        private void HandleCompleteTask(string input)
        {
            string taskTitle = nlpSimulator?.ExtractTaskInfo(input) ?? "";
            if (string.IsNullOrEmpty(taskTitle))
            {
                AddMessage("Bot", "Which task would you like to mark as completed?", Brushes.LightGreen);
                return;
            }

            string result = taskManager?.CompleteTask(taskTitle) ?? "Task not found.";
            AddMessage("Bot", result, Brushes.LightGreen);
            speechService?.Speak(result);
        }

        private void HandleDeleteTask(string input)
        {
            string taskTitle = nlpSimulator?.ExtractTaskInfo(input) ?? "";
            if (string.IsNullOrEmpty(taskTitle))
            {
                AddMessage("Bot", "Which task would you like to delete?", Brushes.LightGreen);
                return;
            }

            string result = taskManager?.DeleteTask(taskTitle) ?? "Task not found.";
            AddMessage("Bot", result, Brushes.LightGreen);
            speechService?.Speak(result);
        }

        private void HandleStartQuiz()
        {
            string userName = memoryManager?.GetUserName() ?? "Anonymous";
            quizManager?.StartQuiz(userName);
            string startMsg = quizManager?.StartQuizPrompt() ?? "Starting quiz!";
            AddMessage("Bot", startMsg, Brushes.LightGreen);
            speechService?.Speak("Starting quiz!");
            activityLogger?.Log("Quiz Started", $"User: {userName}", "Quiz");
        }

        private void HandleQuizAnswer(string input)
        {
            if (quizManager == null || !quizManager.IsQuizActive())
            {
                AddMessage("Bot", "No quiz is currently active. Type 'Start quiz' to begin!", Brushes.LightGreen);
                return;
            }

            string answerStr = nlpSimulator?.ExtractQuizAnswer(input) ?? "";
            if (string.IsNullOrEmpty(answerStr) || !int.TryParse(answerStr, out int answerIndex))
            {
                var currentQuestion = quizManager.GetCurrentQuestion();
                if (currentQuestion != null)
                {
                    AddMessage("Bot", $"Please type the number of your answer (1-{currentQuestion.Options.Count})", Brushes.LightGreen);
                }
                return;
            }

            // Submit answer (adjust for 0-based index)
            int selectedIndex = answerIndex - 1;
            var question = quizManager.GetCurrentQuestion();

            if (selectedIndex < 0 || selectedIndex >= question.Options.Count)
            {
                AddMessage("Bot", $"Invalid answer. Please choose a number between 1 and {question.Options.Count}", Brushes.LightGreen);
                return;
            }

            // Submit the answer and get feedback
            string feedback = quizManager.SubmitAnswer(selectedIndex);
            AddMessage("Bot", feedback, Brushes.LightGreen);
            speechService?.Speak(feedback);

            // Get next question as QuizQuestion object
            var nextQuestion = quizManager.GetNextQuestion();

            if (nextQuestion != null)
            {
                // There's a next question - display it
                string nextQuestionText = quizManager.FormatQuestion(nextQuestion);
                AddMessage("Bot", nextQuestionText, Brushes.LightGreen);
            }
            else
            {
                // Quiz is complete!
                string result = quizManager.GetQuizResult();
                AddMessage("Bot", $"{result}", Brushes.LightGreen);
                speechService?.Speak(result);
                activityLogger?.Log("Quiz Completed", $"Score: {quizManager.GetScore()}/{quizManager.GetTotalQuestions()}", "Quiz");

                // Save score to database
                string userName = memoryManager?.GetUserName() ?? "Anonymous";
                dbHelper?.SaveQuizScore(userName, quizManager.GetScore(), quizManager.GetTotalQuestions());
            }
        }

        private void HandleCybersecurityTopic(string topic)
        {
            string response = keywordRecognizer?.GetKeywordResponse(topic) ??
                              $"Here's what I know about {topic}: It's important to stay informed and follow best practices to protect yourself online.";

            AddMessage("Bot", response, Brushes.LightGreen);
            speechService?.Speak(response);
            lastTopic = topic;
            activityLogger?.Log("Cybersecurity Info", $"Topic: {topic}", "Info");
        }

        private void HandlePart2Logic(string input)
        {
            // Handle exit
            if (input.Contains("exit") || input == "quit" || input == "bye")
            {
                AddMessage("Bot", $"Goodbye {memoryManager.GetUserName()}! Stay safe online! ", Brushes.LightGreen);
                speechService?.Speak($"Goodbye {memoryManager.GetUserName()}! Stay safe online!");
                activityLogger?.Log("Chat Ended", $"User: {memoryManager.GetUserName()}", "System");
                return;
            }

            // Handle name (first interaction)
            if (!memoryManager.HasUserName() && !input.Contains("name") && !string.IsNullOrWhiteSpace(input))
            {
                memoryManager.SetUserName(input);
                AddMessage("Bot", $"Nice to meet you, {memoryManager.GetUserName()}! I'm your Cybersecurity Bot. Ask me about passwords, phishing, scams, or privacy!", Brushes.LightGreen);
                speechService?.Speak($"Nice to meet you, {memoryManager.GetUserName()}! I'm your cybersecurity bot. Ask me about passwords, phishing, scams, or privacy.");
                return;
            }

            // Conversation flow: follow-up handling
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
            {
                HandleFollowUp();
                return;
            }

            // Sentiment detection
            string sentiment = sentimentAnalyzer?.DetectSentiment(input);
            if (sentiment != null)
            {
                RespondWithSentiment(sentiment);
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
                activityLogger?.Log("Unknown Input", $"User said: {input}", "Error");
            }
        }

        // =====================================================
        // ==== YOUR ORIGINAL HELPERS (Unchanged) ==============
        // =====================================================

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
                        AddMessage("Bot", $"Tip: {randomTip}", Brushes.LightGreen);
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
                **What I can help you with:**

                **Tasks:**
                • 'Add task: [title]' - Create a new task
                • 'Add task with reminder in 3 days: [title]'
                • 'List tasks' - View all tasks
                • 'Complete task: [title]' - Mark as done
                • 'Delete task: [title]' - Remove a task

                **Quiz:**
                • 'Start quiz' - Begin cybersecurity quiz
                • Type answer number (1, 2, 3...)

                **Activity Log:**
                • 'Show activity log' - View recent actions

                **Cybersecurity Topics:**
                • 'password', 'phishing', 'scam', 'privacy', 'malware'

                **Help:**
                • 'help' or 'what can you do'";

            AddMessage("Bot", help, Brushes.LightGreen);
            speechService?.Speak("Here's what I can help you with.");
        }

        // =====================================================
        // ==== QUICK TIP BUTTONS (Updated with Part 3) ========
        // =====================================================

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

        // NEW: Tasks Button
        private void QuickTipTasks_Click(object sender, RoutedEventArgs e)
        {
            AddMessage("You", "List tasks", Brushes.LightBlue);
            string taskList = taskManager?.ListTasks() ?? "No tasks found.";
            AddMessage("Bot", taskList, Brushes.LightGreen);
            speechService?.Speak("Here are your tasks.");
        }

        private void QuickTipHelp_Click(object sender, RoutedEventArgs e)
        {
            AddMessage("You", "Help", Brushes.LightBlue);
            ShowHelp();
        }

        // =====================================================
        // ================= EVENT HANDLERS ====================
        // =====================================================

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

        private void VoiceInputButton_Click(object sender, RoutedEventArgs e)
        {
            if (speechService != null && speechService.IsSpeechRecognitionAvailable())
            {
                var statusText = FindName("StatusText") as System.Windows.Controls.TextBlock;
                if (statusText != null)
                {
                    statusText.Text = "Listening... Speak now!";
                    statusText.Foreground = Brushes.Yellow;
                }
                speechService.StartListening();
            }
            else
            {
                AddMessage("Bot", "Voice recognition is not available on this system.", Brushes.Orange);
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

        private void QuickTipQuiz_Click(object sender, RoutedEventArgs e)
        {
            OpenQuizWindow();
        }

        private void OpenQuizWindow()
        {
            try
            {
                // Check if database is available
                if (dbHelper == null)
                {
                    AddMessage("Bot", "Database is not available. Cannot start quiz.", Brushes.Orange);
                    return;
                }

                // Initialize quiz manager if needed
                if (quizManager == null)
                    quizManager = new QuizManager(dbHelper, activityLogger);

                // Create and show quiz window
                var quizWindow = new QuizWindow(quizManager);
                quizWindow.Owner = this;
                quizWindow.QuizCompleted += (s, args) =>
                {
                    AddMessage("Bot", "Quiz completed! Well done!", Brushes.LightGreen);
                    activityLogger?.Log("Quiz Completed", "User finished the quiz", "Quiz");
                };
                quizWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                AddMessage("Bot", $"Error starting quiz: {ex.Message}", Brushes.Orange);
                System.Diagnostics.Debug.WriteLine($"Quiz error: {ex.Message}");
            }
        }
    }
}
