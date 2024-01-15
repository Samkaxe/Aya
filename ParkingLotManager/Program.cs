using System;
using System.Data.OleDb;

class Program
{
    static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ParkingLotDatabase.accdb;";

    static void Main(string[] args)
    {
        GenerateMockEntries(20);
        while (true)
        {
            Console.WriteLine("\nParking Lot Management System");
            Console.WriteLine("1. Add Car Entry");
            Console.WriteLine("2. Record Car Exit");
            Console.WriteLine("3. List All Cars");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddCarEntry();
                    break;
                case "2":
                    RecordCarExit();
                    break;
                case "3":
                    ListAllCars();
                    break;
                case "4":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }

    static void AddCarEntry()
    {
        Console.Write("Enter Car Number Plate: ");
        var numberPlate = Console.ReadLine();

        using (var connection = new OleDbConnection(connectionString))
        {
            var command = new OleDbCommand("INSERT INTO ParkingLot (NumberPlate, EntryTime) VALUES (?, ?)", connection);
            command.Parameters.Add(new OleDbParameter("NumberPlate", numberPlate));
            command.Parameters.Add(new OleDbParameter("EntryTime", OleDbType.Date) { Value = DateTime.Now });

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Car entry recorded.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void RecordCarExit()
    {
        Console.Write("Enter Car Number Plate for Exit: ");
        var numberPlate = Console.ReadLine();

        using (var connection = new OleDbConnection(connectionString))
        {
            var command = new OleDbCommand("UPDATE ParkingLot SET ExitTime = ? WHERE NumberPlate = ?", connection);
            command.Parameters.Add(new OleDbParameter("ExitTime", OleDbType.Date) { Value = DateTime.Now });
            command.Parameters.Add(new OleDbParameter("NumberPlate", numberPlate));

            try
            {
                connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows > 0)
                    Console.WriteLine("Car exit recorded.");
                else
                    Console.WriteLine("Car not found.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void ListAllCars()
    {
        using (var connection = new OleDbConnection(connectionString))
        {
            var command = new OleDbCommand("SELECT NumberPlate, EntryTime, ExitTime FROM ParkingLot", connection);
            connection.Open();
            var reader = command.ExecuteReader();
            Console.WriteLine("\nList of All Cars in Parking Lot:");
            while (reader.Read())
            {
                var numberPlate = reader["NumberPlate"].ToString();
                var entryTime = Convert.ToDateTime(reader["EntryTime"]);
                var exitTime = reader["ExitTime"] as DateTime?;
                var hoursParked = exitTime.HasValue ? (exitTime.Value - entryTime).TotalHours : 0;
                Console.WriteLine($"Number Plate: {numberPlate}, Entry Time: {entryTime}, Exit Time: {exitTime}, Hours Parked: {hoursParked:F2}");
            }
        }
    }
    
    static void GenerateMockEntries(int numberOfEntries)
    {
        Random random = new Random();
        for (int i = 1; i <= numberOfEntries; i++)
        {
            // Generate a mock number plate (e.g., "Mock1234")
            string numberPlate = $"Mock{i:D4}";

            // Generate a random entry time in the last 30 days
            DateTime entryTime = DateTime.Now.AddDays(-random.Next(30)).AddHours(-random.Next(24));

            AddMockCarEntry(numberPlate, entryTime);
        }

        Console.WriteLine($"{numberOfEntries} mock car entries added.");
    }

    static void AddMockCarEntry(string numberPlate, DateTime entryTime)
    {
        using (var connection = new OleDbConnection(connectionString))
        {
            var command = new OleDbCommand("INSERT INTO ParkingLot (NumberPlate, EntryTime) VALUES (?, ?)", connection);
            command.Parameters.Add(new OleDbParameter("NumberPlate", numberPlate));
            command.Parameters.Add(new OleDbParameter("EntryTime", OleDbType.Date) { Value = entryTime });

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}