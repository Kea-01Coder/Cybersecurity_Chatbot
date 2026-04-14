
using ConsoleApp3;
using System;

class Program
{
    static void Main()
    {
        UI.ShowBanner();
        UI.PlayWelcome();
        string name = UI.GetUserName();
        UI.GreetUser(name); 
      
        Chatbot.StartChat(name);
    }
}