namespace GuessTheNumber.GameLogic;

public class NumberChecker : INumberChecker
{
    public GuessResult Check(int userNum, int random)
    {
        if (userNum < random) return GuessResult.TooLow;
        if (userNum > random) return GuessResult.TooHigh;
        return GuessResult.Correct;
    }
}
