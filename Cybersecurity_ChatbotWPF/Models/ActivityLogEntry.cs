using System;

namespace Cybersecurity_ChatbotWPF.Models
{ 
public class ActivityLogEntry
    {
      public int Id { get; set; }
      public string Action { get; set; }
      public string Description { get; set; }
      public DateTime Timestamp { get; set; }
      public string Category { get; set; }

      public override string ToString()
      {
        return $"[{Timestamp:yyyy-MM-dd HH:mm}] {Action} - {Description}";
      }
    }
}
