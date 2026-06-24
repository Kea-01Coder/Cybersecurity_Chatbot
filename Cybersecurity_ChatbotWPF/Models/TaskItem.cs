using System;

namespace Cybersecurity_ChatbotWPF.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; } // e.g., "Password", "Privacy", "General"

        public TaskItem()
        {
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "Completed" : "Pending";
            string reminder = ReminderDate.HasValue ? $"Reminder: {ReminderDate.Value:yyyy-MM-dd}" : "No reminder";
            return $"{Title} - {status} - {reminder}";
        }
    }
}
