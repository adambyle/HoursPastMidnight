using CardTypes = HoursPastMidnight.CardTypes;

record class Setup(IReadOnlyList<Card> CardPool)
{
    IEnumerable<Card>? _domesticCards = null;
    IEnumerable<Card>? _globalCards = null;
    IEnumerable<Card>? _progressiveBaseCards = null;
    IEnumerable<Card>? _advancementCards = null;

    public static Stats StartStats => new(7, 7, 7, 7);

    public const int START_YEAR = 1990;

    public const int YEAR_START_ORDERS = 2;
    public const int ROUND_NEW_ORDERS = 1;
    
    public const int ROUNDS_PER_YEAR = 24;
    public const int DOMESTIC_ROUNDS_COUNT = 9;
    public const int DOMESTIC_DEV_ROUNDS_COUNT = 3;
    public const int GLOBAL_ROUNDS_COUNT = 3;
    public const int GLOBAL_DEV_ROUNDS_COUNT = 6;
    public const int PROGRESSIVE_ROUNDS = 2;
    public const int ADVANCEMENT_ROUNDS = 1;

    public IEnumerable<Card> DomesticCards
    {
        get
        {
            _domesticCards ??= CardsWithType<CardTypes.Domestic>();
            return _domesticCards;
        }
    }

    public IEnumerable<Card> GlobalCards
    {
        get
        {
            _globalCards ??= CardsWithType<CardTypes.Global>();
            return _globalCards;
        }
    }

    public IEnumerable<Card> ProgressiveBaseCards
    {
        get
        {
            _progressiveBaseCards ??= from Card in CardPool
                                           where Card.CardType is CardTypes.Progressive progressive
                                                 && progressive.Stage == 0
                                           select Card;
            return _progressiveBaseCards;
        }
    }

    public IEnumerable<Card> AdvancementCards
    {
        get
        {
            _advancementCards ??= CardsWithType<CardTypes.Advancement>();
            return _advancementCards;
        }
    }

    public static Random Random { get; } = new();

    private IEnumerable<Card> CardsWithType<T>()
        where T : CardTypes.ICardType
    {
        return from Card in CardPool
               where Card.CardType is T
               select Card;
    }
}
