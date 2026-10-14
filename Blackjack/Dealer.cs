namespace Blackjack;

public class Dealer
{
    private static Random RandomNumberGenerator = new();

    struct Rules
    {
        public static int TargetCardSum { get; } = 21;

        public Rules()
        {
            
        }
    }

    public Dealer()
    {
        
    }
    
    private int GetRandomCard()
    {
        var minNumber = 1;
        var maxNumber = 10;
        return RandomNumberGenerator.Next(minNumber, maxNumber + 1);
    }
    
    public void DealCardToPlayer(Player player)
    {
        var newCard = GetRandomCard();
        player.CurrentCards.Add(newCard);
            
        var currentCardSum = player.CurrentCards.Sum();
        if (currentCardSum > Rules.TargetCardSum)
        {
            Console.WriteLine($"Card sum: {player.CurrentCards.Sum()}");
            Console.WriteLine($"You went over {Rules.TargetCardSum} with your total sum of {currentCardSum} and lost the game!");
            Program.IsGameActive = false;
        }
    }
}