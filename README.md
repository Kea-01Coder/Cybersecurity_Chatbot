# Cybersecurity_Chatbot

## Project Completion Date
14 April 2026

## Project Purpose
The purpose of this project is to develop and demonstrate a **Cybersecurity Awareness Chatbot Assistant** while applying **Git**, **GitHub**, and **GitHub CLI (gh)** in a real-world version control workflow.

The chatbot is designed to provide users with basic cybersecurity awareness information, helping them understand safe online practices, common threats, and how to protect themselves from cyber risks such as phishing, weak passwords, scamming, malware and safe browsing.

In addition to the chatbot functionality, this project also serves as a **CLI workshop exercise**, focusing on proper source control management and GitHub integration.

## Chatbot Purpose (Cybersecurity Focus)

The chatbot assistant aims to:
- Educate users about basic cybersecurity threats
- Promote safe password practices
- Raise awareness about phishing and online scams
- Encourage safe browsing habits
- Provide simple, beginner-friendly cybersecurity guidance

## Tools and Technologies Used

- Git (version control system)
- GitHub (remote repository hosting)
- GitHub CLI (`gh`)
- Visual Studio Code (development environment)
- Command Line / Terminal
- .NET / C#

## Project Workflow
### 1. Repository Setup
The project repository was cloned from GitHub to the local machine:

### 2. Project Classes
Consists of four classes
- UI.cs : User interface classes with multiple methods done for capturing user attention, ASCII art, text-to-speech, font color          change, personalised welcome voice
- Chatbot.cs: Filled with response based methods, makes use of control statements such as if-statements and switch cases for user input and also input validation. Also consists of submenus to give the user more options and more information
- Speech.cs: Done mainly for text-to-speech
- Program.cs: Contains main class, that is done to call the other classes and also display. 

 ---

# PART 2
## Project Overview
I Created a project using the same code for part 1, but i now added a GUI component, that makes the user experience even
better. There are 2 project folders shown within the github folder, the part 2 folder is the Cybersecurity_ChatbotWPF folder.

### Development Timeline
| Class/Component                   | Date Created | Last Modified | Description                              |
|-----------------------------------|--------------|---------------|------------------------------------------|
| **MainWindow.xaml**               | 25 May 2026  | 29 May 2026   | GUI layout and styling                   |
| **MainWindow.xaml.cs**            | 25 May 2026  | 29 May 2026   | Main UI event handlers and orchestration |
| **Models/ChatMessage.cs**         | 26 May 2026  | 27 May 2026   | Chat message data model                  |
| **Services/KeywordRecognizer.cs** | 23 May 2026  | 28 May 2026   | Keyword detection logic                  |
| **Services/SentimentAnalyzer.cs** | 25 May 2026  | 28 May 2026   | Sentiment detection logic                |
| **Services/ResponseManager.cs**   | 24 May 2026  | 28 May 2026   | Random response management               |
| **Services/MemoryManager.cs**     | 23 May 2026  | 28 May 2026   | User memory and recall                   |
| **Services/SpeechService.cs**     | 21 May 2026  | 29 May 2026   | Text-to-speech & voice recognition       |


---

## How the Program Works

### **1. Application Startup**
- User launches the application
- ASCII art banner displays in the header
- Bot welcomes user and asks for their name
- Text-to-speech speaks the welcome message

### **2. Intent Detection (in order)**
| Priority | Detection Type | Keywords/Patterns                  |
|----------|----------------|------------------------------------|
| 1st      | Exit commands  | "exit", "quit", "bye"              |
| 2nd      | User name      | First time user input              |
| 3rd      | Follow-up      | "tell me more", "another tip"      |
| 4th      | Sentiment      | "worried", "curious", "frustrated" |
| 5th      | Keywords       | "password", "scam", "privacy"      |
| 6th      | Topics         | "phishing", "help"                 |
| 7th      | Default        | Unknown input handling             |

### **3. Memory System**
- Stores user's name after first interaction
- Remembers favorite cybersecurity topic
- Personalizes responses based on stored preferences
- Example: *"As someone interested in privacy, you might want to review your security settings"*

### **4. Sentiment Detection**
| Sentiment | Trigger Words                  | Bot Response              |
|-----------|--------------------------------|---------------------------|
| Worried   | "worried", "scared", "nervous" | Empathetic + helpful tip  |
| Curious   | "curious", "interested"        | Encouraging + educational |
| Frustrated| "frustrated", "confusing"      | Supportive + simplified   |

### **6. Voice Features**
| Feature            | How it works                                              |
|--------------------|-----------------------------------------------------------|
| Text-to-Speech     | All bot responses are spoken aloud                        |
| Voice Input        | Click the speach button and speak keywords                |
| Speech Recognition | Recognizes: password, phishing, scam, privacy, help, exit |

---

## Class Descriptions

### **Models/ChatMessage.cs**
Stores individual chat messages with sender, message content, color, and timestamp.

### **Services/KeywordRecognizer.cs**
- **Methods:** `GetKeywordResponse()`, `GetKeywordTopic()`
- **Keywords:** "password", "scam", "privacy"
- **Purpose:** Detects cybersecurity topics and returns relevant advice

### **Services/SentimentAnalyzer.cs**
- **Methods:** `DetectSentiment()`, `GetSentimentResponse()`
- **Sentiments:** Worried, curious, frustrated
- **Purpose:** Adjusts bot responses based on user's emotional state

### **Services/ResponseManager.cs**
- **Methods:** `GetRandomPhishingTip()`, `GetRandomGeneralTip()`, `GetFollowUpResponse()`
- **Features:** Random response selection from arrays/lists
- **Purpose:** Provides varied, engaging responses for common topics

### **Services/MemoryManager.cs**
- **Methods:** `SetUserName()`, `GetUserName()`, `RememberFavoriteTopic()`, `GetPersonalizedGreeting()`
- **Storage:** User name, favorite topic, conversation history
- **Purpose:** Personalizes conversation and recalls user information

### **Services/SpeechService.cs**
- **Methods:** `Speak()`, `StartListening()`, `StopListening()`, `IsSpeechRecognitionAvailable()`
- **Features:** Text-to-speech synthesis, voice command recognition
- **Purpose:** Enables voice interaction with the chatbot

### **MainWindow.xaml.cs**
- **Role:** Orchestrates all services and handles UI events
- **Key Methods:** `ProcessInput()`, `AddMessage()`, `Welcome()`
- **Integration:** Connects GUI with all service classes

---
