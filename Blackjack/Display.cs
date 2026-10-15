using Spectre.Console;

namespace Blackjack;

public class Display
{
    public static void DisplayCardPanel(
        List<int> cardList,
        Color panelDefaultBorderColor,
        Color panelHighlightBorderColor,
        Color cardBorderColor,
        string cardTextColorName,
        string playerName,
        string playerPhrase,
        int panelWidth,
        bool isStanding,
        bool revealCards
        )
    {
        AnsiConsole.WriteLine("");
        
        var grid = new Grid();
        grid.AddColumns(cardList.Count);
        
        List<Panel> cardPanelList = new();
        
        foreach (var card in cardList)
        {
            var cardText = revealCards ? $"[bold][{cardTextColorName}]{card.ToString()}[/][/]" : $"[bold][{cardTextColorName}]?[/][/]";
            var cardPanel = new Panel(cardText).BorderColor(cardBorderColor);
            cardPanelList.Add(cardPanel);
        }
        grid.AddRow(cardPanelList.ToArray());

        var nameText = isStanding ? "STANDING" : playerName;
        var panelColor = isStanding ? panelHighlightBorderColor : panelDefaultBorderColor;
        
        var headerText = revealCards ? $"{nameText} [{cardList.Sum()}]" : $"{nameText} - {playerPhrase}";
        var escapedHeaderMarkup = Markup.Escape(headerText);
        
        var panel = new Panel(grid)
            {
                Width = panelWidth
            }
            .Header(escapedHeaderMarkup)
            .BorderColor(panelColor);
        
        AnsiConsole.Write(panel);
    }
}