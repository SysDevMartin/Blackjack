namespace Blackjack;

public class Stats
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; }
    public float DamageMultiplier { get; private set; }

    public Stats(int maxHealth, float damageMultiplier)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        DamageMultiplier = damageMultiplier;
    }
}