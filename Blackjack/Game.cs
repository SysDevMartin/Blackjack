using Spectre.Console;

namespace Blackjack;

public class Game
{
    public struct Settings
    {
        public static int GamePanelWidth { get; } = 75;
        public static int TargetCardSum { get; } = 21;
        public static int StartingCardCount { get; } = 2;
        public static int MaxCardCount { get; } = 10;
        public static int MinCardValue { get; } = 1;
        public static int MaxCardValue { get; } = 10;
    }
    
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
    
    public static GameState CurrentGameState;
    public static Turn CurrentTurn;

    private void InitializeGame(Player player, Dealer dealer)
    {
        CurrentGameState = GameState.Play;
        CurrentTurn = Turn.Player;

        dealer.Properties.ResetState();
        player.Properties.ResetState();
        
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
    }
    
    public void Run()
    {
        Dealer dealer = new();
        Player player = new();
        var randomNumberGenerator = new Random();
        
        InitializeGame(player, dealer);
        
        void EndGame()
        {
            AnsiConsole.WriteLine("");
            if (AnsiConsole.Confirm("[bold]Quit the game?[/] [gray50]All progress will be lost.[/]"))
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
            var playerChoice = player.PromptActionChoice();
            switch (playerChoice)
            {
                case Player.ActionChoice.Hit:
                    if (player.Properties.Cards.Count < Settings.MaxCardCount)
                    {
                        player.Properties.IsStanding = false;
                        dealer.DealCardToPlayer(player);
                        CurrentTurn = Turn.Dealer;
                    }
                    else
                    {
                        gameMessageMarkupText = "[red]You cannot ask for more cards. You have reached the limit.[/]";
                    }
                    break;
                case Player.ActionChoice.Stand:
                    player.Properties.IsStanding = true;
                    CurrentTurn = Turn.Dealer;
                    break;
                case Player.ActionChoice.Quit:
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
        
        InitializeGame(player, dealer);

        var gameMessageMarkupText = "";
        
        while (CurrentGameState != GameState.Quit)
        {
            var revealDealerCards = player.Properties.IsStanding && dealer.Properties.IsStanding;
            AnsiConsole.Clear();
            DisplayGameTitlePanel();
            
            if (player.Properties.Score > 0 || dealer.Properties.Score > 0)
            {
                var scoreTable = new Table().BorderColor(Color.Gray);
                scoreTable.AddColumn("[white]Player[/]");
                scoreTable.AddColumn("[white]Wins[/]");
                scoreTable.AddRow("[darkorange]Dealer[/]", $"[darkorange]{dealer.Properties.Score.ToString()}[/]");
                scoreTable.AddRow("[cornflowerblue]You[/]", $"[cornflowerblue]{player.Properties.Score.ToString()}[/]");
                AnsiConsole.Write(scoreTable);
            }
            
            dealer.DisplayCardPanel(revealDealerCards);
            player.DisplayCardPanel();
            
            if (gameMessageMarkupText.Length > 0)
            {
                AnsiConsole.MarkupLine(gameMessageMarkupText);
                gameMessageMarkupText = "";
            }

            if (player.Properties.IsStanding && dealer.Properties.IsStanding)
            {
                // Both the player and the dealer are standing.
                CurrentGameState = GameState.Stand;
                var playerCardSum = player.Properties.Cards.Sum();
                var dealerCardSum = dealer.Properties.Cards.Sum();
                var playerDealerSumDifference = playerCardSum - dealerCardSum;
                var isPlayerOverTargetSum = playerCardSum > Settings.TargetCardSum;
                var isDealerOverTargetSum = dealerCardSum > Settings.TargetCardSum;
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
                    player.Properties.AddScore();
                    AnsiConsole.MarkupLine("[bold][green]You win![/][/]");
                }
                else if (dealerWins)
                {
                    dealer.Properties.AddScore();
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
                            InitializeGame(player, dealer);
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