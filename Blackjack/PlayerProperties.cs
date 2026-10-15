namespace Blackjack;

public class PlayerProperties
{
    public readonly List<int> CurrentCards = [];
    public bool IsStanding { get; set; }
    public int Score { get; private set; }

    public PlayerProperties()
    {
        ResetState();
    }

    public void ResetState()
    {
        CurrentCards.Clear();
        IsStanding = false;
    }

    public void AddScore()
    {
        Score++;
    }
}