using System;
using System.Collections.Generic;
using System.Text;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class TaskManager
    {
        private DatabaseHelper dbHelper;
        private ActivityLogger activityLogger;

        public TaskManager(DatabaseHelper dbHelper, ActivityLogger logger)
        {
            this.dbHelper = dbHelper;
            this.activityLogger = logger;
        }

        public string AddTask(string title, string description = "", string reminderDays = null)
        {
            var task = new TaskItem
            {
                Title = title,
                Description = string.IsNullOrEmpty(description) ? title : description,
                Category = DetectCategory(title)
            };

            if (!string.IsNullOrEmpty(reminderDays) && int.TryParse(reminderDays, out int days))
            {
                task.ReminderDate = DateTime.Now.AddDays(days);
            }

            int id = dbHelper.AddTask(task);
            string reminderMsg = task.ReminderDate.HasValue ?
                $"Reminder set for {task.ReminderDate.Value:yyyy-MM-dd}" :
                "No reminder set";

            activityLogger.Log($"Task Added: {task.Title}", $"Reminder: {reminderMsg}", "Task");

            return $"Task '{task.Title}' added successfully! {reminderMsg}";
        }

        public string ListTasks()
        {
            var tasks = dbHelper.GetAllTasks();
            if (tasks.Count == 0)
                return "No tasks found. Add a task using: 'Add task: [title]'";

            var sb = new StringBuilder("**Your Tasks:**\n\n");
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Yes" : "Still in progress";
                string reminder = task.ReminderDate.HasValue ?
                    $"{task.ReminderDate.Value:yyyy-MM-dd}" : "";
                sb.AppendLine($"{status} {task.Title} {reminder}");
                if (!string.IsNullOrEmpty(task.Description))
                    sb.AppendLine($"   {task.Description}");
            }
            return sb.ToString();
        }

        public string DeleteTask(string taskTitle)
        {
            var tasks = dbHelper.GetAllTasks();
            var task = tasks.Find(t => t.Title.Contains(taskTitle, StringComparison.OrdinalIgnoreCase) && !t.IsCompleted);

            if (task == null)
                return "Task not found. Please specify the exact task title.";

            dbHelper.DeleteTask(task.Id);
            activityLogger.Log($"Task Deleted: {task.Title}", "", "Task");
            return $"Task '{task.Title}' deleted successfully!";
        }

        public string CompleteTask(string taskTitle)
        {
            var tasks = dbHelper.GetAllTasks();
            var task = tasks.Find(t => t.Title.Contains(taskTitle, StringComparison.OrdinalIgnoreCase) && !t.IsCompleted);

            if (task == null)
                return "Task not found or already completed.";

            dbHelper.MarkTaskCompleted(task.Id);
            activityLogger.Log($"Task Completed: {task.Title}", "", "Task");
            return $"Task '{task.Title}' marked as completed! Great work!";
        }

        private string DetectCategory(string title)
        {
            string lower = title.ToLower();
            if (lower.Contains("password") || lower.Contains("2fa") || lower.Contains("authentication"))
                return "Password";
            if (lower.Contains("privacy") || lower.Contains("data") || lower.Contains("permission"))
                return "Privacy";
            if (lower.Contains("phishing") || lower.Contains("scam") || lower.Contains("fraud"))
                return "Security";
            return "General";
        }
    }
}
