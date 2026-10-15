using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blackjack;

public class Player
{
    public readonly List<int> CurrentCards = [];
    public bool IsStanding { get; set; }
    public int Score = 0;

    public enum Choice
    {
        Hit,
        Stand,
        EndGame,
        None
    }

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        IsStanding = false;
        CurrentCards.Clear();
    }

    private void GetCardOutput(int number)
    {
        
    }

    public void DisplayCardPanel()
    {
        var panelDefaultBorderColor = Color.DeepSkyBlue1;
        var panelHighlightBorderColor = Color.Aqua;
        var cardBorderColor = Color.DarkSlateGray3;
        var cardTextColorName = "lightskyblue1";
        var playerName = "YOU";
        var revealCards = true;
        
        Display.DisplayCardPanel(
            CurrentCards,
            panelDefaultBorderColor,
            panelHighlightBorderColor,
            cardBorderColor,
            cardTextColorName,
            playerName,
            Game.Settings.GamePanelWidth,
            IsStanding,
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