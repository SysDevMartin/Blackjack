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
            var cardText = $"[bold][{cardTextColorName}]{card.ToString()}[/][/]";
            var cardPanel = new Panel(cardText).BorderColor(cardBorderColor);
            cardPanelList.Add(cardPanel);
        }
        grid.AddRow(cardPanelList.ToArray());

        var nameText = isStanding ? "STANDING" : playerName;
        var panelColor = isStanding ? panelHighlightBorderColor : panelDefaultBorderColor;
        
        var headerText = $"{nameText} [{cardList.Sum()}]";
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