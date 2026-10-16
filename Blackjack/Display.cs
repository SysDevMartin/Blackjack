using Spectre.Console;

namespace Blackjack;

public class Display
{
    public static void DisplayGameLogoPanel()
    {
        var figlet = new FigletText("Blackjack Battle").Color(Color.DarkSeaGreen4_1);
        AnsiConsole.Write(figlet);
        AnsiConsole.MarkupLine("  [darkseagreen4]Get the sum of your cards as close as possible to [bold]21[/] without going over.[/]\n");
    }

    public static void DisplayPlayerPanel(
        Hand hand,
        Stats stats,
        string playerName,
        string playerPhrase,
        Color borderColor,
        string textColorName,
        bool revealCards
        )
    {
        var cardPanel = GetCardPanel(hand, revealCards);
        var damageMultiplierText = $"{stats.DamageMultiplier.ToString(".0#")}x";
        var cardSum = hand.GetEffectiveCardSum();
        var hiddenCardSum = cardSum - hand.Cards[1].EffectiveValue;
        var displayedCardSum = revealCards ? $"{cardSum}" : $"{hiddenCardSum} + ?";
        var headerText = playerPhrase.Length > 0 ? $"{playerName} - {playerPhrase}" : playerName;
        
        var rows = new Rows(
            new Markup($"Health: {stats.CurrentHealth} / {stats.MaxHealth}"),
            new Markup($"Damage multiplier: {damageMultiplierText}"),
            new Rule { Style = Style.Parse("gray30") },
            cardPanel,
            new Rule { Style = Style.Parse("gray30") },
            new Markup($"[{textColorName}]Sum: {displayedCardSum}[/]")
            );

        var panel = new Panel(rows)
        {
            Width = Game.Settings.GamePanelWidth
        }
        .Header(headerText)
        .BorderColor(borderColor);
        
        AnsiConsole.Write(panel);
    }

    private static Panel GetCardPanel(Hand hand, bool revealCards)
    {
        var grid = new Grid();
        grid.AddColumns(hand.Cards.Count);

        var cardPanelList = new List<Panel>();
        var hiddenCardIndex = 1;

        for (int i = 0; i < hand.Cards.Count; i++)
        {
            var card = hand.Cards[i];
            var reveal = (i != hiddenCardIndex) || revealCards;
            var cardPanel = card.GetCardPanel(reveal);
            cardPanelList.Add(cardPanel);
        }
        grid.AddRow(cardPanelList.ToArray());

        var panel = new Panel(grid)
        {
            Width = Game.Settings.GamePanelWidth
        }
        .NoBorder();

        return panel;
    }
}