namespace Blackjack;

public class Hand
{
    public readonly List<Card> Cards;
    
    private readonly Random _randomNumberGenerator = new();

    public Hand()
    {
        Cards = new List<Card>();
    }

    public void Reset()
    {
        Cards.Clear();
    }

    public int GetEffectiveCardSum()
    {
        var sum = 0;
        
        foreach (var card in Cards)
        {
            if (card.IsAce)
            {
                card.UpdateAceValue(sum);
            }
            
            sum += card.EffectiveValue;
        }

        return sum;
    }

    public void AddRandomCard()
    {
        Cards.Add(GetRandomCard());
        RecalculateCardValues();
    }

    private void RecalculateCardValues()
    {
        foreach (var card in Cards)
        {
            if (card.IsAce)
            {
                card.UpdateAceValue(GetEffectiveCardSum());
            }
        }
    }

    private int GetTotalCardSum()
    {
        var sum = 0;
        foreach (var card in Cards)
        {
            sum += card.EffectiveValue;
        }
        return sum;
    }
    
    private Card GetRandomCard()
    {
        // Use MaxCardValue + 1 to account for the fact that the
        // random function starts at value 0, not 1.
        var randomNumber = _randomNumberGenerator.Next(
            Game.Settings.MinCardValue,
            Game.Settings.MaxCardValue + 1
        );
        
        return new Card(randomNumber, GetTotalCardSum());
    }
}