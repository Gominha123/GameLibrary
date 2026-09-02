using System;

public class Game
{
    public string Title { get; }
    public string Genre { get; }
    public string Developer { get; }
    public float Rating { get; }
    public int ReleaseYear { get; }
    public int Id { get; }

    public Game(string title, string genre, string developer, float rating, int releaseYear, int id)
    {
        Title = title;
        Genre = genre;
        Developer = developer;
        Rating = rating;
        ReleaseYear = releaseYear;
        Id = id;
    }
}
