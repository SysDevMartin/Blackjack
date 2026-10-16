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
        Hand hand,
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
        grid.AddColumns(hand.Cards.Count);

        List<Panel> cardPanelList = new();
        var cardSum = hand.GetEffectiveCardSum();
        var hiddenCardIndex = 1;

        for (int i = 0; i < hand.Cards.Count; i++)
        {
            var card = hand.Cards[i];
            var reveal = (i != hiddenCardIndex) || revealCards;
            
            var cardText = reveal ? $"[bold][{cardTextColorName}]{card.EffectiveValue}[/][/]" : $"[bold][{cardTextColorName}]?[/][/]";
            var cardPanel = new Panel(cardText).BorderColor(cardBorderColor);
            cardPanelList.Add(cardPanel);
        }
        grid.AddRow(cardPanelList.ToArray());

        var nameText = isStanding ? "STANDING" : playerName;
        var panelColor = isStanding ? panelHighlightBorderColor : panelDefaultBorderColor;
        
        var hiddenCardSum = cardSum - hand.Cards[hiddenCardIndex].EffectiveValue;
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