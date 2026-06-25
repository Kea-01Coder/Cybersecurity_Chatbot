using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Cybersecurity_ChatbotWPF.Models;

namespace Cybersecurity_ChatbotWPF.Services
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "chatbot.db");
            connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Ensure directory exists
            string directory = System.IO.Path.GetDirectoryName(connectionString.Replace("Data Source=", ""));
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Tasks Table
                string createTasksTable = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Description TEXT,
                        CreatedDate TEXT NOT NULL,
                        ReminderDate TEXT,
                        IsCompleted INTEGER DEFAULT 0,
                        Category TEXT
                    )";
                ExecuteNonQuery(createTasksTable, connection);

                // Activity Log Table
                string createActivityLogTable = @"
                    CREATE TABLE IF NOT EXISTS ActivityLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Action TEXT NOT NULL,
                        Description TEXT,
                        Timestamp TEXT NOT NULL,
                        Category TEXT
                    )";
                ExecuteNonQuery(createActivityLogTable, connection);

                // Quiz Scores Table
                string createQuizTable = @"
                    CREATE TABLE IF NOT EXISTS QuizScores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName TEXT,
                        Score INTEGER,
                        TotalQuestions INTEGER,
                        DateTaken TEXT
                    )";
                ExecuteNonQuery(createQuizTable, connection);
            }
        }

        private void ExecuteNonQuery(string sql, SqliteConnection connection = null)
        {
            bool closeConnection = connection == null;
            if (connection == null)
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
            }

            using (var command = new SqliteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            if (closeConnection)
                connection.Close();
        }

        // --- TASK OPERATIONS ---

        public int AddTask(TaskItem task)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    INSERT INTO Tasks (Title, Description, CreatedDate, ReminderDate, IsCompleted, Category)
                    VALUES (@Title, @Description, @CreatedDate, @ReminderDate, @IsCompleted, @Category);
                    SELECT last_insert_rowid();";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description ?? "");
                    command.Parameters.AddWithValue("@CreatedDate", task.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@ReminderDate", task.ReminderDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@Category", task.Category ?? "General");

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Tasks ORDER BY IsCompleted ASC, CreatedDate DESC";

                using (var command = new SqliteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            CreatedDate = DateTime.Parse(reader.GetString(3)),
                            ReminderDate = reader.IsDBNull(4) ? (DateTime?)null : DateTime.Parse(reader.GetString(4)),
                            IsCompleted = reader.GetInt32(5) == 1,
                            Category = reader.IsDBNull(6) ? "General" : reader.GetString(6)
                        });
                    }
                }
            }
            return tasks;
        }

        public void DeleteTask(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Tasks WHERE Id = @Id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void MarkTaskCompleted(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // --- ACTIVITY LOG OPERATIONS ---

        public void AddActivityLog(string action, string description, string category = "General")
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    INSERT INTO ActivityLog (Action, Description, Timestamp, Category)
                    VALUES (@Action, @Description, @Timestamp, @Category)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Description", description ?? "");
                    command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Category", category);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ActivityLogEntry> GetRecentActivityLogs(int count = 10)
        {
            var logs = new List<ActivityLogEntry>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ActivityLog ORDER BY Timestamp DESC LIMIT @Count";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Count", count);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(new ActivityLogEntry
                            {
                                Id = reader.GetInt32(0),
                                Action = reader.GetString(1),
                                Description = reader.GetString(2),
                                Timestamp = DateTime.Parse(reader.GetString(3)),
                                Category = reader.IsDBNull(4) ? "General" : reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return logs;
        }

        // --- QUIZ OPERATIONS ---

        public void SaveQuizScore(string userName, int score, int totalQuestions)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    INSERT INTO QuizScores (UserName, Score, TotalQuestions, DateTaken)
                    VALUES (@UserName, @Score, @TotalQuestions, @DateTaken)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName ?? "Anonymous");
                    command.Parameters.AddWithValue("@Score", score);
                    command.Parameters.AddWithValue("@TotalQuestions", totalQuestions);
                    command.Parameters.AddWithValue("@DateTaken", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
