record struct Stats(int Popularity, int Military, int Environment, int Economy)
{
    public static Stats Zero() => new(0, 0, 0, 0);

    public readonly int Get(StatType stat)
    {
        return stat switch
        {
            StatType.Popularity => Popularity,
            StatType.Military => Military,
            StatType.Environment => Environment,
            StatType.Economy => Economy,
            _ => 0,
        };
    }

    public static Stats operator+(Stats stat1, Stats stat2)
    {
        return new(stat1.Popularity + stat2.Popularity,
            stat1.Military + stat2.Military,
            stat1.Environment + stat2.Environment,
            stat1.Economy + stat2.Economy);
    }

    public static Stats operator-(Stats stat1, Stats stat2)
    {
        return new(stat1.Popularity - stat2.Popularity,
            stat1.Military - stat2.Military,
            stat1.Environment - stat2.Environment,
            stat1.Economy - stat2.Economy);
    }

    public static Stats operator-(Stats stats)
    {
        return new(-stats.Popularity,
            -stats.Military,
            -stats.Environment,
            -stats.Economy);
    }
}
