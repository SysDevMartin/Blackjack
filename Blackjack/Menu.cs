using Spectre.Console;

namespace Blackjack;

public static class Menu
{
    public enum MenuChoice
    {
        PlayAgain,
        QuitGame,
        None
    }
    
    public static MenuChoice PromptGameChoice()
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
}