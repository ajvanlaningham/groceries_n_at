using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace groceries_n_at
{
    class Program
    {
        //List<Measurements> measurements1 = new List<Measurements>();

        /*
         * VAR1
         * VAR2
         * 
         */


        static void Main(string[] args)
        {
            // instantiateValues OR populateGobals

            Console.WriteLine("Welcome to grocery list beta. I can't promise it will work in any capacity");
            bool isRunning = true;
            while (isRunning)
            {

                Console.WriteLine("Would you like to...");
                Console.WriteLine("1. Pick from the list of recipes for this week");
                Console.WriteLine("2. Create a new recipe?");
                Console.WriteLine("3. View an existing recipe?");
                Console.WriteLine();
                Console.Write("User choice:  ");

                string menu = Console.ReadLine().ToLower();
                Console.Clear();
                if (menu.Contains("pick"))
                {
                    pickrecipes();
                }
                else if (menu.Contains("new"))
                {
                    newrecipe();
                }
                else if (menu.Contains("view"))
                {
                    viewrecipe();
                }
                else
                {
                    Console.WriteLine("I don't think that was one of the options that I gave you");
                }
            }
        }

        static void pickrecipes()
        {

        }

        static void newrecipe()
        {
            Recipe recipe = new Recipe();
            Console.WriteLine("You selected that you'd like to build a new recipe for the database... is that correct? y/n");
            string menu = Console.ReadLine().ToLower();
            if (menu == "y")
            {
                Console.Clear();
                Console.WriteLine("Cool beans... What do you want to call this recipe?");
                recipe.Title = Console.ReadLine();
                Console.Clear();
                Console.WriteLine($"Your recipe will be saved as {recipe.Title}.");
                Console.WriteLine("What category would you like this recipe stored in? (ex: main, dessert, breakfast, etc)");
                recipe.Category = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Last but not least, what is this recipe's cuisine? Chinese? French? American?");
                recipe.Cuisine = Console.ReadLine();

                SqlConnection connection = openconnection();
                connection.Open();

                saveRecipe(recipe, connection);

                saveingredients(recipe, connection);

                Instructions instructions = new Instructions();
                Console.WriteLine($"Write the instructions for {recipe.Title} as thoroughly as you can");
                instructions.Text = Console.ReadLine();
                instructions.RecipeId = recipe.Id;
                connection = openconnection();
                saveinstructions(instructions, connection);
                connection.Close();
            }
            else if (menu == "n")
            {
                return;
            }
        }
        static void viewrecipe()
        {

        }
        static void saveRecipe(Recipe recipe, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [Recipe] (Title, Category, Cuisine) VALUES ('{recipe.Title}', '{recipe.Category}', '{recipe.Cuisine}')", connection);
            command.ExecuteNonQuery();
        }
        static void saveinstructions(Instructions instructions, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [Instructions] (RecipeId, Text) VALUES ('{instructions.RecipeId}','{instructions.Text}'", connection);
            command.ExecuteNonQuery();
        }
        static void saveingredients(Recipe recipe, SqlConnection connection)
        {

            List<Measurements> measurements = getmeasurements(connection);
            connection.Close();
            List<Ingredients> ingredients = new List<Ingredients>();
            Console.WriteLine($"how many ingredients would one need to make {recipe.Title}?");
            int counter = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("List them");
            for (int i = 0; i < counter; i++)
            {
                Ingredients tempIngredient = new Ingredients();
                tempIngredient.Name = Console.ReadLine();
                tempIngredient.RecipeId = recipe.Id;
                ingredients.Add(tempIngredient);
            }

            foreach (Ingredients item in ingredients)
            {
                Console.WriteLine($"What is {item.Name} measured in?");
                string measureName = Console.ReadLine();
                foreach (Measurements measureType in measurements)
                {
                    if (measureType.MeasurementType == measureName)
                    {
                        item.MeasurmentId = measureType.Id;
                    }
                }
                Console.WriteLine($"How many {measureName}s of {item.Name}?");
                item.Quantity = Convert.ToInt32(Console.ReadLine());
            }
            foreach (Ingredients item in ingredients)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Name);
                Console.WriteLine(item.RecipeId);
                Console.WriteLine(item.Quantity);
                Console.WriteLine(item.MeasurmentId);

            }
        }
        static List<Measurements> getmeasurements(SqlConnection connection)
        {
            List<Measurements> measurements = new List<Measurements>();
            SqlCommand command = new SqlCommand("Select * FROM [Measurements]", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Measurements tempMeasurement = new Measurements();
                tempMeasurement.Id = Convert.ToInt32(reader["Id"]);
                tempMeasurement.MeasurementType = reader["MeasurementType"].ToString();
                measurements.Add(tempMeasurement);
            }
            reader.Close();
            return measurements;
        }
        static SqlConnection openconnection()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ajvan\source\repos\groceries\groceries_n_at\groceries_n_at\groceries_n_at\Database1.mdf;Integrated Security=True");
            return connection;
        }




    }

    class ITEMS
    {
        public int Id;
        public string Name;
        public int StoreId;
        public decimal Cost;
    }
    class Recipe
    {
        public int Id;
        public string Title;
        public string Cuisine;
        public string Category;

    }
    class Ingredients
    {
        public int Id;
        public int RecipeId;
        public string Name;
        public int ItemsId;
        public int MeasurmentId;
        public int Quantity;
    }
    class Measurements
    {
        public int Id;
        public string MeasurementType;
    }
    class Instructions
    {
        public int Id;
        public int RecipeId;
        public string Text;
    }
}
