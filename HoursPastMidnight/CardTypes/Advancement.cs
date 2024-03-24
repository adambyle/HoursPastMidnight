namespace HoursPastMidnight.CardTypes;

class Advancement(Technology technology) : ICardType
{
    public static string Name => "Technology";
    public Technology Technology { get; } = technology;
}
