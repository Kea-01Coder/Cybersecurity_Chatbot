using System;
using System.Collections.Generic;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public Brush SenderColor { get; set; }
        public string Timestamp { get; set; }

        public ChatMessage()
        {
            Timestamp = DateTime.Now.ToString("HH:mm");
        }

    }
}
