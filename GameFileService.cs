using System;
using System.Text.Json;

public class GameFileService
{
    private GameRepository<Game> repository;
    public GameFileService(GameRepository<Game> _repository)
    {
        repository = _repository;
    }

    public async Task LoadAllGames()
    {
        string json;
        try
        {
            json = await File.ReadAllTextAsync("games.json");

            List<Game> games = JsonSerializer.Deserialize<List<Game>>(json);
            if (games != null)
            {
                repository.AddMultiple(games);
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Could not find the file.");
            Console.ReadLine();
            Console.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
            Console.Clear();
        }
    }

    public async Task SaveAllGames()
    {
        try
        {
            List<Game> games = repository.GetAll();
            string json = JsonSerializer.Serialize(games);
            await File.WriteAllTextAsync("games.json", json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
