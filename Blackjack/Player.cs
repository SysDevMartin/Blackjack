using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blackjack;

public class Player
{
    public PlayerProperties Properties;

    public enum Choice
    {
        Hit,
        Stand,
        EndGame,
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
            Properties.CurrentCards,
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
    
    public Choice PromptChoice()
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
                return Choice.Hit;
            case "[bold][gold3_1]Stand[/][/] [dim]- Stay at current card sum.[/]":
                return Choice.Stand;
            case "[bold][red]Quit[/][/]":
                return Choice.EndGame;
        }

        return Choice.None;
    }
}