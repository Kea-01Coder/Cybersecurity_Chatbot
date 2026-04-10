
using ConsoleApp3;
using System;

class Program
{
    static void Main()
    {
        UI Interface = new UI();
        Interface.ShowBanner();
        Interface.Divider();

        string name = Interface.GetUserName();
        Interface.GreetUser(name);

        Chatbot.HandleUserInput(name);
    }
}