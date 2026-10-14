using Spectre.Console;

namespace Blackjack;

class Program
{
    private const int TargetCardSum = 21;
    private const int StartingCardCount = 2;
    
    //private static Random RandomNumberGenerator = new();
    private static List<int> CurrentCards = new();
    public static bool IsGameActive = true;
    
    static void Main(string[] args)
    {
        Dealer dealer = new();
        Player player = new();
        
        /*int GetRandomCard()
        {
            var minNumber = 1;
            var maxNumber = 10;
            return RandomNumberGenerator.Next(minNumber, maxNumber + 1);
        }*/

        /*void DrawCard()
        {
            var newCard = GetRandomCard();
            CurrentCards.Add(newCard);
            
            var currentCardSum = CurrentCards.Sum();
            if (currentCardSum > TargetCardSum)
            {
                Console.WriteLine($"Card sum: {CurrentCards.Sum()}");
                Console.WriteLine($"You went over {TargetCardSum} with your total sum of {currentCardSum} and lost the game!");
                IsGameActive = false;
            }
        }*/

        void Stand()
        {
            //Console.WriteLine($"Card sum: {CurrentCards.Sum()}");
            var currentCardSum = CurrentCards.Sum();
            if (CurrentCards.Sum() == TargetCardSum)
            {
                Console.WriteLine("You won!");
            }
            else
            {
                Console.WriteLine($"You lost! Your total card sum of {currentCardSum} didn't match the target card sum of {TargetCardSum}.");
            }
            IsGameActive = false;
        }

        void QuitGame()
        {
            Console.WriteLine("Quit the game? (Y/N)");
            var quitConfirmationPrompt = Console.ReadLine();
            if (quitConfirmationPrompt?.ToUpper() == "Y")
            {
                IsGameActive = false;
            }
        }

        /*void PromptInput()
        {
            while (true)
            {
                Console.WriteLine("Enter your choice [1=Draw | 2=Stand | 0=Quit]:");
                var playerInput = Console.ReadLine();

                switch (playerInput)
                {
                    case "1":
                        Console.WriteLine("You chose to draw a card.");
                        DrawCard();
                        ShowCards();
                        break;
                    case "2":
                        Console.WriteLine("You chose to stand.");
                        Stand();
                        break;
                    case "0":
                        QuitGame();
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        continue;
                }

                break;
            }
        }*/

        void PromptSelection()
        {
            var selectedMenuItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your choice?")
                    .AddChoices("Hit", "Stand", "End Game"));
            
            switch (selectedMenuItem)
            {
                case "Hit":
                    Console.WriteLine("You chose to draw a card.");
                    dealer.DealCardToPlayer(player);
                    //ShowCards();
                    player.DisplayHand();
                    break;
                case "Stand":
                    Console.WriteLine("You chose to stand.");
                    Stand();
                    break;
                case "End Game":
                    QuitGame();
                    break;
            }
        }
        
        var gamePanel = new Panel("BLACKJACK");
        AnsiConsole.Write(gamePanel);
        
        // Draw a starting hand.
        for (int i = 0; i < StartingCardCount; i++)
        {
            dealer.DealCardToPlayer(player);
        }
        player.DisplayHand();
        
        while (IsGameActive)
        {
            //PromptInput();
            PromptSelection();
        }
        
        Console.WriteLine("----------");
        Console.WriteLine("Press <Enter> to exit");
        Console.ReadLine();
    }
}