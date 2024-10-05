using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

class Car
{
    public int CarID { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public DateTime DateAdded { get; set; }

    public override string ToString()
    {
        return $"{CarID}: {Make} {Model} ({Year}) - ${Price} (Added on {DateAdded.ToShortDateString()})";
    }
}

class Program
{
    static void Main()
    {
        string conn = "Server=localhost;Database=datab;UserId=root;Password=1234";
        bool exit = false;

        
        while (!exit)
        {
            Console.WriteLine("\nWelcome to the Car Management App:");
            Console.WriteLine("1- View All Cars");
            Console.WriteLine("2- Add a New Car");
            Console.WriteLine("3- Update an Existing Car");
            Console.WriteLine("4- Exit");
            Console.Write("Please select an option: ");

            string option = Console.ReadLine();         
            switch (option)
            {
                case "1":
                   
                    SeeAllCars(conn);
                    break;
                case "2":
                    
                    AddNewCar(conn);
                    break;
                case "3":
                   
                    EditCar(conn);
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Exiting the program...");
                    break;
                default: 
                    Console.WriteLine("Invalid option. try again.");
                    break;
            }
        }
    }

    static void SeeAllCars(string conn)
    {
        List<Car> cars = new List<Car>();

        using (MySqlConnection cnx = new MySqlConnection(conn))
        {
            try
            {
                cnx.Open(); 

                string query = "SELECT * FROM car"; 
                MySqlCommand cmd = new MySqlCommand(query, cnx);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Car car = new Car
                        {
                            CarID = reader.GetInt32("CarID"),
                            Make = reader.GetString("Make"),
                            Model = reader.GetString("Model"),
                            Year = reader.GetInt32("Year"),
                            Price = reader.GetDecimal("Price"),
                            DateAdded = reader.GetDateTime("DateAdded")
                        };
                        cars.Add(car);
                    }
                }

                
                Console.WriteLine("\nAvailable cars:");
                foreach (var car in cars)
                {
                    Console.WriteLine(car);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void AddNewCar(string conn)
    {
        using (MySqlConnection cnx = new MySqlConnection(conn)) 
        {
            try
            {
                cnx.Open(); 

                
                Console.Write("Enter the make: ");
                string make = Console.ReadLine(); 

                Console.Write("Enter the model: ");
                string model = Console.ReadLine(); 

                Console.Write("Enter the year: ");
                int year = int.Parse(Console.ReadLine()); 

                Console.Write("Enter the price: ");
                decimal price = decimal.Parse(Console.ReadLine()); 

                DateTime dateAdded = DateTime.Now; 

               
                string query = "INSERT INTO car (Make, Model, Year, Price, DateAdded) VALUES (@Make, @Model, @Year, @Price, @DateAdded)";
                MySqlCommand cmd = new MySqlCommand(query, cnx); 

                
                cmd.Parameters.AddWithValue("@Make", make); 
                cmd.Parameters.AddWithValue("@Model", model); 
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Price", price); 
                cmd.Parameters.AddWithValue("@DateAdded", dateAdded); 

               
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Car added successfully."); 
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error: " + ex.Message); 
            }
        }

    }

    static void EditCar(string conn)
    {
        using (MySqlConnection cnx = new MySqlConnection(conn))
        {
            try
            {
                cnx.Open(); 

                Console.Write("Enter the ID of the car you want to edit: ");
                int carID = int.Parse(Console.ReadLine()); 

                Console.Write("Enter the new make: ");
                string make = Console.ReadLine(); 

                Console.Write("Enter the new model: ");
                string model = Console.ReadLine(); 

                Console.Write("Enter the new year: ");
                int year = int.Parse(Console.ReadLine()); 

                Console.Write("Enter the new price: ");
                decimal price = decimal.Parse(Console.ReadLine()); 

                string query = "UPDATE car SET Make=@Make, Model=@Model, Year=@Year, Price=@Price WHERE CarID=@CarID";
                MySqlCommand cmd = new MySqlCommand(query, cnx); 

                cmd.Parameters.AddWithValue("@Make", make); 
                cmd.Parameters.AddWithValue("@Model", model); 
                cmd.Parameters.AddWithValue("@Year", year); 
                cmd.Parameters.AddWithValue("@Price", price); 
                cmd.Parameters.AddWithValue("@CarID", carID); 

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Car updated successfully.");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error: " + ex.Message); 
            }

        }
    }
}
