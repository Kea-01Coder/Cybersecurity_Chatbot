using System;
using System.Collections.Generic;

namespace Cybersecurity_ChatbotWPF.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
        public string Category { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
        }

        public bool IsCorrect(int selectedIndex)
        {
            return selectedIndex == CorrectAnswerIndex;
        }
    }
}
