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
    
    public static GameState CurrentGameState;
    public static Turn CurrentTurn;
    
    private void Initialize(Player player, Dealer dealer)
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

    public void End()
    {
        AnsiConsole.WriteLine("");
        if (AnsiConsole.Confirm("[bold]Quit the game?[/] [gray50]All progress will be lost.[/]"))
        {
            CurrentGameState = GameState.Quit;
        }
    }

    private void CheckWinCondition(Player player, Dealer dealer)
    {
        CurrentGameState = GameState.Stand;
        var playerCardSum = player.Properties.Cards.Sum();
        var dealerCardSum = dealer.Properties.Cards.Sum();
        var playerDealerSumDifference = playerCardSum - dealerCardSum;
        var isPlayerOverTargetSum = playerCardSum > Settings.TargetCardSum;
        var isDealerOverTargetSum = dealerCardSum > Settings.TargetCardSum;
        var playerWins = false;
        var dealerWins = false;
        
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
        
        AnsiConsole.WriteLine("");
        
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

    private void PerformGameState(ref string gameMessageMarkupText, Player player, Dealer dealer)
    {
        switch (CurrentGameState)
        {
            case GameState.Play:
                switch (CurrentTurn)
                {
                    case Turn.Player:
                        gameMessageMarkupText = player.PerformTurn(this, dealer);
                        break;
                    case Turn.Dealer:
                        dealer.PerformTurn();
                        break;
                }
                break;
            case GameState.Stand:
                var menuChoice = Menu.PromptGameChoice();
                switch (menuChoice)
                {
                    case Menu.MenuChoice.PlayAgain:
                        Initialize(player, dealer);
                        break;
                    case Menu.MenuChoice.QuitGame:
                        CurrentGameState = GameState.Quit;
                        break;
                }
                break;
        }
    }
    
    public void Run()
    {
        var dealer = new Dealer();
        var player = new Player();
        var gameMessageMarkupText = "";
        
        Initialize(player, dealer);
        
        while (CurrentGameState != GameState.Quit)
        {
            AnsiConsole.Clear();
            Display.DisplayGameLogoPanel();
            
            // Show leaderboard table.
            if (player.Properties.Score > 0 || dealer.Properties.Score > 0)
            {
                var scoreTable = new Table().BorderColor(Color.Gray);
                scoreTable.AddColumn("[white]Player[/]");
                scoreTable.AddColumn("[white]Wins[/]");
                scoreTable.AddRow("[darkorange]Dealer[/]", $"[darkorange]{dealer.Properties.Score.ToString()}[/]");
                scoreTable.AddRow("[cornflowerblue]You[/]", $"[cornflowerblue]{player.Properties.Score.ToString()}[/]");
                AnsiConsole.Write(scoreTable);
            }
            
            var revealDealerCards = player.Properties.IsStanding && dealer.Properties.IsStanding;
            dealer.DisplayCardPanel(revealDealerCards);
            player.DisplayCardPanel();
            
            if (gameMessageMarkupText.Length > 0)
            {
                // Show message (for example when the user has too many cards).
                AnsiConsole.MarkupLine(gameMessageMarkupText);
                gameMessageMarkupText = "";
            }

            if (player.Properties.IsStanding && dealer.Properties.IsStanding)
            {
                CheckWinCondition(player, dealer);
            }

            PerformGameState(ref gameMessageMarkupText, player, dealer);
        }
    }
}