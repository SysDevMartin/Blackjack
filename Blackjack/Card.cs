using Spectre.Console;

namespace Blackjack;

public class Card
{
    private const int AceCardValue = 1;
    private readonly int _originalValue;
    public int EffectiveValue { get; private set; }
    public bool IsAce { get; private set; }

    public Card(int value, int totalCardSum)
    {
        _originalValue = value;
        EffectiveValue = value;

        if (_originalValue == AceCardValue)
        {
            // It's an ace!
            UpdateAceValue(totalCardSum);
            IsAce = true;
        }
    }

    public void UpdateAceValue(int cardSum)
    {
        const int lowAceValue = 1;
        const int highAceValue = 11;
        
        if (_originalValue == AceCardValue)
        {
            if (cardSum + highAceValue <= Game.Settings.TargetCardSum)
            {
                EffectiveValue = highAceValue;
            }
            else
            {
                EffectiveValue = lowAceValue;
            }
            
            IsAce = true;
        }
    }

    public Panel GetCardPanel(Color borderColor, string valueColorName, bool revealCard)
    {
        var cardText = revealCard ? $"[bold][{valueColorName}]{EffectiveValue.ToString()}[/][/]" : $"[bold][{valueColorName}]?[/][/]";
        var cardPanel = new Panel(cardText).BorderColor(borderColor);
        return cardPanel;
    }
}