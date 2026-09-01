
using System.Globalization;

public class Program
{
    List<Game> games = new List<Game>();
    public static void Main(string[] args)
    {
        int option = -1;
        Program program = new Program();

        while (option != 5)
        {
            Console.WriteLine("Press the number for the corresponding option:");
            Console.WriteLine("1 - Add Game");
            Console.WriteLine("2 - List Games");
            Console.WriteLine("3 - Search Game");
            Console.WriteLine("4 - Remove Game");
            Console.WriteLine("5 - Exit");

            option = program.ReadInt();


            if (option == 1)
            {
                Console.Clear();
                program.AddGame();
            }
            else if (option == 2)
            {
                Console.Clear();
                program.ListGames();
            }
            else if (option == 3)
            {
                Console.Clear();
                program.SearchGame();
            }
            else if (option == 4)
            {
                Console.Clear();
                program.RemoveGame();
            }
            else if (option == 5)
            {
                Console.WriteLine("Exit");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid option");
            }
        }
    }

    public void AddGame()
    {
        Console.WriteLine("Add Game");
        Console.WriteLine("Enter the title of the game:");
        string title = Console.ReadLine();
        Console.WriteLine("Enter the genre of the game:");
        string genre = Console.ReadLine();
        Console.WriteLine("Enter the developer of the game:");
        string developer = Console.ReadLine();
        Console.WriteLine("Enter the rating of the game: (0-5)");
        string rating = Console.ReadLine();
        rating = rating.Replace(',', '.'); // normalize
        if (!float.TryParse(rating, NumberStyles.Float, CultureInfo.InvariantCulture, out float ratingValue))
        {
            // handle invalid input
            Console.WriteLine("invalid input");
            Leave();
            return;
        }
        if(ratingValue < 0 || ratingValue > 5)
        {
            Console.WriteLine("Rating must be between 0 and 5.");
            Leave();
            return;
        }

        Console.WriteLine("Enter the realease year of the game:");
        string releaseYear = Console.ReadLine();
        if (!int.TryParse(releaseYear, out int releaseYearInt))
        {
            Console.WriteLine("Invalid release year. Please enter a valid number.");
            Leave();
            return;
        }
        if(releaseYearInt < 1900 || releaseYearInt > DateTime.Now.Year)
        {
            Console.WriteLine($"Release year must be between 1950 and {DateTime.Now.Year}.");
            Leave();
            return;
        }

        Game newGame = new Game(title, genre, developer, ratingValue, releaseYearInt);
        games.Add(newGame);

        Console.Clear();
    }

    public void ListGames()
    {
        Console.WriteLine("List Games");

        foreach (Game game in games)
        {
            Console.WriteLine($"Title: {game.Title}, Genre: {game.Genre}, Developer: {game.Developer}, Rating: {game.Rating}, Release Year: {game.ReleaseYear}");
        }
        Leave();
    }

    public void SearchGame()
    {
        Console.Clear();
        int option = -1;
        string searchCondition = "";
        while (option > 6 || option < 1)
        {
            Console.WriteLine("Search Game");
            Console.WriteLine("Select how you wanna search the game");

            Console.WriteLine("1 - Title");
            Console.WriteLine("2 - Genre");
            Console.WriteLine("3 - Developer");
            Console.WriteLine("4 - Rating");
            Console.WriteLine("5 - Release Year");
            Console.WriteLine("6 - Exit");

            option = ReadInt();


            if (option == 1)
            {
                Console.WriteLine("Enter the title of the game:");
            }
            else if (option == 2)
            {
                Console.WriteLine("Enter the genre of the game:");
            }
            else if (option == 3)
            {
                Console.WriteLine("Enter the developer of the game:");
            }
            else if (option == 4)
            {
                Console.WriteLine("Enter the rating of the game:");
            }
            else if (option == 5)
            {
                Console.WriteLine("Enter the release year of the game:");
            }
            else if (option == 6)
            {
                Console.Clear();
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid option");
            }
        }

        searchCondition = Console.ReadLine();
        if (option == 1)
        {
            foreach (Game game in games)
            {
                if (game.Title.Contains(searchCondition,StringComparison.OrdinalIgnoreCase))
                {
                    ShowGame(game);
                }
            }
        }
        else if (option == 2)
        {
            foreach (Game game in games)
            {
                if (game.Genre.Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
                {
                    ShowGame(game);
                }
            }
        }
        else if (option == 3)
        {
            foreach (Game game in games)
            {
                if (game.Developer.Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
                {
                    ShowGame(game);
                }
            }
        }
        else if (option == 4)
        {
            searchCondition = searchCondition.Replace('.', ','); // normalize

            foreach (Game game in games)
            {
                if (game.Rating.ToString().Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
                {
                    ShowGame(game);
                }
            }

        }
        else if (option == 5)
        {
            foreach (Game game in games)
            {
                if (game.ReleaseYear.ToString().Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
                {
                    ShowGame(game);
                }
            }
        }

        Leave();
    }

    public void RemoveGame()
    {
        Console.WriteLine("Remove Game");
        Console.WriteLine("Enter the title of the game you want to remove:");

        string gameTitle = Console.ReadLine();
        int gamesFound = 0;
        List<int> indexOfGamesToBeRemoved = new List<int>();
        foreach (Game game in games)
        {
            if (gameTitle == game.Title)
            {
                indexOfGamesToBeRemoved.Add(gamesFound);
            }
            gamesFound++;
        }
        if (indexOfGamesToBeRemoved.Count == 0)
        {
            Console.WriteLine("Game not found");
            Leave();
        }
        else if (indexOfGamesToBeRemoved.Count > 1)
        {
            Console.Clear();
            Console.WriteLine("Multiple games found with the same title. Please select the id of the game to remove the one you want:\n");

            int i = 0;
            foreach (Game game in games)
            {
                if (indexOfGamesToBeRemoved.Contains(i))
                {
                    Console.Write($"ID: {i + 1} ");
                    ShowGame(game);
                }
                i++;
            }
            Console.WriteLine("-1 to cancel");
            int gameToRemove = 0;
            while (gameToRemove != -1)
            {
                if (int.TryParse(Console.ReadLine(), out gameToRemove) && indexOfGamesToBeRemoved.Contains(gameToRemove - 1))
                {
                    Console.WriteLine(gameTitle + " removed");
                    games.RemoveAt(gameToRemove - 1);
                    break;
                }
                else if (gameToRemove == -1)
                {
                    Console.WriteLine("Operation canceled");
                }
                else
                {
                    Console.WriteLine("Option invalid");
                }
            }
            Leave();
        }
        else
        {
            Console.WriteLine(gameTitle + " removed");
            games.RemoveAt(indexOfGamesToBeRemoved[0]);
            Leave();
        }
    }

    public void Leave()
    {
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        Console.Clear();
    }

    public void ShowGame(Game game)
    {
        Console.WriteLine($"Title: {game.Title}; Genre: {game.Genre}; Developer: {game.Developer}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
    }

    int ReadInt()
    {
        int result = 0;
        if (!int.TryParse(Console.ReadLine(), out result))
        {
            result = -1;
        }
        return result;
    }
}