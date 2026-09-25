using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public class GameService
{
    private GameRepository<Game> repository;
    public GameService(GameRepository<Game> _repository)
    {
        repository = _repository;
    }
    public GameConsoleUI gameConsoleUI = new GameConsoleUI();

    public void AddGame(string title, string genre, string developer, string rating, string releaseYear)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidGameException("Title cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new InvalidGameException("Genre cannot be empty.");
        }

        List<Game> duplicate = GetGames(repository.GetAll(), g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && g.Developer.Equals(developer, StringComparison.OrdinalIgnoreCase));
        if (duplicate.Any())
        {
            throw new InvalidGameException("Game already exists");
        }

        float ratingValue = ValidateRatingValue(rating);

        int releaseYearInt = ValidateReleaseYearValue(releaseYear);

        Game newGame = new Game(title, genre, developer, ratingValue, releaseYearInt);

        repository.Add(newGame);

    }

    public void ListGames()
    {
        ShowGame(repository.GetAll());
    }

    public List<Game> SearchGame(int option, string searchCondition)
    {
        List<Game> games = repository.GetAll();
        List<Game> results = new List<Game>();
        if (option == 1)
        {
            results = GetGames(games, g => g.Title.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
        }
        else if (option == 2)
        {
            results = GetGames(games, g => g.Genre.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
        }
        else if (option == 3)
        {
            results = GetGames(games, g => g.Developer.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
        }
        else if (option == 4)
        {
            searchCondition = searchCondition.Replace(',', '.'); // normalize
            float ratingValue = ValidateRatingValue(searchCondition);

            results = GetGames(games, g => g.Rating.Equals(ratingValue));
        }
        else if (option == 5)
        {
            int releaseYearInt = ValidateReleaseYearValue(searchCondition);

            results = GetGames(games, g => g.ReleaseYear.Equals(releaseYearInt));
        }
        return results;
    }

    public void RemoveGame(Game game)
    {
        repository.Remove(game);
    }

    public List<Game> ChooseGameToRemove(string gameTitle)
    {
        List<Game> games = repository.GetAll();
        List<Game> gamesToBeRemoved = new List<Game>();

        gamesToBeRemoved = GetGames(games, g => g.Title.Contains(gameTitle, StringComparison.OrdinalIgnoreCase));

        if (!gamesToBeRemoved.Any())
        {
            throw new InvalidGameException("Game not found");
        }
        return gamesToBeRemoved;
    }

    public List<Game> FilterGames(int option, string condition)
    {
        List<Game> games = repository.GetAll();
        List<Game> gameSort = new List<Game>();
        if (option == 1)
        {
            float ratingValue = ValidateRatingValue(condition);
            gameSort = GetGames(games, g => g.Rating > ratingValue);
        }
        else if (option == 2)
        {
            gameSort = GetGames(games, g => g.Genre.Contains(condition, StringComparison.OrdinalIgnoreCase));
        }
        else if (option == 3)
        {
            int releaseYearInt = ValidateReleaseYearValue(condition);
            gameSort = GetGames(games, g => g.ReleaseYear > releaseYearInt);
        }
        else if (option == 4)
        {
            gameSort = games.OrderByDescending(g => g.Rating).ToList();
        }
        else if (option == 5)
        {

            gameSort = games.OrderBy(g => g.ReleaseYear).ToList();
        }
        return gameSort;
    }

    public List<Game> Statistics()
    {
        return repository.GetAll();
    }

    public void HowManyGamesFromGenre(string genre)
    {
        List<Game> games = repository.GetAll();
        List<Game> gamesFound = GetGames(games, g => g.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
        if (gamesFound.Any())
        {
            gameConsoleUI.PrintMessage($"There are {genre} games.");

            gameConsoleUI.PrintMessage($"Total {genre} Games: {gamesFound.Count}");

            ShowGameName(gamesFound);
        }
        else
        {
            gameConsoleUI.PrintMessage($"There are no {genre} games.");
        }
        gameConsoleUI.Leave();
    }

    public void Top3GamesAfterYearOrderedByRating(string year)
    {
        int yearValue = ValidateReleaseYearValue(year);

        List<Game> games = repository.GetAll();
        List<Game> gamesFound = games.Where(g => g.ReleaseYear > yearValue).OrderByDescending(g => g.Rating).Take(3).ToList();

        foreach (Game game in gamesFound)
        {
            gameConsoleUI.PrintMessage($"Title: {game.Title}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
        }
        gameConsoleUI.Leave();
    }

    private static List<Game> GetGames(List<Game> games, Func<Game, bool> condition)
    {
        return games.Where(condition).ToList();
    }

    private void ShowGame(Game game)
    {
        gameConsoleUI.PrintMessage($"Title: {game.Title}; Genre: {game.Genre}; Developer: {game.Developer}; Rating: {game.Rating}; Release Year: {game.ReleaseYear}");
    }

    private void ShowGame(List<Game> games)
    {
        foreach (Game game in games)
        {
            ShowGame(game);
        }
    }

    private void ShowGameName(List<Game> games)
    {
        foreach (Game game in games)
        {
            gameConsoleUI.PrintMessage($"Title: {game.Title}");
        }
    }

    public int ValidateReleaseYearValue(string releaseYear)
    {
        int releaseYearInt = ParseValue(releaseYear, int.Parse);
        if (releaseYearInt < 1950 || releaseYearInt > DateTime.Now.Year)
        {
            throw new InvalidGameException($"Release year must be between 1950 and {DateTime.Now.Year}.");
        }

        return releaseYearInt;

    }

    public float ValidateRatingValue(string rating)
    {
        float ratingValue = ParseValue(rating, value => float.Parse(value, CultureInfo.InvariantCulture));
        if (ratingValue < 0 || ratingValue > 5)
        {
            throw new InvalidGameException("Rating Value must be between 0 and 5");
        }

        return ratingValue;
    }

    private T ParseValue<T>(string value, Func<string, T> parser)
    {
        return parser(value);
    }
}
