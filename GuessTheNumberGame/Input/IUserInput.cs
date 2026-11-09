namespace GuessTheNumber.Input;

public interface IUserInput
{
    bool TryReadNumber(string input, out int userNum);
}
