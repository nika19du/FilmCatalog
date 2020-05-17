using FilmCatalog.Controller;
using FilmCatalog.Controller.Validation;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FilmCatalog.View
{
    public class Display
    {
        private const string closeOperationCommand = "12";
        private const string logInMessage = "You need to log in.";

        public static string username;
        private static string password;

        private MovieTagController movieTagController;
        private TagController tagController;
        private MovieController movieController;
        private GenreController genreController;
        private RatingController ratingController;
        private UserController userController;
        private Validate validate;
        public Display()
        {

        }
        public Display(FilmCatalogContext context)
        {
            this.movieTagController = new MovieTagController(context);
            this.tagController = new TagController(context);
            this.movieController = new MovieController(context);
            this.genreController = new GenreController(context);
            this.ratingController = new RatingController(context);
            this.userController = new UserController(context);
            this.validate = new Validate(context);
            this.RegisterOrLogin();
            HandleInput();
        }

        private void HandleInput()
        {
            string input;
            do
            {
                ShowCommands();
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListAllCommands();
                        break;
                    case "2":
                        GetByCommands();
                        break;
                    case "3":
                        GetByIdCommands();
                        break;
                    case "4":
                        AddCommands();
                        break;
                    case "5":
                        RemoveCommands();
                        break;
                    case "6":
                        UpdateCommand();
                        break;
                    case "7":
                        Rate();
                        break;
                    case "8"://show a user history
                        HistoryOfAddedMoviesByUser();
                        break;
                    case "9":
                        DeleteUser();
                        break;
                    case "10":
                        UpdateUser();
                        break;
                    case "11":
                        ListAllUsers();
                        break;
                }
            } while (input != closeOperationCommand);
        }


        private void UpdateUser()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Update a user");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("You can only update-name,password,email!!!");
            Console.WriteLine("\nWhat do you want to update?");
            string command = Console.ReadLine();
            switch (command)
            {
                case "name":
                    UpdateUserName();
                    break;
                case "password":
                    UpdatePassword();
                    break;
                case "email":
                    UpdateEmail();
                    break;
            }
            this.ReturnToMainScreen();
        }

        private void UpdateEmail()
        {
            Console.Write("User email: ");
            var email = Console.ReadLine();
            Console.Write("New email: ");
            var newEmail = Console.ReadLine();
            userController.UpdateUserEmail(email, newEmail);
        }

        private void UpdatePassword()
        {
            //Console.Write("User name: ");
            //var name = Console.ReadLine();
            var name = username;
            Console.Write("New password: ");
            var newPassword = Console.ReadLine();
            userController.UpdateUserPassword(name, newPassword);
        }

        private void UpdateUserName()
        {
            //Console.Write("User name: ");
            //var name = Console.ReadLine();
            var name = username;
            Console.Write("New name: ");
            var newName = Console.ReadLine();
            userController.UpdateUserName(name, newName);
        }

        private void DeleteUser()
        {
            Console.WriteLine("Are you sure you want to delete this profile. Yes or No?");
            var answer = Console.ReadLine();
            if (answer == "Yes" || answer=="yes")
            {
                var user = userController.GetUser(username);
                if (user != null)
                {
                    userController.DeleteUser(username);
                    Console.WriteLine("You successfully deleted your account!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("You don't have permission to delete this account.");
                }
            }
        }
        private void ListAllUsers()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("SHOW ALL Users!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            List<User> users = userController.GetAllUsers();
            List<string> list = new List<string>();
            foreach (var user in users)
            {
                var tags = user.Tags;
                StringBuilder sb = new StringBuilder();
                sb.Append("\nUsername: " + user.UserName + $" (email {user.email})");
                if (tags.Count > 0)
                {
                    string uTag = string.Empty;
                    sb.Append($" create tag ");
                    foreach (var tag in tags)
                    {
                        uTag = tag.Name;
                        sb.Append(uTag + "; ");
                    }
                }
                Console.WriteLine(sb.ToString());
                var movies = user.MoviesList;
                string m = string.Empty;
                foreach (Movie u in movies)
                {
                    Console.WriteLine($"\nUser {user.UserName} added movie - {u.Name} in movie catalog.");
                }
                Console.WriteLine(new string('_', 120));
            }
            Console.WriteLine();
            this.ReturnToMainScreen();
        }

        private void HistoryOfAddedMoviesByUser()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("SHOW USER HISTORY.");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            //Console.WriteLine("\nEnter existing user name: ");
            //var userName = Console.ReadLine();
            var userName = username;
            var user = userController.GetUser(userName);
            foreach (var u in user.MoviesList)
            {
                Console.WriteLine("\nMovie name: " + u.Name);
                Console.WriteLine("Movie actors: " + u.Actors);
                Console.WriteLine("Movie director: " + u.Director);
                Console.WriteLine("\nMovie info: " + u.Description);
                Console.WriteLine("Movie genre: " + u.Genre);
                Console.WriteLine(new string('_', 120));
            }
            foreach (var rating in user.Ratings)
            {
                var name = rating.User.UserName;
                var movie = rating.MovieId;
                var m = movieController.GetMovie(movie);
                Console.WriteLine($"User {name} added rating score {rating.Score:f2} on \"{m.Name}\" movie");
                Console.WriteLine(new string('_', 120));
            }
            foreach (var tag in user.Tags)
            {//pokazva koi user kakuv tag e dobavil, no ne kum filma a poprincip
                var userId = tag.User.Id;
                var username = userController.GetUserById(userId);
                Console.WriteLine($"User {username.UserName} create tag: {tag.Name}");
                Console.WriteLine(new string('_', 120) + "\n");
            }
            this.ReturnToMainScreen();
        }
        public string ReturnName()
        {
            //if (string.IsNullOrEmpty(username)==false)
          //  this.RegisterOrLogin();
                return username;
        }
        public void RegisterOrLogin()
        {
            if (string.IsNullOrEmpty(Form2.loginUser)==false)
            {
                username = Form2.loginUser;
                password = Form2.passUser;
                return;
            }
            if (string.IsNullOrEmpty(RegisterForm.user)==false)
            {
                username = RegisterForm.user;
                password = RegisterForm.password;
            }
        }
        private void Rate()
        {
            this.RatingCmdDisplay();
            Console.WriteLine(ratingController.AddRating());
            this.ReturnToMainScreen();
        }
        public void RatingCmdDisplay()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Rate a movie.");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- Add a rating to a movie by typing its name below");
            Console.WriteLine("- You can only rate an existing movie once or multiple times");
            Console.WriteLine("- If you rate a movie multiple times,the rating of the movie will be averaged");
        }
        private void RemoveCommands()
        {
            Console.WriteLine("1.Remove movie.");
            Console.WriteLine("2.Remove tag.");
            Console.WriteLine("3.Remove a tag from a recipe.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    RemoveMovie();
                    break;
                case "2":
                    RemoveTag();
                    break;
                case "3":
                    RemoveTagFromMovie();
                    break;
                default:
                    break;
            }
        }

        private void RemoveTagFromMovie()
        {
            DeleteTagFromMovieDisplay();
            Console.WriteLine("\nMovie id:");
            int movieId = int.Parse(Console.ReadLine());
            Console.WriteLine("\nTag id:");
            int tagId = int.Parse(Console.ReadLine());
            movieTagController.DeleteMovieTag(movieId, tagId);
            Console.WriteLine("Successfully remove tag from movie.");
            this.ReturnToMainScreen();
        }
        public void DeleteTagFromMovieDisplay()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Delete a tag from movie!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- Enter movie id you want to delete below!");
            Console.WriteLine("- Enter tag id you want to delete below!");
            Console.WriteLine("- You can only delete an existing tag id and movie id!");
        }

        private void RemoveTag()
        {
            this.DeleteTagDisplay();
            tagController.Delete();
            Console.WriteLine("Successfully remove tag!");
            this.ReturnToMainScreen();
        }

        private void RemoveMovie()
        {
            this.DeleteCmdDisplay();
            movieController.Delete();
            this.ReturnToMainScreen();
        }
        public void DeleteTagDisplay()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Delete a tag");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- Enter the tag name you want to delete below!");
            Console.WriteLine("- You can only delete an existing tag");
        }
        public void DeleteCmdDisplay()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Delete a movie");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- Enter the movie name you want to delete below!");
            Console.WriteLine("- You can only delete an existing movie");
        }

        private void GetByIdCommands()
        {
            Console.WriteLine("1.Get movie by id.");
            Console.WriteLine("2.Get tag by id.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    GetMovieById();
                    break;
                case "2":
                    GetMovieByTag();
                    break;
            }
        }

        private void GetMovieByTag()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Get movies by tag!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            Console.WriteLine("\nEnter tag id:");
            int id;
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("No such id.");
                return;
            }
            Tag tag = tagController.GetTag(id);
            if (tag == null)
            {
                Console.WriteLine("No such id.");
            }
            else
            {
                Console.WriteLine("Name: " + tag.Name);
            }
            ReturnToMainScreen();
        }

        private void GetMovieById()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Get movies by id!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nEnter movie id:");
            int id;
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("No such id.");
                return;
            }
            Movie movie = movieController.GetMovie(id);
            if (movie == null)
            {
                Console.WriteLine("No such id.");
            }
            else
            {
                PrintMovies(movie);
            }
            this.ReturnToMainScreen();
        }

        private void GetByCommands()
        {
            Console.WriteLine("1.List all movies with a given tag.");
            Console.WriteLine("2.List a movie by name.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    GetAllMoviesWithTag();
                    break;
                case "2":
                    GetMovieByName();
                    break;
            }
        }

        private void GetMovieByName()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Get all movies by name!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            Console.WriteLine("\nEnter movie name:");
            string name = Console.ReadLine();
            List<Movie> movies = movieController.GetMovieByName(name);
            if (movies.Count == 0)
            {
                Console.WriteLine("No movie with that name were found.");
            }
            else
            {
                foreach (Movie movie in movies)
                {
                    PrintMovies(movie);
                }
            }
            ReturnToMainScreen();
        }

        private void GetAllMoviesWithTag()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Get all movies with tag!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nEnter tag name:");
            string name = Console.ReadLine();
            Tag tag = tagController.GetAllTags().FirstOrDefault(x => x.Name == name);
            if (tag == null)
            {
                Console.WriteLine("No tags with that name were found.");
            }
            else
            {
                List<MovieTag> movieTags = tag.MovieTags.ToList();
                foreach (MovieTag movieTag in movieTags)
                {
                    PrintMovies(movieTag.Movie);
                }
            }
            ReturnToMainScreen();
        }

        private void UpdateCommand()
        {
            Console.WriteLine("1.Update movie.");
            Console.WriteLine("2.Update tag.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    this.UpdateCmdDisplay();
                    movieController.UpdateCommand();
                    break;
                case "2":
                    UpdateTag();
                    break;
            }
            this.ReturnToMainScreen();
        }

        private void UpdateTag()
        {
            Tag tag = new Tag();
            Console.WriteLine("Enter tag id:");
            int id = int.Parse(Console.ReadLine());
            tag.Id = id;
            Console.WriteLine("Enter new tag name:");
            string name = Console.ReadLine();
            tag.Name = name;
            tagController.UpdateTag(tag);
        }

        public void UpdateCmdDisplay()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("Update a movie");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- By updating a movie you have to change all its features");
            Console.WriteLine("- Type the name of the movie you want to change below");
            Console.WriteLine("- You can only update an existing movie");
            Console.WriteLine("- Type either genres,description or name to edit");
        }

        private void ListAllCommands()
        {
            string input;
            Console.WriteLine("1.List all movies.");
            Console.WriteLine("2.List all tags.");
            Console.WriteLine("3.List all genres.");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ListAllMovies();
                    break;
                case "2":
                    ListAllTags();
                    break;
                case "3":
                    ListAllGenres();
                    break;
            }
        }

        private void ListAllGenres()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("SHOW ALL MOVIES!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            var all = genreController.GetAllGenres();
            int count = 0;
            Console.WriteLine("\n\nGenres:");
            foreach (var genre in all)
            {
                count++;
                Console.WriteLine(count + ". " + genre.Name);
            }
            this.ReturnToMainScreen();
        }

        private void ListAllTags()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("SHOW ALL TAGS!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            Console.WriteLine("\n\nTags:");
            List<Tag> tags = tagController.GetAllTags();
            foreach (Tag tag in tags)
            {
                Console.WriteLine("ID: " + tag.Id);
                Console.WriteLine("Name: " + tag.Name);
                Console.WriteLine(new string('_', 40));
            }
            this.ReturnToMainScreen();
        }

        private void ListAllMovies()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("SHOW ALL MOVIES!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            List<Movie> movies = movieController.GetAllMovies();
            foreach (Movie movie in movies)
            {
                PrintMovies(movie);
            }
            this.ReturnToMainScreen();
        }

        private void PrintMovies(IMovie movie)
        {
            int count = 0;
            Console.WriteLine("\nID: " + movie.Id);
            Console.WriteLine("Name: " + movie.Name);
            Console.WriteLine("Actors: " + movie.Actors);
            Console.WriteLine("Director: " + movie.Director);
            Console.WriteLine("\nMovie info: " + movie.Description);
            Console.WriteLine($"Rating: {ratingController.CalculateRating(movie):f2}");
            Console.WriteLine("\nGenre: " + movie.Genre);
            Console.WriteLine("\nAdded by user with id: " + movie.UserId);
            var user = userController.GetUserById(movie.UserId);
            Console.WriteLine("Username: " + user.UserName);
            Console.WriteLine("\nTags: ");
            foreach (MovieTag movieTag in movie.MovieTags)
            {
                count++;
                if (count == movie.MovieTags.Count)
                {
                    Console.Write(movieTag.Tag.Name);
                    continue;
                }
                Console.Write(movieTag.Tag.Name + ", ");
            }
            Console.WriteLine();
            Console.WriteLine(new string('_', 122));
        }

        private void AddCommands()
        {
            Console.WriteLine("1.Add a movie.");
            Console.WriteLine("2.Add a tag.");
            Console.WriteLine("3.Add a tag to a movie.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddMovie();
                    break;
                case "2":
                    AddTag();
                    break;
                case "3":
                    AddTagToMovie();
                    break;
            }
        }

        private void AddTagToMovie()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("ADD TAG TO MOVIE!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            Console.WriteLine("\nEnter movie id:");
            int movieId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter tag id:");
            int tagId = int.Parse(Console.ReadLine());
            movieTagController.AddMovieTag(new MovieTag()
            { MovieId = movieId, TagId = tagId });

            this.ReturnToMainScreen();
        }
        private void AddTag()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("ADD A TAG!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");

            string input = tagController.TagRead();
            Tag tag = tagController.GetAllTags().FirstOrDefault(x => x.Name == input);
            //Console.WriteLine("Enter user name:");
            //string userName = Console.ReadLine();
            validate.ValidateUserName(username);
            User user = userController.GetUser(username);

            if (tag != null)
            {
                Console.WriteLine("Tag already exists and has id: " + tag.Id);
            }
            else
            {
                Tag tag1 = new Tag { Name = input, User = user };
                tagController.AddTag(tag1);
                userController.AddTag(tag1, user);
                Console.WriteLine("Successfully added tag!");
            }
            this.ReturnToMainScreen();
        }

        private void AddMovie()
        {
            this.AddCmdCommand();
            Movie movie = new Movie();
            movie.Name = movieController.MovieRead();
            Console.WriteLine("Enter actors:");
            movie.Actors = movieController.ActorsRead();
            Console.WriteLine("Enter director:");
            movie.Director = movieController.DirectorRead();
            movie.Description = movieController.DescriptionRead();
            movie.Genre = genreController.GenreRead();
            //  Console.WriteLine("Enter exist user name: ");
            //  var username = Console.ReadLine();
            var userName = username;
            var user = userController.GetUser(userName);
            movie.UserId = user.Id;
            movie.MovieTags = new List<MovieTag>();
            movieController.AddMovie(movie);
            this.ReturnToMainScreen();
        }
        public void ReturnToMainScreen()
        {
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            Console.Clear();
        }
        public void AddCmdCommand()
        {
            Console.Clear();
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("ADD A MOVIE!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\nAbout:");
            Console.WriteLine("- This command lets you add a new movie with all its characteristics");
            Console.WriteLine("- After you enter a genre,press Enter to enter new genre");
            Console.WriteLine("- After you entered your genres enter 'end' as a genre to finish your input");
            Console.WriteLine("- In the description you say about movie info and after you finished writing it enter '#' and press ENTER");
        }
        private void ShowCommands()
        {
            for (int i = 0; i < 122; i++)
            {
                Console.Write("-");
            }
            for (int i = 0; i < 52; i++)
                Console.Write(" ");
            Console.WriteLine("WELCOME TO MOVIE CATALOG!");
            for (int i = 0; i < 122; i++)
                Console.Write("-");
            Console.WriteLine("\n" + new string('_', 40));
            Console.WriteLine("List of the general commands:");
            Console.WriteLine("1.List all of type of commands.");
            Console.WriteLine("2.Search/List by type of commands.");
            Console.WriteLine("3.Get by id type of commands");
            Console.WriteLine("4.Add type of commands.");
            Console.WriteLine("5.Remove type of commands.");
            Console.WriteLine("6.Update type of commands.");
            Console.WriteLine("7.Rate type of commands.");
            Console.WriteLine("8.Show a user history.");
            Console.WriteLine("9.Delete user.");
            Console.WriteLine("10.Update user.");
            Console.WriteLine("11.Show all users.");
            Console.WriteLine("12.Exit.");
            Console.WriteLine(new string('_', 40));
        }
        
        public void PrintResult(string result)
        {
            if (string.IsNullOrEmpty(result) == false)
            {
                Console.WriteLine(result);
            }
        }
    }
}
