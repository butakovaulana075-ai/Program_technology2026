namespace GuessTheNumber.NumberGeneration;

public class NumberGenerator : INumberGenerator
{
    private readonly Random _random;

    public NumberGenerator(Random random)
    {
        _random = random;
    }

    public int Generate()
    {
        return _random.Next(1, 101);
    }
}
