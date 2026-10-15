namespace Blackjack;

public class PlayerProperties
{
    public readonly List<int> Cards = [];
    public bool IsStanding { get; set; }
    public int Score { get; private set; }
    
    private readonly Random _randomNumberGenerator = new();

    public PlayerProperties()
    {
        ResetState();
    }

    public void ResetState()
    {
        Cards.Clear();
        IsStanding = false;
    }

    public void AddRandomCard()
    {
        Cards.Add(GetRandomCard());
    }

    public void AddScore()
    {
        Score++;
    }
    
    private int GetRandomCard()
    {
        // Use MaxCardValue + 1 to account for the fact that the
        // random function starts at value 0, not 1.
        return _randomNumberGenerator.Next(
            Game.Settings.MinCardValue,
            Game.Settings.MaxCardValue + 1
        );
    }
}