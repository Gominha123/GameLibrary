using System;

public class GameConsoleUI
{
    public void ClearConsole()
    {
        Console.Clear();
    }

    public string GetRating(string message)
    {
        Console.WriteLine(message);
        string rating = Console.ReadLine();
        rating = rating.Replace(',', '.'); // normalize
        return rating;
    }

    public string GetYear(string message)
    {
        Console.WriteLine(message);
        string releaseYear = Console.ReadLine();
        return releaseYear;
    }

    public string GetInput(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine();
    }

    public void ShowMainMenu()
    {
        PrintMessage("Press the number for the corresponding option:");
        PrintMessage("1 - Add Game");
        PrintMessage("2 - List Games");
        PrintMessage("3 - Search Game");
        PrintMessage("4 - Remove Game");
        PrintMessage("5 - Filter Games");
        PrintMessage("6 - Statistics");
        PrintMessage("7 - How Many Games from Genre");
        PrintMessage("8 - Top 3 games after year");
        PrintMessage("9 - Exit");
    }

    public int ReadInt()
    {
        int result = 0;
        if (!int.TryParse(Console.ReadLine(), out result))
        {
            result = -1;
        }
        return result;
    }

    public void PrintMessage(string message)
    {
                Console.WriteLine(message);
    }
    public void PrintMessageNoEnter(string message)
    {
                Console.Write(message);
    }

    public void Leave()
    {
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        ClearConsole();
    }

    public void WaitForInput()
    {
        Console.ReadLine();
        ClearConsole();
    }

    public void ShowGame(Game game)
    {
        PrintMessage($"Title: {game.Title}; Genre: {game.Genre}; Developer: {game.Developer}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
    }

    public void ShowGame(List<Game> games)
    {
        foreach (Game game in games)
        {
            ShowGame(game);
        }
    }

    public void ShowGameName(List<Game> games)
    {
        foreach (Game game in games)
        {
            PrintMessage($"Title: {game.Title}");
        }
    }
}
