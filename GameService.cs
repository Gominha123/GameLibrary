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
        if (releaseYearInt < 1900 || releaseYearInt > DateTime.Now.Year)
        {
            Console.WriteLine($"Release year must be between 1950 and {DateTime.Now.Year}.");
            Leave();
            return;
        }

        Game newGame = new Game(title, genre, developer, ratingValue, releaseYearInt, games.Count);
        games.Add(newGame);

        Console.Clear();
    }

    public void ListGames(List<Game> games)
    {
        Console.WriteLine("List Games");

        foreach (Game game in games)
        {
            Console.WriteLine($"Title: {game.Title}, Genre: {game.Genre}, Developer: {game.Developer}, Rating: {game.Rating}, Release Year: {game.ReleaseYear}");
        }
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
                if (game.Title.Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
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

    public void RemoveGame(List<Game> games)
    {
        Console.WriteLine("Remove Game");
        Console.WriteLine("Enter the title of the game you want to remove:");

        string gameTitle = Console.ReadLine();
        int gamesFound = 0;
        List<Game> indexOfGamesToBeRemoved = new List<Game>();

        indexOfGamesToBeRemoved = games.Where(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase)).ToList();

        if (indexOfGamesToBeRemoved.Count == 0)
        {
            Console.WriteLine("Game not found");
            Leave();
            return;
        }
        else if (indexOfGamesToBeRemoved.Count > 1)
        {
            Console.WriteLine("Select the id of the game you want to remove: ");
            foreach(Game game in indexOfGamesToBeRemoved)
            {
                Console.Write($"Id: {game.Id} ");
                ShowGame(game);
            }

            int idSelected = ReadInt();
            if(idSelected < 0 || idSelected >= indexOfGamesToBeRemoved.Count)
            {
                Console.WriteLine("Invalid id");
                Leave();
                Console.Clear();
                return;
            }
            games.RemoveAt(idSelected);
            Console.WriteLine($"{gameTitle} with Id {idSelected} was removed");
            Leave();

        }
        else if (indexOfGamesToBeRemoved.Count == 1)
        {
            Console.WriteLine($"{gameTitle} was removed");
            games.Remove(indexOfGamesToBeRemoved[0]);
            Leave();
        }

        //foreach (Game game in games)
        //{
        //    if (gameTitle == game.Title)
        //    {
        //        indexOfGamesToBeRemoved.Add(gamesFound);
        //    }
        //    gamesFound++;
        //}
        //if (indexOfGamesToBeRemoved.Count == 0)
        //{
        //    Console.WriteLine("Game not found");
        //    Leave();
        //}
        //else if (indexOfGamesToBeRemoved.Count > 1)
        //{
        //    Console.Clear();
        //    Console.WriteLine("Multiple games found with the same title. Please select the id of the game to remove the one you want:\n");

        //    int i = 0;
        //    foreach (Game game in games)
        //    {
        //        if (indexOfGamesToBeRemoved.Contains(i))
        //        {
        //            Console.Write($"ID: {i + 1} ");
        //            ShowGame(game);
        //        }
        //        i++;
        //    }
        //    Console.WriteLine("-1 to cancel");
        //    int gameToRemove = 0;
        //    while (gameToRemove != -1)
        //    {
        //        if (int.TryParse(Console.ReadLine(), out gameToRemove) && indexOfGamesToBeRemoved.Contains(gameToRemove - 1))
        //        {
        //            Console.WriteLine(gameTitle + " removed");
        //            games.RemoveAt(gameToRemove - 1);
        //            break;
        //        }
        //        else if (gameToRemove == -1)
        //        {
        //            Console.WriteLine("Operation canceled");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Option invalid");
        //        }
        //    }
        //    Leave();
        //}
        //else
        //{
        //    Console.WriteLine(gameTitle + " removed");
        //    games.RemoveAt(indexOfGamesToBeRemoved[0]);
        //    Leave();
        //}
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
                if (!float.TryParse(rating, NumberStyles.Float, CultureInfo.InvariantCulture, out float ratingValue) && (ratingValue > 0 && ratingValue < 5))
                {
                    // handle invalid input
                    Console.WriteLine("Invalid rating. Please enter a valid number.");
                    Leave();
                }

                List<Game> gamesSort = games.Where(g => g.Rating > ratingValue).ToList();

                if (gamesSort.Count > 0)
                {
                    foreach (Game game in gamesSort)
                    {
                        ShowGame(game);
                    }
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
                List<Game> gameSort = games.Where(g => g.Genre == genre).ToList();
                if (gameSort.Count > 0)
                {
                    foreach (Game game in gameSort)
                    {
                        ShowGame(game);
                    }
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
                if (releaseYear <= -1 || releaseYear >= DateTime.Now.Year)
                {
                    Console.WriteLine("Invalid release year. Please enter a valid number.");
                    Leave();
                    return;
                }

                List<Game> gameSort = games.Where(g => g.ReleaseYear > releaseYear).ToList();
                if (gameSort.Count > 0)
                {
                    foreach (Game game in gameSort)
                    {
                        ShowGame(game);
                    }
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
                List<Game> gameSort = games.OrderBy(g => g.Rating).ToList();

                if (gameSort.Count > 0)
                {
                    foreach (Game game in gameSort)
                    {
                        ShowGame(game);
                    }
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
                    foreach (Game game in gameSort)
                    {
                        ShowGame(game);
                    }
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
        if (games.Count == 0)
        {
            Console.WriteLine("No games found");
            Leave();
            return;
        }
        Console.WriteLine("Statistics");

        Console.WriteLine($"Total Games: {games.Count}");
        Console.WriteLine($"Average rating: {games.Average(g => g.Rating):0.00}");
        Console.WriteLine($"Highest Rated Game: {games.Max(g => g.Rating)}");
        Console.WriteLine($"Lowest Rated Game: {games.Min(g => g.Rating)}");
        Console.WriteLine($"Oldest Game: {games.OrderBy(g => g.ReleaseYear).ToList()[0].Title}");
        Console.WriteLine($"Newest Game: {games.OrderByDescending(g => g.ReleaseYear).ToList()[0].Title}");

        Leave();
    }

    public void ShowGame(Game game)
    {
        Console.WriteLine($"Title: {game.Title}; Genre: {game.Genre}; Developer: {game.Developer}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
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
