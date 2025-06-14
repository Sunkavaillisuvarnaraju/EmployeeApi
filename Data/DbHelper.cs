using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeAPI.Models;

namespace EmployeeAPI.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            // Fix for CS8601: Ensure GetConnectionString does not return null by using null-coalescing operator  
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null.");
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("SELECT * FROM Employees", con);
            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Salary = Convert.ToDecimal(reader["Salary"])
                });
            }
            return employees;
        }

        public void AddEmployee(Employee emp)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("INSERT INTO Employees (Name, Salary) VALUES (@Name, @Salary)", con);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateEmployee(Employee emp)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("UPDATE Employees SET Name = @Name, Salary = @Salary WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", emp.Id);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public string DeleteEmployee(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Delete employee
                var deleteCmd = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", con);
                deleteCmd.Parameters.AddWithValue("@Id", id);
                int affected = deleteCmd.ExecuteNonQuery();

                if (affected == 0)
                    return "Not Found";

                // Reseed identity to current MAX(Id)
                var maxIdCmd = new SqlCommand("SELECT ISNULL(MAX(Id), 0) FROM Employees", con);
                int maxId = (int)maxIdCmd.ExecuteScalar();

                var reseedCmd = new SqlCommand($"DBCC CHECKIDENT ('Employees', RESEED, {maxId})", con);
                reseedCmd.ExecuteNonQuery();
            }

            return "Employee deleted and identity reseeded.";
        }
    
    }
}
