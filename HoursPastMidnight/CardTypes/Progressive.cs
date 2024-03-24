namespace HoursPastMidnight.CardTypes;

class Progressive(int stage) : ICardType
{
    public static string Name => "Progressive";
    public int Stage { get; } = stage;
}
