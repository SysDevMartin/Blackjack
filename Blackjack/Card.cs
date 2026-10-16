using Spectre.Console;

namespace Blackjack;

public class Card
{
    private const int AceCardValue = 1;
    
    public int EffectiveValue { get; private set; }
    public bool IsAce { get; private set; }
    
    private readonly int _originalValue;

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
            var highAceStaysBelowOrAtTargetSum = cardSum + highAceValue <= Game.Settings.TargetCardSum;
            EffectiveValue = highAceStaysBelowOrAtTargetSum ? highAceValue : lowAceValue;
            IsAce = true;
        }
    }

    public Panel GetCardPanel(bool revealCard)
    {
        var borderColor = IsAce ? Color.Purple : Color.Gray50;
        var textColorName = IsAce ? "mediumpurple1" : "gray78";
        
        var cardText = revealCard ? $"[bold][{textColorName}]{EffectiveValue.ToString()}[/][/]" : $"[bold][{textColorName}]?[/][/]";
        var cardPanel = new Panel(cardText).BorderColor(borderColor);
        return cardPanel;
    }
}