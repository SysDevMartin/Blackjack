using Spectre.Console;

namespace Blackjack;

public class Player
{
    public readonly List<int> CurrentCards = [];

    public enum Choice
    {
        Hit,
        Stand,
        EndGame,
        None
    }

    private void GetCardOutput(int number)
    {
        
    }

    public void DisplayPanel()
    {
        var panelContent = "";
        foreach (var card in CurrentCards)
        {
            panelContent += $"[{card}] ";
        }

        var escapedMarkup = Markup.Escape(panelContent);
        var markup = new Markup(escapedMarkup);
        var panel = new Panel(markup)
        {
            Width = Program.Settings.GamePanelWidth
        }
        .Header("YOU")
        .BorderColor(Color.CornflowerBlue);
        
        AnsiConsole.Write(panel);
    }
    
    public Choice PromptChoice()
    {
        string[] availableChoices =
        {
            "[bold][green]Hit[/][/] [dim]- Get a card from the dealer.[/]",
            "[bold][yellow]Stand[/][/] [dim]- Stay at current card sum.[/]",
            "[bold][red]End Game[/][/]"
        };
            
        var selectedMenuItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Your turn. What's your choice?[/]")
                .AddChoices(availableChoices));
            
        switch (selectedMenuItem)
        {
            case "[bold][green]Hit[/][/] [dim]- Get a card from the dealer.[/]":
                return Choice.Hit;
            case "[bold][yellow]Stand[/][/] [dim]- Stay at current card sum.[/]":
                Stand();
                return Choice.Stand;
            case "[bold][red]End Game[/][/]":
                return Choice.EndGame;
        }

        return Choice.None;
    }
    
    public void Stand()
    {
        var currentCardSum = CurrentCards.Sum();
        if (CurrentCards.Sum() == Program.Settings.TargetCardSum)
        {
            Console.WriteLine("You won!");
        }
        else
        {
            Console.WriteLine($"You lost! Your total card sum of {currentCardSum} didn't match the target card sum of {Program.Settings.TargetCardSum}.");
        }
        Program.IsGameActive = false;
    }
}