
using ConsoleApp3;

class Program
{
    static void Main()
    {
        UI UI = new UI();
        UI.ShowBanner();
        UI.Divider();

        string name = UI.GetUserName();
        UI.GreetUser(name);

        Chatbot.HandleUserInput(name);
    }
}