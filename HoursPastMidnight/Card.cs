
using CardTypes = HoursPastMidnight.CardTypes;

class Card(int id, string title, string premise, CardTypes.ICardType cardType, List<Choice> choices)
{
    protected List<Choice> _choices = choices;

    public int Id { get; } = id;
    public string Title { get; } = title;
    public string Premise { get; } = premise;
    public IReadOnlyList<Choice> Choices => _choices;
    public CardTypes.ICardType CardType { get; } = cardType;
}
