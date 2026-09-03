public class Program
{
    public static void Main(string[] args)
    {
        List<Game> games = new List<Game>();
        GameService service = new GameService();

        int option = -1;

        while (option != 7)
        {
            Console.WriteLine("Press the number for the corresponding option:");
            Console.WriteLine("1 - Add Game");
            Console.WriteLine("2 - List Games");
            Console.WriteLine("3 - Search Game");
            Console.WriteLine("4 - Remove Game");
            Console.WriteLine("5 - Filter Games");
            Console.WriteLine("6 - Statistics");
            Console.WriteLine("7 - Exit");

            option = service.ReadInt();


            if (option == 1)
            {
                Console.Clear();
                service.AddGame(games);
            }
            else if (option == 2)
            {
                Console.Clear();
                service.ListGames(games);
            }
            else if (option == 3)
            {
                Console.Clear();
                service.SearchGame(games);
            }
            else if (option == 4)
            {
                Console.Clear();
                service.RemoveGame(games);
            }
            else if(option == 5)
            {
                Console.Clear();
                service.FilterGames(games);
            }
            else if (option == 6)
            {
                Console.Clear();
                service.Statistics(games);
            }
            else if (option != 7)
            {
                Console.Clear();
                Console.WriteLine("Invalid option");
            }
        }
    }
}