namespace GuessTheNumber.Input;

public class UserInput : IUserInput
{

    public bool TryReadNumber(string input, out int userNum)
    {
        bool isValid = int.TryParse(input, out userNum) && userNum <= 100 && userNum > 0;

        return isValid;
    }

}

