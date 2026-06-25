using System;
using System.Collections.Generic;
using System.Linq;
using Cybersecurity_ChatbotWPF.Models;

namespace Cybersecurity_ChatbotWPF.Services
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        private bool isQuizActive;
        private DatabaseHelper dbHelper;
        private ActivityLogger activityLogger;
        private string userName;

        public QuizManager(DatabaseHelper dbHelper, ActivityLogger logger)
        {
            this.dbHelper = dbHelper;
            this.activityLogger = logger;
            InitializeQuestions();
            ResetQuiz();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                // Multiple Choice Questions
                new QuizQuestion
                {
                    Id = 1,
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others from falling victim.",
                    Category = "Phishing"
                },
                new QuizQuestion
                {
                    Id = 2,
                    QuestionText = "Which of the following is a strong password?",
                    Options = new List<string> { "password123", "John1985", "P@ssw0rd!", "qwerty" },
                    CorrectAnswerIndex = 2,
                    Explanation = "A strong password uses a mix of uppercase, lowercase, numbers, and special characters.",
                    Category = "Password"
                },
                new QuizQuestion
                {
                    Id = 3,
                    QuestionText = "What is two-factor authentication (2FA)?",
                    Options = new List<string>
                    {
                        "A password that is two words long",
                        "A security method that requires two forms of verification",
                        "A type of antivirus software",
                        "A way to reset your password"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification beyond your password.",
                    Category = "Authentication"
                },
                // True/False Questions
                new QuizQuestion
                {
                    Id = 4,
                    QuestionText = "True or False: Public Wi-Fi networks are always safe to use for online banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi networks are not always secure. Avoid accessing sensitive accounts on public networks.",
                    Category = "Safe Browsing"
                },
                new QuizQuestion
                {
                    Id = 5,
                    QuestionText = "True or False: You should use the same password for all your accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Using the same password for all accounts makes you vulnerable - if one account is compromised, all are at risk.",
                    Category = "Password"
                },
                new QuizQuestion
                {
                    Id = 6,
                    QuestionText = "What is social engineering in cybersecurity?",
                    Options = new List<string>
                    {
                        "A type of computer virus",
                        "Manipulating people to reveal confidential information",
                        "Building social media profiles",
                        "A way to hack social media accounts"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering is psychological manipulation that tricks users into revealing sensitive information.",
                    Category = "Security"
                },
                new QuizQuestion
                {
                    Id = 7,
                    QuestionText = "True or False: Ransomware is a type of malware that encrypts your files and demands payment.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Ransomware encrypts your files and holds them hostage until you pay a ransom.",
                    Category = "Malware"
                },
                new QuizQuestion
                {
                    Id = 8,
                    QuestionText = "What should you do before downloading an app?",
                    Options = new List<string>
                    {
                        "Download it immediately",
                        "Check the app permissions and reviews",
                        "Share your personal information",
                        "Ignore the security warnings"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Always review app permissions and read reviews before downloading to ensure safety.",
                    Category = "Safe Browsing"
                },
                new QuizQuestion
                {
                    Id = 9,
                    QuestionText = "True or False: HTTPS indicates a secure website connection.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 0,
                    Explanation = "HTTPS encrypts the connection between your browser and the website for added security.",
                    Category = "Safe Browsing"
                },
                new QuizQuestion
                {
                    Id = 10,
                    QuestionText = "Which of the following is a sign of a phishing email?",
                    Options = new List<string>
                    {
                        "Personalized greeting with your name",
                        "Urgent language asking for immediate action",
                        "Correct grammar and spelling",
                        "Familiar sender address"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing emails often create urgency to make you act quickly without thinking.",
                    Category = "Phishing"
                },
                new QuizQuestion
                {
                    Id = 11,
                    QuestionText = "True or False: Sharing your location on social media is always safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Sharing your location can reveal patterns and compromise your physical safety.",
                    Category = "Privacy"
                },
                new QuizQuestion
                {
                    Id = 12,
                    QuestionText = "What is the best way to secure your home Wi-Fi network?",
                    Options = new List<string>
                    {
                        "Use the default password",
                        "Enable WPA3 or WPA2 encryption",
                        "Keep the network name visible",
                        "Disable firewall protection"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "WPA3 (or WPA2) encryption protects your network from unauthorized access.",
                    Category = "Privacy"
                }
            };
        }

        public void StartQuiz(string name)
        {
            userName = name;
            ResetQuiz();
            isQuizActive = true;
            activityLogger.Log("Quiz Started", $"User: {userName}", "Quiz");
        }

        private void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
        }

        public bool IsQuizActive() => isQuizActive;

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
                return questions[currentQuestionIndex];
            return null;
        }

        public string SubmitAnswer(int selectedIndex)
        {
            var question = questions[currentQuestionIndex];
            bool correct = question.IsCorrect(selectedIndex);

            if (correct)
            {
                score++;
                activityLogger.Log("Quiz Question Correct", $"{question.QuestionText} - Answer: {question.Options[selectedIndex]}", "Quiz");
                return $"Correct! {question.Explanation}";
            }
            else
            {
                activityLogger.Log("Quiz Question Incorrect", $"{question.QuestionText} - User chose: {question.Options[selectedIndex]}", "Quiz");
                return $"Incorrect. {question.Explanation}";
            }
        }

        public QuizQuestion GetNextQuestion()
        {
            if (currentQuestionIndex + 1 < questions.Count)
            {
                currentQuestionIndex++;
                return questions[currentQuestionIndex];
            }
            return null;
        }

        public string GetQuizResult()
        {
            int total = questions.Count;
            double percentage = (double)score / total * 100;

            string feedback;
            if (percentage >= 80)
                feedback = "Excellent! You're a cybersecurity pro! Keep up the great work!";
            else if (percentage >= 60)
                feedback = "Good job! You have a solid understanding of cybersecurity basics!";
            else if (percentage >= 40)
                feedback = "Not bad! Keep learning and you'll become a cybersecurity expert!";
            else
                feedback = "Keep going! Cybersecurity is important - keep learning to stay safe online!";

            return $"Quiz Complete! You scored {score} out of {total} ({percentage:F0}%). {feedback}";
        }

        public string FormatQuestion(QuizQuestion question)
        {
            string options = "";
            for (int i = 0; i < question.Options.Count; i++)
            {
                options += $"{i + 1}. {question.Options[i]}\n";
            }
            return $"**{question.QuestionText}**\n\n{options}\n\nType the number of your answer (1-{question.Options.Count})";
        }

        public string StartQuizPrompt()
        {
            if (isQuizActive)
                return "A quiz is already in progress! Answer the current question.";

            activityLogger.Log("Quiz Started (via prompt)", "", "Quiz");
            ResetQuiz();
            isQuizActive = true;
            var firstQuestion = questions[0];
            return $"**Welcome to the Cybersecurity Quiz!**\n\nAnswer all {questions.Count} questions.\n\n{FormatQuestion(firstQuestion)}";
        }

        public bool IsAnswerCorrect(int selectedIndex)
        {
            if (currentQuestionIndex >= questions.Count) return false;
            return questions[currentQuestionIndex].IsCorrect(selectedIndex);
        }

        public string GetExplanation(int selectedIndex)
        {
            if (currentQuestionIndex >= questions.Count) return "No explanation available.";
            return questions[currentQuestionIndex].Explanation;
        }

        public int GetScore()
        {
            return score;
        }

        public bool IsQuizComplete()
        {
            return currentQuestionIndex >= questions.Count - 1;
        }

        
        public int GetTotalQuestions() => questions.Count;
        public int GetCurrentQuestionIndex() => currentQuestionIndex + 1;
    }
}
