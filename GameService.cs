using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public class GameService
{
    private IGameReader<Game> reader;
    private IGameWriter<Game> writer;

    public GameService(IGameReader<Game> _reader, IGameWriter<Game> _writer)
    {
        reader = _reader;
        writer = _writer;
    }

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

        if (string.IsNullOrWhiteSpace(developer))
        {
            throw new InvalidGameException("Genre cannot be empty.");
        }

        List<Game> duplicate = GetGames(reader.GetAll(), g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && g.Developer.Equals(developer, StringComparison.OrdinalIgnoreCase));
        if (duplicate.Any())
        {
            throw new InvalidGameException("Game already exists");
        }

        float ratingValue = ValidateRatingValue(rating);

        int releaseYearInt = ValidateReleaseYearValue(releaseYear);

        Game newGame = new Game(title, genre, developer, ratingValue, releaseYearInt);

        writer.Add(newGame);
    }

    public List<Game> SearchByTitle(string searchCondition)
    {
        List<Game> games = reader.GetAll();
        return GetGames(games, g => g.Title.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
    }

    public List<Game> SearchByGenre(string searchCondition)
    {
        List<Game> games = reader.GetAll();
        return GetGames(games, g => g.Genre.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
    }

    public List<Game> SearchByDeveloper(string searchCondition)
    {
        List<Game> games = reader.GetAll();
        return GetGames(games, g => g.Developer.Contains(searchCondition, StringComparison.OrdinalIgnoreCase));
    }

    public List<Game> SearchByRating(string searchCondition)
    {
        List<Game> games = reader.GetAll();
        searchCondition = searchCondition.Replace(',', '.');
        float ratingValue = ValidateRatingValue(searchCondition);

        return GetGames(games, g => g.Rating.Equals(ratingValue));
    }

    public List<Game> SearchByReleaseYear(string searchCondition)
    {
        List<Game> games = reader.GetAll();
        int releaseYearInt = ValidateReleaseYearValue(searchCondition);
        
        return GetGames(games, g => g.ReleaseYear.Equals(releaseYearInt));
    }

    public void RemoveGame(Game game)
    {
        writer.Remove(game);
    }

    public List<Game> ChooseGameToRemove(string gameTitle)
    {
        List<Game> games = reader.GetAll();
        List<Game> gamesToBeRemoved = new List<Game>();

        gamesToBeRemoved = GetGames(games, g => g.Title.Contains(gameTitle, StringComparison.OrdinalIgnoreCase));

        if (!gamesToBeRemoved.Any())
        {
            throw new InvalidGameException("Game not found");
        }
        return gamesToBeRemoved;
    }

    public List<Game> FilterByRating(string condition)
    {
        List<Game> games = reader.GetAll();
        float ratingValue = ValidateRatingValue(condition);
        return GetGames(games, g => g.Rating > ratingValue);
    }

    public List<Game> FilterByGenre(string condition)
    {
        List<Game> games = reader.GetAll();
        return GetGames(games, g => g.Genre.Contains(condition, StringComparison.OrdinalIgnoreCase));
    }

    public List<Game> FilterByReleaseYear(string condition)
    {
        List<Game> games = reader.GetAll();
        int releaseYearInt = ValidateReleaseYearValue(condition);
        return GetGames(games, g => g.ReleaseYear > releaseYearInt);
    }

    public List<Game> SortByRating()
    {
        List<Game> games = reader.GetAll();
        return games.OrderByDescending(g => g.Rating).ToList();
    }

    public List<Game> SortByReleaseYear()
    {
        List<Game> games = reader.GetAll();
        return games.OrderBy(g => g.ReleaseYear).ToList();
    }

    public List<Game> SortByTitle()
    {
        List<Game> games = reader.GetAll();
        return games.OrderBy(g => g.Title).ToList();
    }

    public float Average(List<Game> games)
    {
        return games.Average(g => g.Rating);
    }

    public float HighestRated(List<Game> games)
    {
        return games.Max(g => g.Rating);
    }

    public float LowestRated(List<Game> games)
    {
        return games.Min(g => g.Rating);
    }

    public string Top3Rated(List<Game> games)
    {
        var top3Games = games.OrderByDescending(g => g.Rating).Take(3).Select(g => g.Title);
        return string.Join(", ", top3Games);
    }

    public string Oldest(List<Game> games)
    {
        return games.MinBy(g => g.ReleaseYear).Title;
    }

    public string Newest(List<Game> games)
    {
        return games.MaxBy(g => g.ReleaseYear).Title;
    }

    public List<Game> HowManyGamesFromGenre(string genre)
    {
        List<Game> games = reader.GetAll();
        return GetGames(games, g => g.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
    }

    public List<Game> Top3GamesAfterYearOrderedByRating(string year)
    {
        int yearValue = ValidateReleaseYearValue(year);

        List<Game> games = reader.GetAll();
        return games.Where(g => g.ReleaseYear > yearValue).OrderByDescending(g => g.Rating).Take(3).ToList();
    }

    private static List<Game> GetGames(List<Game> games, Func<Game, bool> condition)
    {
        return games.Where(condition).ToList();
    }

    public List<Game> GetAllGames()
    {
        return reader.GetAll();
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
