using Spectre.Console;

namespace Blackjack;

public class Player
{
    public PlayerProperties Properties;

    public enum ActionChoice
    {
        Hit,
        Stand,
        Quit,
        None
    }

    public Player()
    {
        Properties = new();
    }

    public void DisplayCardPanel()
    {
        var panelDefaultBorderColor = Color.DeepSkyBlue1;
        var panelHighlightBorderColor = Color.Aqua;
        var cardBorderColor = Color.DarkSlateGray3;
        var cardTextColorName = "lightskyblue1";
        var playerName = "YOU";
        var playerPhrase = "";
        var revealCards = true;
        
        Display.DisplayCardPanel(
            Properties.Cards,
            panelDefaultBorderColor,
            panelHighlightBorderColor,
            cardBorderColor,
            cardTextColorName,
            playerName,
            playerPhrase,
            Game.Settings.GamePanelWidth,
            Properties.IsStanding,
            revealCards
        );
    }
    
    public ActionChoice PromptActionChoice()
    {
        AnsiConsole.WriteLine("");
        
        string[] availableChoices =
        {
            "[bold][chartreuse3]Hit[/][/] [dim]- Get a card from the dealer.[/]",
            "[bold][gold3_1]Stand[/][/] [dim]- Stay at current card sum.[/]",
            "[bold][red]Quit[/][/]"
        };
        
        var selectedMenuItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Your turn. What's your decision?[/]")
                .AddChoices(availableChoices));
            
        switch (selectedMenuItem)
        {
            case "[bold][chartreuse3]Hit[/][/] [dim]- Get a card from the dealer.[/]":
                return ActionChoice.Hit;
            case "[bold][gold3_1]Stand[/][/] [dim]- Stay at current card sum.[/]":
                return ActionChoice.Stand;
            case "[bold][red]Quit[/][/]":
                return ActionChoice.Quit;
        }

        return ActionChoice.None;
    }
    
    public string PerformTurn(Game game, Dealer dealer)
    {
        var actionMessage = String.Empty;
        var playerChoice = PromptActionChoice();
        
        switch (playerChoice)
        {
            case ActionChoice.Hit:
                if (Properties.Cards.Count < Game.Settings.MaxCardCount)
                {
                    Properties.IsStanding = false;
                    dealer.DealCardToPlayer(this);
                    Game.CurrentTurn = Game.Turn.Dealer;
                }
                else
                {
                    actionMessage = "[red]You cannot ask for more cards. You have reached the limit.[/]";
                }
                break;
            case ActionChoice.Stand:
                Properties.IsStanding = true;
                Game.CurrentTurn = Game.Turn.Dealer;
                break;
            case ActionChoice.Quit:
                game.EndGame();
                break;
        }

        return actionMessage;
    }
}