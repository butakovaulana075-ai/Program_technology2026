namespace GuessTheNumber.GameLogic;

public interface INumberChecker
{
    GuessResult Check(int userNum, int random);
}
