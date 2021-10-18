using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll =
            "SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date " +
            "FROM employee";

        private const string SqlSelectEmployeeByFullName =
            "SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date " +
            "FROM employee " +
            "WHERE first_name = @first_name AND last_name = @last_name";

        private const string SqlSelectEmployeesNotAssignedToProjects =
           "SELECT * FROM employee " +
            "WHERE NOT EXISTS " +
            "(SELECT employee_id FROM project_employee prem WHERE prem.employee_id = employee.employee_id)";

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["department_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return results;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public ICollection<Employee> Search(string firstname, string lastname)
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectEmployeeByFullName, conn);

                    command.Parameters.AddWithValue("@first_name", firstname);
                    command.Parameters.AddWithValue("@last_name", lastname);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["department_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return results;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectEmployeesNotAssignedToProjects, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["department_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return results;
        }

    }
}
