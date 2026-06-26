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

# CyberGuardian AI - Part 3: Advanced Features

## Project Overview

**CyberGuardian AI** is a cybersecurity awareness chatbot with advanced features including task management, cybersecurity quizzes, NLP simulation, and activity logging. This is **Part 3** of the PROG6221 POE, building upon the console application (Part 1) and WPF GUI (Part 2).

**Language:** C#  
**Framework:** .NET 10.0 / WPF  
**Architecture:** MVVM Pattern with Service Layer  
**Database:** SQLite (local, self-contained)  

---

## Part 3 Development Timeline

| Date         | Component                  | Description                           |
|--------------|----------------------------|---------------------------------------|
| 15 June 2026 | Models/TaskItem.cs         | Task data model with reminders        |
| 15 June 2026 | Models/QuizQuestion.cs     | Quiz question data model              |
| 15 June 2026 | Models/ActivityLogEntry.cs | Activity log data model               |
| 15 June 2026 | Services/DatabaseHelper.cs | SQLite database operations            |
| 16 June 2026 | Services/TaskManager.cs    | Task management logic                 |
| 16 June 2026 | Services/QuizManager.cs    | Quiz logic with 12+ questions         |
| 17 June 2026 | Services/NLPSimulator.cs   | NLP simulation with keyword detection |
| 17 June 2026 | Services/ActivityLogger.cs | Activity logging with in-memory + DB  |
| 18 June 2026 | QuizWindow.xaml            | Quiz GUI layout                       |
| 18 June 2026 | QuizWindow.xaml.cs         | Quiz UI logic                         |
| 20 June 2026 | Views/Converters/          | WPF value converters                  |
| 25 June 2026 | Database integration       | SQLite database fully integrated      |    
| 26 June 2026 | Testing & Bug Fixes        | All features working                  |

---

## Part 3 Features

### Task 1: Task Assistant with Reminders
- **Add tasks** with title and description
- **Set reminders** with specific timeframes (e.g., "remind me in 3 days")
- **View all tasks** with status (pending/completed)
- **Complete tasks** - mark as done
- **Delete tasks** - remove from database
- **Database storage** - SQLite persistence

### Task 2: Cybersecurity Mini-Game (Quiz)
- **12+ cybersecurity questions** covering phishing, passwords, privacy, malware
- **Mixed question types** - Multiple Choice and True/False
- **Immediate feedback** with explanations for each answer
- **Score tracking** - correct answers counted
- **Final feedback** based on percentage score
- **Dedicated Quiz Window** with progress bar

### Task 3: NLP Simulation
- **Intent recognition** using keyword detection
- **Flexible phrasing** support (e.g., "Add task", "Create task", "New task")
- **Regex pattern matching** for robust detection
- **Extract task info** from natural language
- **Extract reminder days** from phrases like "in 3 days"
- **Extract quiz answers** from numeric input

### Task 4: Activity Log Feature
- **Automatic logging** of all actions
- **Log types**: Tasks (add/complete/delete), Quiz (start/complete), NLP interactions, System events
- **In-memory storage** with database backup
- **View recent logs** with "Show activity log" command
- **Timestamp tracking** for each action
- **Fallback** if database is unavailable

---

## How to Navigate the Application

### 1. **Getting Started**

| Action            | Steps                                       |
|-------------------|---------------------------------------------|
| **Launch App**    | Press F5 or run the .exe file               |
| **Welcome**       | Bot asks for your name                      |
| **Set Name**      | Type your name and press Enter              |
| **Voice Welcome** | Custom voice recording plays (if available) |

### 2. **Quick Tip Buttons (Part 3 Additions)**

| Button    | Function                 | Example Response                     |
|-----------|--------------------------|--------------------------------------|
| **Tasks** | View all current tasks   | Shows list with status and reminders |
| **Quiz**  | Start cybersecurity quiz | Opens QuizWindow with 12+ questions  |

### 3. **Task Commands**

| Command                                      | Example                                                     | Function                  |
|----------------------------------------------|-------------------------------------------------------------|---------------------------|
| `Add task: [title]`                          | "Add task: Enable 2FA"                                      | Creates new task          |
| `Add task with reminder in X days: [title]`  | "Add task with reminder in 7 days: Review privacy settings" |Creates task with reminder |
| `List tasks`                                 | "List tasks"                                                | Shows all tasks           |
| `Complete task: [title]`                     | "Complete task: Enable 2FA"                                 | Marks task as completed   |
| `Delete task: [title]`                       | "Delete task: Enable 2FA"                                   | Deletes a task            |

### 4. **Activity Log Commands**

| Command             | Function             |
|---------------------|----------------------|
| `Show activity log` | Shows recent actions |
| `Show log`          | Shows recent actions |

### 5. **NLP Examples (Flexible Phrasing)**

| User Input                                      | Intent Recognized      |
|-------------------------------------------------|------------------------|
| "Add task to enable 2FA"                        | Add Task               |
| "Create new task: Update passwords"             | Add Task               |
| "I need to remember to review privacy settings" | Add Task               |
| "Remind me to check my accounts in 5 days"      | Add Task with Reminder |
| "Show me my tasks"                              | List Tasks             |
| "What tasks do I have?"                         | List Tasks             |
| "Mark Enable 2FA as done"                       | Complete Task          |
| "Delete the Enable 2FA task"                    | Delete Task            |
| "Let's play the quiz"                           | Start Quiz             |
| "I want to test my knowledge"                   | Start Quiz             |
| "Show activity log"                             | Show Log               |

---

## Part 3 Demo Script
1.Launch app → Database auto-creates
2.Type "Add task: Enable 2FA" → Task saved to database
3.Type "Add task with reminder in 7 days: Review privacy settings" → Task with reminder
4.Type "List tasks" → Shows all tasks with status and reminders
5.Type "Complete task: Enable 2FA" → Task marked completed
6.Click Quiz button → Opens dedicated QuizWindow
7.Type "Show activity log" → Shows recent actions
8.Type "help" → Shows all available commands



