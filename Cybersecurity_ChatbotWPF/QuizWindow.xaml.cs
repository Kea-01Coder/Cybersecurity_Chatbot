using Cybersecurity_ChatbotWPF.Models;
using Cybersecurity_ChatbotWPF.Services;
using Cybersecurity_ChatbotWPF.Models;
using Cybersecurity_ChatbotWPF.Services;
using System;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cybersecurity_ChatbotWPF
{
    public partial class QuizWindow : Window
    {
        private QuizManager quizManager;
        private List<string> currentOptions;
        private int currentQuestionIndex;
        private bool isAnswered;

        public event EventHandler QuizCompleted;

        public QuizWindow(QuizManager manager)
        {
            InitializeComponent();
            quizManager = manager;
            LoadFirstQuestion();
        }

        private void LoadFirstQuestion()
        {
            var question = quizManager.GetCurrentQuestion();
            if (question != null)
            {
                DisplayQuestion(question);
                UpdateProgress();
            }
            else
            {
                // No questions available
                QuestionText.Text = "No questions available.";
                AnswerOptions.Visibility = Visibility.Collapsed;
                StatusText.Text = "Quiz could not be loaded.";
            }
        }

        private void DisplayQuestion(QuizQuestion question)
        {
            // Reset state
            isAnswered = false;
            FeedbackBorder.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;

            // Display question
            CategoryText.Text = $"{question.Category ?? "General"}";
            QuestionText.Text = question.QuestionText;

            // Display options
            currentOptions = question.Options;
            AnswerOptions.ItemsSource = currentOptions;
            AnswerOptions.Visibility = Visibility.Visible;

            // Enable buttons
            EnableAnswerButtons(true);

            // Update status
            currentQuestionIndex = quizManager.GetCurrentQuestionIndex();
            StatusText.Text = $"Question {currentQuestionIndex} of {quizManager.GetTotalQuestions()}";
        }

        private void EnableAnswerButtons(bool enabled)
        {
            foreach (var item in AnswerOptions.Items)
            {
                var container = AnswerOptions.ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    var button = FindVisualChild<Button>(container);
                    if (button != null)
                        button.IsEnabled = enabled;
                }
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAnswered) return;

            var button = sender as Button;
            int selectedIndex = currentOptions.IndexOf(button.Content.ToString());

            if (selectedIndex == -1) return;

            // Disable buttons to prevent double-clicking
            EnableAnswerButtons(false);
            isAnswered = true;

            // Check answer
            bool correct = quizManager.IsAnswerCorrect(selectedIndex);
            string feedback = quizManager.SubmitAnswer(selectedIndex);
            string explanation = quizManager.GetExplanation(selectedIndex);

            // Show feedback
            FeedbackBorder.Visibility = Visibility.Visible;
            FeedbackText.Text = correct ? "Correct! Well done!" : "Incorrect. Let's learn from this!";
            FeedbackText.Foreground = correct ?
                new SolidColorBrush(Colors.LightGreen) :
                new SolidColorBrush(Colors.Salmon);

            ExplanationText.Text = explanation;

            // Highlight correct answer (visual feedback)
            HighlightAnswer(selectedIndex, correct);

            // Update score
            ScoreText.Text = quizManager.GetScore().ToString();

            // Check if quiz is complete
            if (quizManager.IsQuizComplete())
            {
                NextButton.Content = "See Results";
                NextButton.Visibility = Visibility.Visible;
                StatusText.Text = "Quiz complete! Click 'See Results' to view your score.";
            }
            else
            {
                NextButton.Content = "Next Question →";
                NextButton.Visibility = Visibility.Visible;
            }
        }

        private void HighlightAnswer(int selectedIndex, bool correct)
        {
            // Get all answer buttons
            var buttons = new List<Button>();
            foreach (var item in AnswerOptions.Items)
            {
                var container = AnswerOptions.ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    var btn = FindVisualChild<Button>(container);
                    if (btn != null)
                    {
                        buttons.Add(btn);
                    }
                }
            }

            // Reset all buttons
            foreach (var btn in buttons)
            {
                btn.Background = new SolidColorBrush(Color.FromRgb(26, 26, 46)); // #1A1A2E
                btn.Foreground = new SolidColorBrush(Colors.LightGray);
            }

            // Highlight selected
            if (selectedIndex < buttons.Count)
            {
                if (correct)
                {
                    buttons[selectedIndex].Background = new SolidColorBrush(Color.FromRgb(46, 125, 50)); // Green
                    buttons[selectedIndex].Foreground = new SolidColorBrush(Colors.DarkGray);
                }
                else
                {
                    buttons[selectedIndex].Background = new SolidColorBrush(Color.FromRgb(183, 28, 28)); // Red
                    buttons[selectedIndex].Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (quizManager.IsQuizComplete())
            {
                // Quiz completed - show results and close
                string result = quizManager.GetQuizResult();
                MessageBox.Show($"{result}", "Quiz Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                // Fire event and close
                QuizCompleted?.Invoke(this, EventArgs.Empty);
                this.Close();
                return;
            }

            // Load next question
            var nextQuestion = quizManager.GetNextQuestion();
            if (nextQuestion != null)
            {
                DisplayQuestion(nextQuestion);
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            int current = quizManager.GetCurrentQuestionIndex();
            int total = quizManager.GetTotalQuestions();
            ProgressText.Text = $"{current}/{total}";
            ProgressBar.Value = (double)(current - 1) / total * 100;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit the quiz? Your progress won't be saved.",
                                         "Quit Quiz",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        // Helper method to find a child element of a specific type
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            QuizCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
