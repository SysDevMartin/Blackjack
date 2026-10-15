using Spectre.Console;

namespace Blackjack;

public class Display
{
    public static void DisplayGameLogoPanel()
    {
        var figlet = new FigletText("BLACKJACK").Color(Color.DarkSeaGreen4_1);
        AnsiConsole.Write(figlet);
        AnsiConsole.MarkupLine("  [darkseagreen4]Get the sum of your cards as close as possible to [bold]21[/] without going over.[/]\n");
    }
    
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

        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];
            var reveal = (i != 1) || revealCards;
            
            var cardText = reveal ? $"[bold][{cardTextColorName}]{card.ToString()}[/][/]" : $"[bold][{cardTextColorName}]?[/][/]";
            var cardPanel = new Panel(cardText).BorderColor(cardBorderColor);
            cardPanelList.Add(cardPanel);
        }
        grid.AddRow(cardPanelList.ToArray());

        var nameText = isStanding ? "STANDING" : playerName;
        var panelColor = isStanding ? panelHighlightBorderColor : panelDefaultBorderColor;

        var cardSum = cardList.Sum();
        var hiddenCardSum = cardList.Sum() - cardList[1];
        var headerText = revealCards ? $"{nameText} [{cardSum}]" : $"{nameText} [{hiddenCardSum}+?] - {playerPhrase}";
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