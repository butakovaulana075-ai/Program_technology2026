
using GuessTheNumber.GameLogic;
using GuessTheNumber.Input;
using GuessTheNumber.NumberGeneration;
using GuessTheNumberGame.GameLogic;
using GuessTheNumberGame.Statistics;
using System.Globalization;

namespace GuessTheNumberGame;

internal class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        INumberGenerator numberGenerator = new NumberGenerator(random);

        IUserInput userInput = new UserInput();

        INumberChecker numberChecker = new NumberChecker();

        Game game = new Game(numberGenerator, userInput, numberChecker);

        GameStatistics statistics = new GameStatistics();

        string filePath = "statistics.txt";
        statistics.LoadFromFile(filePath);

        Console.WriteLine("Выберите уровень сложности:");
        Console.WriteLine("1 — Легкий");
        Console.WriteLine("2 — Средний");
        Console.WriteLine("3 — Сложный");

        string difficultyInput = Console.ReadLine();

        DifficultyLevel difficultyLevel;

        switch (difficultyInput)
        {
            case "1":
                difficultyLevel = DifficultyLevel.Easy;
                break;

            case "2":
                difficultyLevel = DifficultyLevel.Medium;
                break;

            case "3":
                difficultyLevel = DifficultyLevel.Hard;
                break;

            default: 
                Console.WriteLine("Некорректный ввод. Установлена легкая сложность");
                difficultyLevel = DifficultyLevel.Easy;
                break;
        }

        game.SetDifficulty(difficultyLevel);

        game.Start();
        statistics.StartGame();

        while (game.Status == GameStatus.InProgress)
        {
            Console.WriteLine("Введите число от 1 до 100:");
            string input = Console.ReadLine();
            GuessResult result = game.ProcessInput(input);

            switch (result)
            {
                case (GuessResult.TooHigh):
                    Console.WriteLine("Число больше загаданного");
                    break;

                case (GuessResult.TooLow):
                    Console.WriteLine("Число меньше загаданного");
                    break;

                case (GuessResult.Correct):
                    Console.WriteLine("Вы угадали!");
                    break;

                case (GuessResult.InvalidInput):
                    Console.WriteLine("Некорректный ввод");
                    break;
            }
        }

        bool isWin = game.Status == GameStatus.Won;

        statistics.EndGame(isWin, game.AttemptsCount);

        statistics.SaveToFile(filePath);

        if (game.Status == GameStatus.Won)
        {
            Console.WriteLine("Победа!");
        }
        else if (game.Status == GameStatus.AttemptsEnded)
        {
            Console.WriteLine("Попытки закончились!");
        }
    }
}