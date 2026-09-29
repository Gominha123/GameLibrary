using System.Globalization;
using Xunit;

namespace CSharpReview.Tests
{
    public class GameTests
    {
        #region GameConsole
        [Theory]
        [InlineData("1.2", "1.2")]
        [InlineData("4,6", "4.6")]
        [InlineData("5,0", "5.0")]
        public void GameConsole_ShouldReturnValidRating(string rating, string expected)
        {
            GameConsoleUI console = new GameConsoleUI();

            string ratingValue = console.GetRating(rating);

            Assert.Equal(expected, ratingValue);
        }

        #endregion

        #region GameService
        [Theory]
        [InlineData("1.2", 1.2f)]
        [InlineData("2.1", 2.1f)]
        [InlineData("4.1", 4.1f)]
        public void GameService_ShouldValidateRating(string rating, float expectedRating)
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            GameService service = new GameService(repository, repository);

            float ratingValue = service.ValidateRatingValue(rating);

            Assert.Equal(expectedRating, ratingValue);
        }

        [Theory]
        [InlineData("-1,2")]
        [InlineData("5,1")]
        [InlineData("10")]
        public void GameService_ShouldThrowForInvalidRating(string rating)
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            GameService service = new GameService(repository, repository);

            Assert.Throws<InvalidGameException>(() => service.ValidateRatingValue(rating));
        }

        [Theory]
        [InlineData("Minecraft", "Survival", "Mojang", 4.5f, 2011)]
        [InlineData("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026)]
        [InlineData("Valheim", "Survival", "Iron gate", 0.0f, 2000)]
        public void GameService_AddGame_ShouldAddGameToRepository(string title, string genre, string developer, float rating, int releaseYear)
        {
            // Arrange
            GameRepository<Game> repository = new GameRepository<Game>();
            GameService service = new GameService(repository, repository);

            // Act
            // chama aqui o AddGame com dados válidos
            service.AddGame(title, genre, developer, rating.ToString(CultureInfo.InvariantCulture), releaseYear.ToString());
            List<Game> games = repository.GetAll();

            // Assert
            // verifica aqui se o jogo foi adicionado
            Assert.Single(games);
            Assert.Equal(title, games[0].Title);
            Assert.Equal(genre, games[0].Genre);
            Assert.Equal(developer, games[0].Developer);
            Assert.Equal(rating, games[0].Rating);
            Assert.Equal(releaseYear, games[0].ReleaseYear);
        }

        [Theory]
        [InlineData("", "Survival", "Mojang", 4.5f, 2011)]
        [InlineData("Scrap Mechanic", "", "Axolot", 5.0f, 2026)]
        [InlineData("Valheim", "Survival", "", 0.0f, 2000)]
        [InlineData("GTA", "action rpg", "Rockstar", 5.1f, 2000)]
        [InlineData("HSR", "Turn-Based", "Hoyoverse", 0.0f, 2100)]
        public void GameService_AddGame_ShouldThrowForInvalidData(string title, string genre, string developer, float rating, int releaseYear)
        {
            // Arrange
            GameRepository<Game> repository = new GameRepository<Game>();
            GameService service = new GameService(repository, repository);

            Assert.Throws<InvalidGameException>(() =>
            service.AddGame(
                title,
                genre,
                developer,
                rating.ToString(CultureInfo.InvariantCulture),
                releaseYear.ToString()
                ));
        }


        [Fact]
        public void GameService_GetAllGames_ShouldGetAllGames()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);
            List<Game> expected = repository.GetAll();

            GameService service = new GameService(repository, repository);

            List<Game> result = service.GetAllGames();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GameService_GetAllGames_ShouldReturnEmptyListWhenNoGameMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();

            GameService service = new GameService(repository, repository);

            List<Game> result = service.GetAllGames();

            Assert.Empty(result);
        }


        [Fact]
        public void GameService_HowManyGamesFromGenre_ShouldReturnListOfGamesFromGenre()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 5.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.9f, 2021);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 4.0f, 2025);
            Game game4 = new Game("REPO", "Co-Op", "idk", 4.6f, 2024);
            Game game5 = new Game("Warframe", "MMO", "idk", 4.3f, 2014);
            Game game6 = new Game("League Of Legends", "MOBA", "Riot Games", 5.0f, 2009);

            repository.AddMultiple([game1, game2, game3, game4, game5, game6]);

            List<Game> expected = [game1, game2, game3];

            GameService service = new GameService(repository, repository);

            List<Game> result = service.HowManyGamesFromGenre("Survival");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GameService_Top3GamesAfterYearOrderedByRating_ShouldReturnTop3GamesOrdered()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 5.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.9f, 2021);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 4.0f, 2025);
            Game game4 = new Game("REPO", "Co-Op", "idk", 4.6f, 2024);
            Game game5 = new Game("Warframe", "MMO", "idk", 4.3f, 2014);
            Game game6 = new Game("League Of Legends", "MOBA", "Riot Games", 4.8f, 2009);

            repository.AddMultiple([game1, game2, game3, game4, game5, game6]);

            List<Game> expected = [game2, game4, game5];

            GameService service = new GameService(repository, repository);

            List<Game> result = service.Top3GamesAfterYearOrderedByRating("2012");

            Assert.Equal(expected, result);
        }


        #endregion

        #region GameRepository

        [Theory]
        [InlineData("Minecraft", "Survival", "Mojang", 4.5f, 2011)]
        [InlineData("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026)]
        [InlineData("Valheim", "Survival", "Iron gate", 0.0f, 2000)]
        public void GameRepository_Add_ShouldAddGame(string title, string genre, string developer, float rating, int releaseYear)
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game = new Game(title, genre, developer, rating, releaseYear);

            repository.Add(game);

            List<Game> list = repository.GetAll();
            Assert.Single(list);
            Assert.Equal(game, list[0]);
        }


        [Fact]
        public void GameRepository_AddMultiple_ShouldAddMultipleGames()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival", "Iron gate", 0.0f, 2000);

            List<Game> games = [game1, game2, game3];

            repository.AddMultiple(games);

            Assert.Equal(games, repository.GetAll());
        }

        [Fact]
        public void GameRepository_Remove_ShouldRemoveGame()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival", "Iron gate", 0.0f, 2000);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            repository.Remove(game2);

            Assert.Equal([game1, game3], repository.GetAll());
        }
        #endregion

        #region Filter

        [Fact]
        public void GameService_FilterByRating_ShouldReturnGamesWithAboveRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByRating("4.5");

            Assert.Equal([game2], result);
        }

        [Fact]
        public void GameService_FilterByRating_ShouldReturnEmptyListWhenNoGameMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2026);
            Game game3 = new Game("Valheim", "Survival", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByRating("4.5");

            Assert.Empty(result);
        }

        [Fact]
        public void GameService_FilterByGenre_ShouldReturnGamesWithMatchingGenre()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByGenre("Rpg");

            Assert.Equal([game3], result);
        }

        [Fact]
        public void GameService_FilterByGenre_ShouldReturnEmptyListWhenNoGameMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2026);
            Game game3 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByGenre("Action");

            Assert.Empty(result);
        }

        [Fact]
        public void GameService_FilterByReleaseYear_ShouldReturnGamesWithAboveReleaseYear()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2026);
            Game game3 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByReleaseYear("2015");

            Assert.Equal([game2], result);
        }

        [Fact]
        public void GameService_FilterByReleaseYear_ShouldReturnEmptyListWhenNoGameMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2025);
            Game game3 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.FilterByReleaseYear("2026");

            Assert.Empty(result);
        }

        #endregion

        #region Sort

        [Fact]
        public void GameService_SortByRating_ShouldReturnOrderedListByRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2025);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.SortByRating();

            Assert.Equal([game1, game3, game2], result);
        }

        [Fact]
        public void GameService_SortByReleaseYear_ShouldReturnOrderedListByReleaseYear()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2025);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.SortByReleaseYear();

            Assert.Equal([game2, game1, game3], result);
        }

        [Fact]
        public void GameService_SortByTitle_ShouldReturnOrderedListByTitle()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 0.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 3.0f, 2025);

            repository.AddMultiple([game1, game2, game3]);

            GameService service = new GameService(repository, repository);

            List<Game> result = service.SortByTitle();

            Assert.Equal([game1, game3, game2], result);
        }

        #endregion

        #region Statistics

        [Fact]
        public void GameService_ShouldReturnAverageRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];

            GameService service = new GameService(repository, repository);

            float result = service.Average(games);

            Assert.Equal(4f, result);
        }

        [Fact]
        public void GameService_ShouldReturnHighestRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            GameService service = new GameService(repository, repository);

            float result = service.HighestRated([game1, game2, game3]);

            Assert.Equal(5f, result);
        }

        [Fact]
        public void GameService_ShouldReturnLowestRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 3.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            GameService service = new GameService(repository, repository);

            float result = service.LowestRated([game1, game2, game3]);

            Assert.Equal(3f, result);
        }

        [Fact]
        public void GameService_ShouldReturnTop3Games()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 5.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.9f, 2021);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 4.0f, 2025);
            Game game4 = new Game("REPO", "Co-Op", "idk", 4.6f, 2024);
            Game game5 = new Game("Warframe", "MMO", "idk", 4.3f, 2014);
            Game game6 = new Game("League Of Legends", "MOBA", "Riot Games", 5.0f, 2009);

            GameService service = new GameService(repository, repository);

            string result = service.Top3Rated([game1, game2, game3, game4, game5, game6]);

            Assert.Equal($"{game1.Title}, {game6.Title}, {game2.Title}", result);
        }

        [Fact]
        public void GameService_ShouldReturnOldestGame()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 3.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            GameService service = new GameService(repository, repository);

            string result = service.Oldest([game1, game2, game3]);

            Assert.Equal(game2.Title, result);
        }

        [Fact]
        public void GameService_ShouldReturnNewestGame()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival", "Mojang", 3.0f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            GameService service = new GameService(repository, repository);

            string result = service.Newest([game1, game2, game3]);

            Assert.Equal(game3.Title, result);
        }
        #endregion

        #region Search

        [Fact]
        public void GameService_SearchByTitle_ShouldReturnListFromTitle()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByTitle("Valheim");

            Assert.Equal([game2], results);
        }

        [Fact]
        public void GameService_SearchByGenre_ShouldReturnListFromGenre()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByGenre("sandbox");

            Assert.Equal([game1], results);
        }

        [Fact]
        public void GameService_SearchByDeveloper_ShouldReturnListFromDeveloper()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByDeveloper("Axolot");

            Assert.Equal([game3], results);
        }

        [Fact]
        public void GameService_SearchByRating_ShouldReturnListFromRating()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByRating("4");

            Assert.Equal([game2], results);
        }

        [Fact]
        public void GameService_SearchByReleaseYear_ShouldReturnListFromReleaseYear()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByReleaseYear("2025");

            Assert.Equal([game3], results);
        }

        [Fact]
        public void GameService_SearchByTitle_ShouldReturnEmptyListWithNoMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByTitle("Valorant");

            Assert.Empty(results);
        }

        [Fact]
        public void GameService_SearchByGenre_ShouldReturnEmptyListWithNoMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByGenre("Action");

            Assert.Empty(results);
        }

        [Fact]
        public void GameService_SearchByDeveloper_ShouldReturnEmptyListWithNoMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByDeveloper("Riot Games");

            Assert.Empty(results);
        }

        [Fact]
        public void GameService_SearchByRating_ShouldReturnEmptyListWithNoMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 4.5f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByRating("3.6");

            Assert.Empty(results);
        }

        [Fact]
        public void GameService_SearchByReleaseYear_ShouldReturnEmptyListWithNoMatches()
        {
            GameRepository<Game> repository = new GameRepository<Game>();
            Game game1 = new Game("Minecraft", "Survival sandbox", "Mojang", 3f, 2011);
            Game game2 = new Game("Valheim", "Survival rpg", "Iron gate", 4.0f, 2000);
            Game game3 = new Game("Scrap Mechanic", "Survival", "Axolot", 5.0f, 2025);

            List<Game> games = [game1, game2, game3];
            repository.AddMultiple(games);

            GameService service = new GameService(repository, repository);

            List<Game> results = service.SearchByReleaseYear("2015");

            Assert.Empty(results);
        }

        #endregion
    }
}