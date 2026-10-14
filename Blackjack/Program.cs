using System.IO.Compression;
using Spectre.Console;

namespace Blackjack;

class Program
{
    public static bool IsGameActive = true;
    
    public struct Settings
    {
        public static int GamePanelWidth { get; } = 100;
        public static int TargetCardSum { get; } = 21;
        public static int StartingCardCount { get; } = 2;
    }
    
    static void Main(string[] args)
    {
        Dealer dealer = new();
        Player player = new();

        void QuitGame()
        {
            Console.WriteLine("Quit the game? (Y/N)");
            var quitConfirmationPrompt = Console.ReadLine();
            if (quitConfirmationPrompt?.ToUpper() == "Y")
            {
                IsGameActive = false;
            }
        }

        void DisplayGameTitlePanel()
        {
            var titleText = new Markup("[green][bold]BLACKJACK[/][/]\nRules: Get the sum of your cards as close as possible to [bold]21[/] without going over.")
                .Centered();
            var gameTitlePanel = new Panel(titleText)
                {
                    Width = Settings.GamePanelWidth
                }
                .BorderColor(Color.Green);
            AnsiConsole.Write(gameTitlePanel);
        }
        
        // Deal a starting hand to the dealer.
        for (int i = 0; i < Settings.StartingCardCount; i++)
        {
            dealer.DealCardToSelf();
        }
        
        // Deal a starting hand to the player.
        for (int i = 0; i < Settings.StartingCardCount; i++)
        {
            dealer.DealCardToPlayer(player);
        }
        
        while (IsGameActive)
        {
            AnsiConsole.Clear();
            DisplayGameTitlePanel();
            dealer.DisplayPanel();
            player.DisplayPanel();
            
            var playerChoice = player.PromptChoice();
            switch (playerChoice)
            {
                case Player.Choice.Hit:
                    dealer.DealCardToPlayer(player);
                    break;
                case Player.Choice.Stand:
                    // Do nothing.
                    break;
                case Player.Choice.EndGame:
                    QuitGame();
                    break;
            }
        }
    }
}