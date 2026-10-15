namespace Blackjack;

public static class Game
{
    public struct Settings
    {
        public static int GamePanelWidth { get; } = 75;
        public static int TargetCardSum { get; } = 21;
        public static int StartingCardCount { get; } = 2;
        public static int MaxCardCount { get; } = 10;
        public static int MinCardValue { get; } = 1;
        public static int MaxCardValue { get; } = 10;
    }
}