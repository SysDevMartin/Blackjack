using Spectre.Console;

namespace Blackjack;

public static class Menu
{
    public enum MenuChoice
    {
        Continue,
        None
    }
    
    public static MenuChoice PromptGameChoice()
    {
        AnsiConsole.WriteLine("");
            
        string[] availableChoices =
        {
            "[bold][chartreuse3]Continue[/][/]"
        };
            
        var selectedMenuItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(availableChoices));
            
        switch (selectedMenuItem)
        {
            case "[bold][chartreuse3]Continue[/][/]":
                return MenuChoice.Continue;
        }

        return MenuChoice.None;
    }
}