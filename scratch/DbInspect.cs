using System;
using System.Data;
using Npgsql;

class Program
{
    static void Main()
    {
        string connString = "Host=localhost;Database=designabear_db;Username=postgres;Password=admin";
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        
        Console.WriteLine("--- Table: cart_items columns ---");
        using (var cmd = new NpgsqlCommand("SELECT column_name, data_type, is_nullable FROM information_schema.columns WHERE table_name = 'cart_items'", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader["column_name"]} | {reader["data_type"]} | {reader["is_nullable"]}");
            }
        }

        Console.WriteLine("\n--- Table: builds columns ---");
        using (var cmd = new NpgsqlCommand("SELECT column_name, data_type, is_nullable FROM information_schema.columns WHERE table_name = 'builds'", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader["column_name"]} | {reader["data_type"]} | {reader["is_nullable"]}");
            }
        }
    }
}
