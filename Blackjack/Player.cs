namespace Blackjack;

public class Player
{
    public List<int> CurrentCards = new();
    
    public Player()
    {
        
    }

    public void DisplayHand()
    {
        Console.WriteLine("----------");
        var cardString = "";
        foreach (var card in CurrentCards)
        {
            cardString += $"[{card}] ";
        }
        Console.WriteLine(cardString.Trim());
        Console.WriteLine("----------");
    }
}