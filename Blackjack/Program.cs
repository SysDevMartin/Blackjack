using System.IO.Compression;
using Spectre.Console;

namespace Blackjack;

class Program
{
    public static bool IsGameActive = true;
    public static bool DoQuitGame = false;
    
    public struct Settings
    {
        public static int GamePanelWidth { get; } = 75;
        public static int TargetCardSum { get; } = 21;
        public static int StartingCardCount { get; } = 2;
    }
    
    private enum MenuChoice
    {
        PlayAgain,
        QuitGame,
        None
    }
    
    static void Main(string[] args)
    {
        Dealer dealer = new();
        Player player = new();
        
        void EndGame()
        {
            AnsiConsole.WriteLine("");
            if (AnsiConsole.Confirm("End the current game? All progress will be lost."))
            {
                IsGameActive = false;
                DoQuitGame = false;
            }
        }

        void DisplayGameTitlePanel()
        {
            var figlet = new FigletText("BLACKJACK").Color(Color.Green);
            AnsiConsole.Write(figlet);
            
            AnsiConsole.MarkupLine("[dim]Get the sum of your cards as close as possible to [bold]21[/] without going over.[/]\n");
        }
        
        MenuChoice PromptMenuChoice()
        {
            AnsiConsole.WriteLine("");
        
            string[] availableChoices =
            {
                "[bold][green]Play Again[/][/]",
                "[bold][red]Quit Game[/][/]"
            };
            
            var selectedMenuItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Play again?[/]")
                    .AddChoices(availableChoices));
            
            switch (selectedMenuItem)
            {
                case "[bold][green]Play Again[/][/]":
                    return MenuChoice.PlayAgain;
                case "[bold][red]Quit Game[/][/]":
                    return MenuChoice.QuitGame;
            }

            return MenuChoice.None;
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
                    EndGame();
                    break;
            }
        }

        if (!DoQuitGame)
        {
            var menuChoice = PromptMenuChoice();
            switch (menuChoice)
            {
                case MenuChoice.PlayAgain:
                    break;
                case MenuChoice.QuitGame:
                    DoQuitGame = true;
                    break;
            }
        }
    }
}