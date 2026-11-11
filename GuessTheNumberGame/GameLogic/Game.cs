using GuessTheNumber.Input;
using GuessTheNumber.NumberGeneration;
using GuessTheNumberGame.GameLogic;

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
    private int _attemptsCount;
    public int AttemptsCount => _attemptsCount;
    private int _maxAttempts;

    public void Start()
    {
        _randomNumber = _numberGenerator.Generate();

        _invalidInputCount = 0;
        _status = GameStatus.InProgress;
        _attemptsCount = 0;
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
        _attemptsCount++;

        GuessResult checkResult = _numberChecker.Check(userNum, _randomNumber);

        if (checkResult == GuessResult.Correct)
            _status = GameStatus.Won;

         else if (_attemptsCount >= _maxAttempts)
        {
            _status = GameStatus.AttemptsEnded;
        }

        return checkResult;
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        switch (level)
        {
            case DifficultyLevel.Easy:
                _maxAttempts = 15;
                break;

            case DifficultyLevel.Medium:
                _maxAttempts = 10;
                break;

            case DifficultyLevel.Hard:
                _maxAttempts = 5;
                break;
        }
    }
}