using System.Globalization;

public class GameService
{
    public void AddGame(List<Game> games)
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
        if (ratingValue < 0 || ratingValue > 5)
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
        if (releaseYearInt < 1950 || releaseYearInt > DateTime.Now.Year)
        {
            Console.WriteLine($"Release year must be between 1950 and {DateTime.Now.Year}.");
            Leave();
            return;
        }

        Game newGame = new Game(title, genre, developer, ratingValue, releaseYearInt);
        games.Add(newGame);

        Console.Clear();
    }

    public void ListGames(List<Game> games)
    {
        Console.WriteLine("List Games");

        ShowGame(games);
        Leave();
    }

    public void SearchGame(List<Game> games)
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

            Console.Clear();
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
                return;
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }

        List<Game> results = new List<Game>();
        searchCondition = Console.ReadLine();
        if (option == 1)
        {
            results = games.Where(g => g.Title.Contains(searchCondition, StringComparison.OrdinalIgnoreCase)).ToList();
            ShowGame(results);
        }
        else if (option == 2)
        {
            results = games.Where(g => g.Genre.Contains(searchCondition, StringComparison.OrdinalIgnoreCase)).ToList();
            ShowGame(results);
        }
        else if (option == 3)
        {
            results = games.Where(g => g.Developer.Contains(searchCondition, StringComparison.OrdinalIgnoreCase)).ToList();
            ShowGame(results);
        }
        else if (option == 4)
        {
            searchCondition = searchCondition.Replace(',', '.'); // normalize

            if (!float.TryParse(searchCondition, NumberStyles.Float, CultureInfo.InvariantCulture, out float ratingValue))
            {
                Console.WriteLine("Invalid rating. Please enter a valid number.");
                Leave();
                return;
            }
            results = games.Where(g => g.Rating.Equals(ratingValue)).ToList();
            //results = games.Where(g => g.Rating.ToString().Contains(searchCondition, StringComparison.OrdinalIgnoreCase)).ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("No games found with the specified rating.");
                Leave();
                return;
            }
            ShowGame(results);
        }
        else if (option == 5)
        {
            if (!int.TryParse(searchCondition, out int releaseYearValue))
            {
                Console.WriteLine("Invalid release Year. Please enter a valid number.");
                Leave();
                return;
            }
            results = games.Where(g => g.ReleaseYear.Equals(releaseYearValue)).ToList();
            if (!results.Any())
            {
                Console.WriteLine("No games found with the specified release year.");
                Leave();
                return;
            }
            ShowGame(results);
        }

        Leave();
    }

    public void RemoveGame(List<Game> games)
    {
        Console.WriteLine("Remove Game");
        Console.WriteLine("Enter the title of the game you want to remove:");

        string gameTitle = Console.ReadLine();
        List<Game> gamesToBeRemoved = new List<Game>();

        gamesToBeRemoved = games.Where(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase)).ToList();


        if (!gamesToBeRemoved.Any())
        {
            Console.WriteLine("Game not found");
            Leave();
            return;
        }
        else if (gamesToBeRemoved.Count == 1)
        {
            Console.WriteLine($"{gameTitle} was removed");
            games.Remove(gamesToBeRemoved[0]);
            Leave();
        }
        else if (gamesToBeRemoved.Count > 1)
        {
            int i = 0;
            foreach (Game game in gamesToBeRemoved)
            {
                Console.Write($"{i} - ");
                ShowGame(game);
                i++;
            }
            Console.WriteLine("Multiple games found with the same title. Please select the game you want to remove:\n");

            int option = ReadInt();

            if (option >= 0 && option < gamesToBeRemoved.Count)
            {
                Console.WriteLine($"{gameTitle} was removed");
                games.Remove(gamesToBeRemoved[option]);
            }
            else
            {
                Console.WriteLine("Invalid option. Operation canceled.");
            }

            Leave();
        }
    }

    public void FilterGames(List<Game> games)
    {
        int option = 0;

        while (option > 6 || option < 1)
        {
            Console.WriteLine("Filter Games");
            Console.WriteLine("Select how you wanna filter the games");
            Console.WriteLine("1 - Games above rating");
            Console.WriteLine("2 - Games by genre");
            Console.WriteLine("3 - Games after release year");
            Console.WriteLine("4 - Sort by rating");
            Console.WriteLine("5 - Sort by release year");
            Console.WriteLine("6 - Exit");
            option = ReadInt();
            if (option == 1)
            {
                Console.Clear();
                Console.WriteLine("Enter rating: ");

                string rating = Console.ReadLine();
                rating = rating.Replace(',', '.'); // normalize
                if (!float.TryParse(rating, NumberStyles.Float, CultureInfo.InvariantCulture, out float ratingValue))
                {
                    Console.WriteLine("Invalid rating. Please enter a valid number.");
                    Leave();
                    return;
                }
                if (ratingValue < 0 || ratingValue > 5)
                {
                    Console.WriteLine("Rating must be between 0 and 5.");
                    Leave();
                    return;
                }

                List<Game> gameSort = games.Where(g => g.Rating > ratingValue).ToList();

                if (gameSort.Count > 0)
                {
                    ShowGame(gameSort);
                }
                else
                {
                    Console.WriteLine("No games found");
                }
                Leave();
            }
            else if (option == 2)
            {
                Console.Clear();
                Console.WriteLine("Enter genre: ");
                string genre = Console.ReadLine();
                List<Game> gameSort = games.Where(g => g.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)).ToList();
                if (gameSort.Count > 0)
                {
                    ShowGame(gameSort);
                }
                else
                {
                    Console.WriteLine("No games found");
                }
                Leave();
            }
            else if (option == 3)
            {
                Console.Clear();
                Console.WriteLine("Enter release year: ");
                int releaseYear = ReadInt();
                if (releaseYear < 1950 || releaseYear > DateTime.Now.Year)
                {
                    Console.WriteLine("Invalid release year. Please enter a valid number.");
                    Leave();
                    return;
                }

                List<Game> gameSort = games.Where(g => g.ReleaseYear > releaseYear).ToList();
                if (gameSort.Count > 0)
                {
                    ShowGame(gameSort);
                }
                else
                {
                    Console.WriteLine("No games found");
                }
                Leave();
            }
            else if (option == 4)
            {
                Console.Clear();
                List<Game> gameSort = games.OrderByDescending(g => g.Rating).ToList();

                if (gameSort.Count > 0)
                {
                    ShowGame(gameSort);
                }
                else Console.WriteLine("No games found");

                Leave();
            }
            else if (option == 5)
            {
                Console.Clear();

                List<Game> gameSort = games.OrderBy(g => g.ReleaseYear).ToList();

                if (gameSort.Count > 0)
                {
                    ShowGame(gameSort);
                }
                else Console.WriteLine("No games found");

                Leave();
            }
            else if (option != 6)
            {
                Console.Clear();
                Console.WriteLine("Invalid option");
            }
        }
        Console.Clear();
    }

    public void Statistics(List<Game> games)
    {
        if (!games.Any())
        {
            Console.WriteLine("No games found");
            Leave();
            return;
        }
        Console.WriteLine("Statistics");

        Console.WriteLine($"Total Games: {games.Count}");
        Console.WriteLine($"Average rating: {games.Average(g => g.Rating):0.00}");
        Console.WriteLine($"Highest Rated Game: {games.Max(g => g.Rating)}");
        var top3Games = games.OrderByDescending(g => g.Rating).Take(3).Select(g => g.Title);
        Console.WriteLine($"Top 3 rated games: {string.Join(", ", top3Games)}");
        Console.WriteLine($"Lowest Rated Game: {games.Min(g => g.Rating)}");
        Console.WriteLine($"Oldest Game: {games.MinBy(g => g.ReleaseYear).Title}");
        Console.WriteLine($"Newest Game: {games.MaxBy(g => g.ReleaseYear).Title}");

        Leave();
    }

    public void ShowGame(Game game)
    {
        Console.WriteLine($"Title: {game.Title}; Genre: {game.Genre}; Developer: {game.Developer}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
    }

    public void ShowGame(List<Game> games)
    {
        foreach (Game game in games)
        {
            ShowGame(game);
        }
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

    public void Leave()
    {
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        Console.Clear();
    }
}
