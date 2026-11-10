using GuessTheNumber.Input;
using GuessTheNumber.NumberGeneration;

namespace GuessTheNumber.GameLogic;

public class Game
{
    public GameStatus Status => _status;

    private readonly INumberGenerator _numberGenerator;

    private readonly IUserInput _userInput;

    private readonly INumberChecker _numberChecker;

    public Game(INumberGenerator numberGenerator, IUserInput userInput, INumberChecker numberChecker)
    {
        _numberGenerator = numberGenerator;
        _userInput = userInput;
        _numberChecker = numberChecker;
    }


    private int _randomNumber;
    private int _invalidInputCount;
    private GameStatus _status;


    public void Start()
    {
        _randomNumber = _numberGenerator.Generate();

        _invalidInputCount = 0;
        _status = GameStatus.InProgress;
    }

    public GuessResult ProcessInput(string input)
    {
        if (!_userInput.TryReadNumber(input, out int userNum))
        {
            _invalidInputCount++;

            if (_invalidInputCount >= 3)
                _status = GameStatus.AttemptsEnded;
            return GuessResult.InvalidInput;

        }
        GuessResult checkResult = _numberChecker.Check(userNum, _randomNumber);

        if (checkResult == GuessResult.Correct)
            _status = GameStatus.Won;
        return checkResult;
    }
}