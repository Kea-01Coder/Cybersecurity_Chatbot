
using ConsoleApp3;
using System;

class Program
{
    static void Main()
    {
        UI.ShowBanner();

        string name = UI.GetUserName();
        UI.GreetUser(name); 
        Chatbot.StartChat(name);
    }
}