using Spectre.Console;

namespace Blackjack;

public class Dealer
{
    public readonly List<int> CurrentCards = [];
    
    private Random RandomNumberGenerator = new();
    
    private int GetRandomCard()
    {
        var minNumber = 1;
        var maxNumber = 10;
        return RandomNumberGenerator.Next(minNumber, maxNumber + 1);
    }

    public string GetRandomPhrase()
    {
        string[] availablePhrases =
        {
            "What's the move?",
            "Your call, boss.",
            "Hit or stand?",
            "Time to make a choice.",
            "What are we thinking here?",
            "Feeling brave or playing it safe?",
            "The cards await your decision.",
            "Trust your instincts.",
            "What's the strategy?",
            "Think this one through.",
            "The next move is yours.",
            "Careful now, this one's interesting.",
            "Will you risk it?",
            "Stand proud or hit hard?",
            "What's the game plan?",
            "Let's see that poker face work.",
            "Go with your gut.",
            "Feeling lucky?",
            "What's your play?",
            "No pressure. Well, a little pressure.",
            "Choose wisely.",
            "Do you like those odds?",
            "The table is watching.",
            "Big decision coming up.",
            "One card can change everything.",
            "What's it gonna be?",
            "Fortune favors the bold. Sometimes.",
            "Do you want another card?",
            "How much do you trust this hand?",
            "Your move, card shark.",
            "I can wait. The cards can't.",
            "You've got options.",
            "Don't overthink it. Or do.",
            "Let's hear the verdict.",
            "Hit me with your decision.",
            "Stand and fight or roll the dice?",
            "The suspense is unbearable.",
            "What's your next move, hotshot?",
            "This hand has potential.",
            "Feeling confident?",
            "You've come this far.",
            "Do you tempt fate?",
            "The cards are listening.",
            "Make it count.",
            "What's the play of the day?",
            "I'm curious where this is going.",
            "A risky move could pay off.",
            "All eyes on you.",
            "This could be genius.",
            "Or this could be a disaster."
        };
        
        var randomIndex = RandomNumberGenerator.Next(availablePhrases.Length);
        return availablePhrases[randomIndex];
    }

    public void DisplayPanel(bool revealCards = false)
    {
        //var panelContent = $"Dealer says: {GetRandomPhrase()}\n";
        var panelContent = "";
        foreach (var card in CurrentCards)
        {
            if (revealCards)
            {
                panelContent += $"[{card}] ";
            }
            else
            {
                panelContent += "[?] ";
            }
        }

        AnsiConsole.WriteLine("");
        
        var escapedMarkup = Markup.Escape(panelContent);
        var markup = new Markup(escapedMarkup);
        var panel = new Panel(markup)
            {
                Width = Program.Settings.GamePanelWidth
            }
            .Header("DEALER")
            .BorderColor(Color.DarkOrange);
        AnsiConsole.Write(panel);
    }
    
    public void DealCardToPlayer(Player player)
    {
        var newCard = GetRandomCard();
        player.CurrentCards.Add(newCard);
            
        var currentCardSum = player.CurrentCards.Sum();
        if (currentCardSum > Program.Settings.TargetCardSum)
        {
            Console.WriteLine($"Card sum: {player.CurrentCards.Sum()}");
            Console.WriteLine($"You lost with a card sum of {currentCardSum}!");
            Program.IsGameActive = false;
        }
    }

    public void DealCardToSelf()
    {
        var newCard = GetRandomCard();
        CurrentCards.Add(newCard);
        
        var currentCardSum = CurrentCards.Sum();
        if (currentCardSum > Program.Settings.TargetCardSum)
        {
            Console.WriteLine($"Dealer lost with a card sum of {currentCardSum}!");
        }
    }
}