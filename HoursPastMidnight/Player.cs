using System.Runtime.CompilerServices;

class Player
{
    static int nextId = 0;

    private List<Card> _cards;
    private List<Card> _domesticDevQueue = [];
    private List<Card> _globalDevQueue = [];
    private List<Card> _progressiveQueue = [];

    public Stats Stats { get; private set; }
    public int Orders { get; private set; }
    public int Round { get; private set; }
    public string Name { get; }
    public int Id { get; }
    
    public Player(string name)
    {
        _cards = new(Setup.ROUNDS_PER_YEAR);
        Name = name;
        Id = nextId;
        nextId++;
    }

    public void GameInit()
    {
        Stats = Setup.StartStats;
    }

    public void YearInit()
    {
        Round = 0;
        Orders = Setup.YEAR_START_ORDERS;
    }

    public void NextRound(Setup setup)
    {
        Round++;
        Orders += Setup.ROUND_NEW_ORDERS;

        void chooseFromRemove(List<Card> source)
        {
            int index = Setup.Random.Next(source.Count);
            _cards.Add(source[index]);
            source.RemoveAt(index);
        }

        // Choose cards for the next year.
        _cards.Clear();

        var advancementCards = setup.AdvancementCards;
        _cards.Add(advancementCards.ElementAt(Setup.Random.Next(advancementCards.Count())));

        var newDomesticCards = setup.DomesticCards.ToList();
        for (int i = 0; i < Setup.DOMESTIC_ROUNDS_COUNT; i++)
            chooseFromRemove(newDomesticCards);
        for (int i = 0; i < Setup.DOMESTIC_DEV_ROUNDS_COUNT; i++)
        {
            if (_domesticDevQueue.Count > 0)
                chooseFromRemove(_domesticDevQueue);
            else
                chooseFromRemove(newDomesticCards);
        }

        var newGlobalCards = setup.GlobalCards.ToList();
        for (int i = 0; i < Setup.GLOBAL_ROUNDS_COUNT; i++)
            chooseFromRemove(newGlobalCards);
        for (int i = 0; i < Setup.GLOBAL_DEV_ROUNDS_COUNT; i++)
        {
            if (_globalDevQueue.Count > 0)
                chooseFromRemove(_globalDevQueue);
            else
                chooseFromRemove(newGlobalCards);
        }

        var newProgressiveCards = setup.ProgressiveBaseCards.ToList();
        for (int i = 0; i < Setup.PROGRESSIVE_ROUNDS; i++)
        {
            if (_progressiveQueue.Count > 0)
                chooseFromRemove(_progressiveQueue);
            else
                chooseFromRemove(newProgressiveCards);
        }

        // Fisher-Yates shuffle all but the first item, which will be the last round.
        // Rounds are popped off the top.
        for (int n = _cards.Count - 1; n >= 2; n--)
        {
            int k = Setup.Random.Next(1, n);
            (_cards[n], _cards[k]) = (_cards[k], _cards[n]);
        }
    }
}
