public class Program
{
    public static void Main(string[] args)
    {
        GameRepository<Game> repository = new GameRepository<Game>();
        GameService service = new GameService(repository);

        int option = -1;

        while (option != 9)
        {
            Console.WriteLine("Press the number for the corresponding option:");
            Console.WriteLine("1 - Add Game");
            Console.WriteLine("2 - List Games");
            Console.WriteLine("3 - Search Game");
            Console.WriteLine("4 - Remove Game");
            Console.WriteLine("5 - Filter Games");
            Console.WriteLine("6 - Statistics");
            Console.WriteLine("7 - How Many Games from Genre");
            Console.WriteLine("8 - Top 3 games after year");
            Console.WriteLine("9 - Exit");

            option = service.ReadInt();


            if (option == 1)
            {
                Console.Clear();
                service.AddGame();
            }
            else if (option == 2)
            {
                Console.Clear();
                service.ListGames();
            }
            else if (option == 3)
            {
                Console.Clear();
                service.SearchGame();
            }
            else if (option == 4)
            {
                Console.Clear();
                service.RemoveGame();
            }
            else if (option == 5)
            {
                Console.Clear();
                service.FilterGames();
            }
            else if (option == 6)
            {
                Console.Clear();
                service.Statistics();
            }
            else if (option == 7)
            {
                Console.Clear();
                Console.WriteLine("Enter the genre to count games from:");
                string genre = Console.ReadLine();
                service.HowManyGamesFromGenre(genre);
            }
            else if (option == 8)
            {
                Console.Clear();
                Console.WriteLine("Select the year: ");
                int year = service.ReadInt();
                service.Top3GamesAfterYearOrderedByRating(year);
            }
            else if (option != 9)
            {
                Console.Clear();
                Console.WriteLine("Invalid option");
            }
        }
    }
}