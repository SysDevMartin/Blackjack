using System.Formats.Asn1;
using Spectre.Console;

namespace Blackjack;

class Program
{
    public static GameState CurrentGameState;
    public static Turn CurrentTurn;
    
    public enum GameState
    {
        Play,
        Stand,
        Quit
    }

    public enum Turn
    {
        Player,
        Dealer
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
        var randomNumberGenerator = new Random();
        
        void InitializeGame()
        {
            CurrentGameState = GameState.Play;
            CurrentTurn = Turn.Player;

            dealer.Reset();
            player.Reset();
        
            // Deal a starting hand to the dealer.
            for (int i = 0; i < Game.Settings.StartingCardCount; i++)
            {
                dealer.DealCardToSelf();
            }
        
            // Deal a starting hand to the player.
            for (int i = 0; i < Game.Settings.StartingCardCount; i++)
            {
                dealer.DealCardToPlayer(player);
            }
        }
        
        void EndGame()
        {
            AnsiConsole.WriteLine("");
            if (AnsiConsole.Confirm("Quit game? All progress will be lost."))
            {
                CurrentGameState = GameState.Quit;
            }
        }

        void DisplayGameTitlePanel()
        {
            var figlet = new FigletText("BLACKJACK").Color(Color.DarkSeaGreen4_1);
            AnsiConsole.Write(figlet);
            AnsiConsole.MarkupLine("  [darkseagreen4]Get the sum of your cards as close as possible to [bold]21[/] without going over.[/]\n");
        }

        void PerformPlayerTurn(ref string gameMessageMarkupText)
        {
            var playerChoice = player.PromptChoice();
            switch (playerChoice)
            {
                case Player.Choice.Hit:
                    if (player.CurrentCards.Count < Game.Settings.MaxCardCount)
                    {
                        player.IsStanding = false;
                        dealer.DealCardToPlayer(player);
                        CurrentTurn = Turn.Dealer;
                    }
                    else
                    {
                        gameMessageMarkupText = "[red]You cannot ask for more cards. You have reached the limit.[/]";
                    }
                    break;
                case Player.Choice.Stand:
                    player.IsStanding = true;
                    CurrentTurn = Turn.Dealer;
                    break;
                case Player.Choice.EndGame:
                    EndGame();
                    break;
            }
        }

        void PerformDealerTurn()
        {
            AnsiConsole.Status()
                .Start("\nDealer's turn. Making a decision...", ctx =>
                {
                    var minRandomSleepTime = 3000;
                    var maxRandomSleepTime = 5000;
                    var randomSleepTime = randomNumberGenerator.Next(minRandomSleepTime, maxRandomSleepTime + 1);
                    Thread.Sleep(randomSleepTime);
                    dealer.MakeTurnDecision();
                    CurrentTurn = Turn.Player;
                });
        }
        
        MenuChoice PromptMenuChoice()
        {
            AnsiConsole.WriteLine("");
            
            string[] availableChoices =
            {
                "[bold][chartreuse3]Play Again[/][/]",
                "[bold][red]Quit[/][/]"
            };
            
            var selectedMenuItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Play again?[/]")
                    .AddChoices(availableChoices));
            
            switch (selectedMenuItem)
            {
                case "[bold][chartreuse3]Play Again[/][/]":
                    return MenuChoice.PlayAgain;
                case "[bold][red]Quit[/][/]":
                    return MenuChoice.QuitGame;
            }

            return MenuChoice.None;
        }
        
        InitializeGame();

        var gameMessageMarkupText = "";
        
        while (CurrentGameState != GameState.Quit)
        {
            var revealDealerCards = player.IsStanding && dealer.IsStanding;
            AnsiConsole.Clear();
            DisplayGameTitlePanel();
            
            if (player.Score > 0 || dealer.Score > 0)
            {
                var scoreTable = new Table().BorderColor(Color.Gray);
                scoreTable.AddColumn("[white]Player[/]");
                scoreTable.AddColumn("[white]Wins[/]");
                scoreTable.AddRow("[darkorange]Dealer[/]", $"[darkorange]{dealer.Score.ToString()}[/]");
                scoreTable.AddRow("[cornflowerblue]You[/]", $"[cornflowerblue]{player.Score.ToString()}[/]");
                AnsiConsole.Write(scoreTable);
            }
            
            dealer.DisplayPanel(revealDealerCards);
            player.DisplayPanel();
            
            if (gameMessageMarkupText.Length > 0)
            {
                AnsiConsole.MarkupLine(gameMessageMarkupText);
                gameMessageMarkupText = "";
            }

            if (player.IsStanding && dealer.IsStanding)
            {
                // Both the player and the dealer are standing.
                CurrentGameState = GameState.Stand;
                var playerCardSum = player.CurrentCards.Sum();
                var dealerCardSum = dealer.CurrentCards.Sum();
                var playerDealerSumDifference = playerCardSum - dealerCardSum;
                var isPlayerOverTargetSum = playerCardSum > Game.Settings.TargetCardSum;
                var isDealerOverTargetSum = dealerCardSum > Game.Settings.TargetCardSum;
                var playerWins = false;
                var dealerWins = false;
                
                AnsiConsole.WriteLine("");
                
                if (playerDealerSumDifference == 0)
                {
                    playerWins = false;
                    dealerWins = false;
                }
                else if (isPlayerOverTargetSum && isDealerOverTargetSum)
                {
                    if (playerCardSum < dealerCardSum)
                    {
                        playerWins = true;
                    }
                    else
                    {
                        dealerWins = true;
                    }
                }
                else if (isDealerOverTargetSum && !isPlayerOverTargetSum)
                {
                    playerWins = true;
                }
                else if (isPlayerOverTargetSum && !isDealerOverTargetSum)
                {
                    dealerWins = true;
                }
                else if (playerDealerSumDifference > 0)
                {
                    playerWins = true;
                }
                else
                {
                    dealerWins = true;
                }
                
                if (playerWins)
                {
                    player.Score++;
                    AnsiConsole.MarkupLine("[bold][green]You win![/][/]");
                }
                else if (dealerWins)
                {
                    dealer.Score++;
                    AnsiConsole.MarkupLine("[bold][red]Dealer wins![/][/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold][yellow]Draw![/][/]");
                }
                
                AnsiConsole.MarkupLine($"[darkgoldenrod]You have [bold]{playerCardSum}[/] and dealer has [bold]{dealerCardSum}[/][/].");
            }

            // Active game.
            switch (CurrentGameState)
            {
                case GameState.Play:
                    switch (CurrentTurn)
                    {
                        case Turn.Player:
                            PerformPlayerTurn(ref gameMessageMarkupText);
                            break;
                        case Turn.Dealer:
                            PerformDealerTurn();
                            break;
                    }
                    break;
                case GameState.Stand:
                    var menuChoice = PromptMenuChoice();
                    switch (menuChoice)
                    {
                        case MenuChoice.PlayAgain:
                            InitializeGame();
                            break;
                        case MenuChoice.QuitGame:
                            CurrentGameState = GameState.Quit;
                            break;
                    }
                    break;
            }
        }
    }
}