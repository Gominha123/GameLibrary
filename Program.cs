public class Program
{
    public static async Task Main(string[] args)
    {
        GameConsoleUI gameConsole = new GameConsoleUI();
        GameRepository<Game> repository = new GameRepository<Game>();

        GameFileService fileService = new GameFileService(repository);
        await fileService.LoadAllGames();

        GameService service = new GameService(repository, repository);
        int option = -1;

        while (option != 9)
        {
            gameConsole.ShowMainMenu();
            option = gameConsole.ReadInt();
            ChooseOption(gameConsole, service, option);
        }

        await fileService.SaveAllGames();
    }

    private static void ChooseOption(GameConsoleUI gameConsole, GameService service, int option)
    {
        if (option == 1)
        {
            gameConsole.ClearConsole();
            gameConsole.PrintMessage("Add a new game");
            string title = gameConsole.GetInput("Enter the title of the game:");
            string genre = gameConsole.GetInput("Enter the genre of the game:");
            string developer = gameConsole.GetInput("Enter the developer of the game:");
            string rating = gameConsole.GetRating("Enter the rating of the game (0.0 - 5.0):");
            string releaseYear = gameConsole.GetInput("Enter the release year of the game:");

            ExecuteServiceAction(gameConsole, () => service.AddGame(title, genre, developer, rating, releaseYear));

            gameConsole.ClearConsole();
        }
        else if (option == 2)
        {
            gameConsole.ClearConsole();
            gameConsole.PrintMessage("List Games");
            List<Game> games = new List<Game>();
            ExecuteServiceAction(gameConsole, () => games = service.GetAllGames());
            gameConsole.ShowGame(games);
            gameConsole.Leave();

        }
        else if (option == 3)
        {
            List<Game> searchResults = new List<Game>();
            gameConsole.ClearConsole();
            int searchOption = -1;
            string searchCondition = "";
            while (searchOption > 6 || searchOption < 1)
            {
                gameConsole.PrintMessage("Search Games");
                gameConsole.PrintMessage("Select how you wanna search the game");
                gameConsole.PrintMessage("1 - Title");
                gameConsole.PrintMessage("2 - Genre");
                gameConsole.PrintMessage("3 - Developer");
                gameConsole.PrintMessage("4 - Rating");
                gameConsole.PrintMessage("5 - Release Year");
                gameConsole.PrintMessage("6 - Exit");

                searchOption = gameConsole.ReadInt();

                gameConsole.ClearConsole();
                if (searchOption == 1)
                {
                    gameConsole.PrintMessage("Enter the title of the game:");
                }
                else if (searchOption == 2)
                {
                    gameConsole.PrintMessage("Enter the genre of the game:");
                }
                else if (searchOption == 3)
                {
                    gameConsole.PrintMessage("Enter the developer of the game:");
                }
                else if (searchOption == 4)
                {
                    gameConsole.PrintMessage("Enter the rating of the game:");
                }
                else if (searchOption == 5)
                {
                    gameConsole.PrintMessage("Enter the release year of the game:");
                }
                else if (searchOption == 6)
                {
                    return;
                }
                else
                {
                    gameConsole.PrintMessage("Invalid option");
                }
                searchCondition = gameConsole.GetInput("");
            }
            ExecuteServiceAction(gameConsole, () => searchResults = service.SearchGame(searchOption, searchCondition));
            if (!searchResults.Any())
            {
                gameConsole.PrintMessage("No games found matching the search criteria.");
            }
            else
            {
                gameConsole.ShowGame(searchResults);
            }
            gameConsole.Leave();
        }
        else if (option == 4)
        {
            gameConsole.ClearConsole();
            gameConsole.PrintMessage("Remove Game");
            string gameTitle = gameConsole.GetInput("Enter the title of the game you want to remove: ");
            List<Game> gameResults = new List<Game>();

            ExecuteServiceAction(gameConsole, () => gameResults = service.ChooseGameToRemove(gameTitle));

            if (gameResults.Count == 1)
            {
                service.RemoveGame(gameResults[0]);
                gameConsole.PrintMessage($"{gameResults[0].Title} was removed");
                gameConsole.Leave();
            }
            else if (gameResults.Count > 1)
            {
                gameConsole.PrintMessage("Multiple games found with the same title. Please select the game you want to remove:\n");
                int i = 0;
                foreach (Game game in gameResults)
                {
                    gameConsole.PrintMessageNoEnter($"{i} - ");
                    gameConsole.ShowGame(game);
                    i++;
                }

                int removeOption = gameConsole.ReadInt();

                if (removeOption >= 0 && removeOption < gameResults.Count)
                {
                    gameConsole.PrintMessage($"{gameResults[removeOption].Title} was removed");
                    service.RemoveGame(gameResults[removeOption]);
                }
                else
                {
                    gameConsole.PrintMessage("Invalid option. Operation canceled.");
                    return;
                }
                gameConsole.Leave();
            }
        }
        else if (option == 5)
        {
            gameConsole.ClearConsole();
            int filterOption = 0;
            while (filterOption > 7 || filterOption < 1)
            {
                gameConsole.PrintMessage("Filter Games");
                gameConsole.PrintMessage("Select how you wanna filter the games");
                gameConsole.PrintMessage("1 - Games above rating");
                gameConsole.PrintMessage("2 - Games by genre");
                gameConsole.PrintMessage("3 - Games after release year");
                gameConsole.PrintMessage("4 - Sort by rating");
                gameConsole.PrintMessage("5 - Sort by release year");
                gameConsole.PrintMessage("6 - Sort by title");
                gameConsole.PrintMessage("7 - Exit");
                filterOption = gameConsole.ReadInt();
                gameConsole.ClearConsole();

                List<Game> gamesSort = new List<Game>();
                if (filterOption == 1)
                {
                    string rating = gameConsole.GetRating("Enter rating: ");

                    ExecuteServiceAction(gameConsole, () => gamesSort = service.FilterByRating(rating));
                }
                else if (filterOption == 2)
                {
                    string genre = gameConsole.GetInput("Enter genre: ");

                    ExecuteServiceAction(gameConsole, () => gamesSort = service.FilterByGenre( genre));
                }
                else if (filterOption == 3)
                {
                    string releaseYear = gameConsole.GetYear("Enter the release year of the Game:");
                    ExecuteServiceAction(gameConsole, () => gamesSort = service.FilterByReleaseYear(releaseYear));
                }
                else if (filterOption == 4)
                {
                    ExecuteServiceAction(gameConsole, () => gamesSort = service.SortByRating());
                }
                else if (filterOption == 5)
                {
                    ExecuteServiceAction(gameConsole, () => gamesSort = service.SortByReleaseYear());
                }
                else if (filterOption == 6)
                {
                    ExecuteServiceAction(gameConsole, () => gamesSort = service.SortByTitle());
                }
                else if (filterOption != 7)
                {
                    gameConsole.PrintMessage("Invalid option");
                }
                else 
                {
                    return;
                }

                if (gamesSort.Any())
                {
                    gameConsole.ShowGame(gamesSort);
                }
                else
                {
                    gameConsole.PrintMessage("No games found");
                }
            }
            gameConsole.Leave();
            gameConsole.ClearConsole();
        }
        else if (option == 6)
        {
            List<Game> games = new List<Game>();
            gameConsole.ClearConsole();
            ExecuteServiceAction(gameConsole, () => games = service.GetAllGames());

            if (!games.Any())
            {
                gameConsole.PrintMessage("No games found");
                gameConsole.Leave();
                return;
            }
            gameConsole.PrintMessage("Statistics");
            gameConsole.PrintMessage($"Total Games: {games.Count}");
            gameConsole.PrintMessage($"Average rating: {service.Average(games):0.00}");
            gameConsole.PrintMessage($"Highest Rated Game: {service.HighestRated(games)}");
            gameConsole.PrintMessage($"Top 3 rated games: {service.Top3Rated(games)}");
            gameConsole.PrintMessage($"Lowest Rated Game: {service.LowestRated(games)}");
            gameConsole.PrintMessage($"Oldest Game: {service.Oldest(games)}");
            gameConsole.PrintMessage($"Newest Game: {service.Newest(games)}");
            gameConsole.Leave();

        }
        else if (option == 7)
        {
            gameConsole.ClearConsole();
            string genre = gameConsole.GetInput("Enter the genre to count games from:");
            List<Game> gamesFound = new List<Game>();
            ExecuteServiceAction(gameConsole, () => gamesFound = service.HowManyGamesFromGenre(genre));

            if (gamesFound.Any())
            {
                gameConsole.PrintMessage($"There are {genre} games.");

                gameConsole.PrintMessage($"Total {genre} Games: {gamesFound.Count}");

                gameConsole.ShowGameName(gamesFound);
            }
            else
            {
                gameConsole.PrintMessage($"There are no {genre} games.");
            }
            gameConsole.Leave();
        }
        else if (option == 8)
        {
            gameConsole.ClearConsole();
            string year = gameConsole.GetYear("Select the year: ");
            List<Game> gamesFound = new List<Game>();
            ExecuteServiceAction(gameConsole, () => gamesFound = service.Top3GamesAfterYearOrderedByRating(year));

            foreach (Game game in gamesFound)
            {
                gameConsole.PrintMessage($"Title: {game.Title}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
            }
            gameConsole.Leave();

        }
        else if (option != 9)
        {
            gameConsole.ClearConsole();
            gameConsole.PrintMessage("Invalid option");
        }

    }

    private static void ExecuteServiceAction(GameConsoleUI gameConsole, Action action)
    {
        try
        {
            action();
        }
        catch (InvalidGameException e)
        {
            gameConsole.PrintMessage(e.Message);
            gameConsole.WaitForInput();
        }
        catch (FormatException e)
        {
            gameConsole.PrintMessage("Invalid number format.");
            gameConsole.WaitForInput();
        }
        catch (Exception e)
        {
            gameConsole.PrintMessage(e.Message);
            gameConsole.WaitForInput();
        }
    }
}