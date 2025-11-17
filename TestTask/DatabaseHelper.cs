using System;
using System.Collections.Generic;
using Npgsql;

namespace TestTask
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void TestConnection()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("✅ Подключение к базе данных успешно!");
        }

        public int AddEmployee(Employee employee)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"INSERT INTO ""Employees"" (""FirstName"", ""LastName"", ""Email"", ""DateOfBirth"", ""Salary"")
                               VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary)
                               RETURNING ""EmployeeID""";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Salary", employee.Salary);

            return (int)command.ExecuteScalar();
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"SELECT ""EmployeeID"", ""FirstName"", ""LastName"", ""Email"", ""DateOfBirth"", ""Salary""
                               FROM ""Employees"" ORDER BY ""EmployeeID""";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Salary = reader.GetDecimal(5)
                });
            }

            return employees;
        }

        public bool UpdateEmployee(Employee employee)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"UPDATE ""Employees"" 
                               SET ""FirstName"" = @FirstName, ""LastName"" = @LastName, 
                                   ""Email"" = @Email, ""DateOfBirth"" = @DateOfBirth, ""Salary"" = @Salary
                               WHERE ""EmployeeID"" = @EmployeeID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Salary", employee.Salary);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteEmployee(int employeeId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"DELETE FROM ""Employees"" WHERE ""EmployeeID"" = @EmployeeID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeId);

            return command.ExecuteNonQuery() > 0;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"SELECT ""EmployeeID"", ""FirstName"", ""LastName"", ""Email"", ""DateOfBirth"", ""Salary""
                               FROM ""Employees"" WHERE ""EmployeeID"" = @EmployeeID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Salary = reader.GetDecimal(5)
                };
            }

            return null;
        }

        public int GetEmployeesCountAboveAverageSalary()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string sql = @"SELECT COUNT(*) FROM ""Employees"" 
                               WHERE ""Salary"" > (SELECT AVG(""Salary"") FROM ""Employees"")";

            using var command = new NpgsqlCommand(sql, connection);
            var result = command.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }
    }
}

