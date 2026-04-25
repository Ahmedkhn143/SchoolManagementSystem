using System;
using System.Data.SqlClient; // Yeh line lazmi add karni hai DB ke liye

namespace SchoolManagementSystem
{
    public class DatabaseHelper
    {
        // Yahan apna SSMS wala Server Name daalein jahan 'YOUR_SERVER_NAME' likha hai
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;";

        // Yeh function connection open karne ke liye use hoga
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}