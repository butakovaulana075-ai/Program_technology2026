
using GuessTheNumber.GameLogic;
using GuessTheNumber.Input;
using GuessTheNumber.NumberGeneration;
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