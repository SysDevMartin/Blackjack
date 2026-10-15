using Spectre.Console;

namespace Blackjack;

public class Card
{
    private const int AceCardValue = 1;
    private readonly int _originalValue;
    private int RealValue { get; set; }

    public Card(int value, int cardSum)
    {
        _originalValue = value;
        RealValue = value;

        if (_originalValue == AceCardValue)
        {
            // It's an ace!
            SetAceValue(cardSum);
        }
    }

    public void SetAceValue(int cardSum)
    {
        const int lowAceValue = 1;
        const int highAceValue = 11;
        
        if (_originalValue == AceCardValue)
        {
            if (cardSum + highAceValue <= Game.Settings.TargetCardSum)
            {
                RealValue = highAceValue;
            }
            else
            {
                RealValue = lowAceValue;
            }
        }
    }

    public Panel GetCardPanel(Color borderColor, string valueColorName, bool revealCard)
    {
        var cardText = revealCard ? $"[bold][{valueColorName}]{RealValue.ToString()}[/][/]" : $"[bold][{valueColorName}]?[/][/]";
        var cardPanel = new Panel(cardText).BorderColor(borderColor);
        return cardPanel;
    }
}