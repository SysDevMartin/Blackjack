using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blackjack;

public class Player
{
    public readonly List<int> CurrentCards = [];
    public bool IsStanding { get; set; }

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

    public void DisplayPanel()
    {
        AnsiConsole.WriteLine("");
        
        var grid = new Grid();
        grid.AddColumns(CurrentCards.Count);
        List<Panel> cardPanels = new();
        foreach (var card in CurrentCards)
        {
            var cardText = $"[bold][lightskyblue1]{card.ToString()}[/][/]";
            var cardPanel = new Panel(cardText).BorderColor(Color.DeepSkyBlue1);
            cardPanels.Add(cardPanel);
        }
        grid.AddRow(cardPanels.ToArray());
        
        var escapedHeaderMarkup = Markup.Escape($"YOU [{CurrentCards.Sum()}]");
        var panel = new Panel(grid)
            {
                Width = Program.Settings.GamePanelWidth
            }
            .Header(escapedHeaderMarkup)
            .BorderColor(Color.CornflowerBlue);
        
        AnsiConsole.Write(panel);
    }
    
    public Choice PromptChoice()
    {
        AnsiConsole.WriteLine("");
        
        string[] availableChoices =
        {
            "[bold][chartreuse3]Hit[/][/] [dim]- Get a card from the dealer.[/]",
            "[bold][gold3_1]Stand[/][/] [dim]- Stay at current card sum.[/]",
            "[bold][red]Quit Game[/][/]"
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
            case "[bold][red]Quit Game[/][/]":
                return Choice.EndGame;
        }

        return Choice.None;
    }
    
    /*public bool Stand()
    {
        IsStanding = true;
        var currentCardSum = CurrentCards.Sum();
        if (currentCardSum == Program.Settings.TargetCardSum)
        {
            Console.WriteLine("You won!");
            return true;
        }
        else
        {
            Console.WriteLine($"You lost! Your total card sum of {currentCardSum} didn't match the target card sum of {Program.Settings.TargetCardSum}.");
            return false;
        }
    }*/
}