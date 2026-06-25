using System;
using System.Collections.Generic;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class ActivityLogger
	{
        private DatabaseHelper dbHelper;
        private List<ActivityLogEntry> inMemoryLogs;
        private const int MAX_IN_MEMORY_LOGS = 50;

        public ActivityLogger(DatabaseHelper dbHelper)
		{
            this.dbHelper = dbHelper;
            inMemoryLogs = new List<ActivityLogEntry>();
        }

        public void Log(string action, string description = "", string category = "General")
        {
            var entry = new ActivityLogEntry
            {
                Action = action,
                Description = description ?? "",
                Timestamp = DateTime.Now,
                Category = category
            };
            inMemoryLogs.Add(entry);
            if (inMemoryLogs.Count > MAX_IN_MEMORY_LOGS)
                inMemoryLogs.RemoveAt(0);

            // Save to database
            try
            {
                dbHelper.AddActivityLog(action, description, category);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save log to database: {ex.Message}");
            }
        }

        public List<ActivityLogEntry> GetRecentLogs(int count = 10)
        {
            try
            {
                return dbHelper.GetRecentActivityLogs(count);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to retrieve logs from database: {ex.Message}");
                // Fallback to in-memory logs
                int take = Math.Min(count, inMemoryLogs.Count);
            }
            return inMemoryLogs.GetRange(inMemoryLogs.Count - take, take);
        }

        public string GetActivityLogDisplay(int count = 10)
        {
            var logs = GetRecentLogs(count);
            if (logs.Count == 0)
                return "📭 No activity logs found.";

            string display = "📋 **Recent Activity Log:**\n\n";
            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                display += $"{i + 1}. [{log.Timestamp:HH:mm}] **{log.Action}**";
                if (!string.IsNullOrEmpty(log.Description))
                    display += $" - {log.Description}";
                display += "\n";
            }
            return display;
        }

    }
}
