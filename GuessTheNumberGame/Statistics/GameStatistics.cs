using System.IO;

namespace GuessTheNumberGame.Statistics;

public class GameStatistics
{
    public int TotalGames { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Attempts { get; private set; }

    public void StartGame()
    {
        TotalGames++;
    }

    public void EndGame(bool isWin, int attempts)
    {
        if (isWin)
        {
            Wins++;
        }
        else
        {
            Losses++;
        }
        Attempts += attempts;
    }

    public void SaveToFile(string filePath)
    {
        string statistics = $"Всего игр: {TotalGames}\n" + $"Победы: {Wins}\n" + $"Поражения: {Losses}\n" + $"Всего попыток: {Attempts}";

        File.WriteAllText(filePath, statistics);
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        string[] lines = File.ReadAllLines(filePath);

        TotalGames = int.Parse(lines[0].Split(':')[1]);
        Wins = int.Parse(lines[1].Split(':')[1]);
        Losses = int.Parse(lines[2].Split(':')[1]);
        Attempts = int.Parse(lines[3].Split(':')[1]);
    }
}
