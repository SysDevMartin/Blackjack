using Spectre.Console;

namespace Blackjack;

public class Stats
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; }
    public int DamageMultiplier { get; private set; }

    public Stats(int maxHealth, int damageMultiplier)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        DamageMultiplier = damageMultiplier;
    }

    public int GetDamage(int cardSum)
    {
        return cardSum * DamageMultiplier;
    }

    public bool TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        return CurrentHealth <= 0;
    }

    public void IncreaseDamageMultiplier()
    {
        DamageMultiplier++;
    }
}