using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{


    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;
        private const string SqlSelect =
        "SELECT department_id, name " +
        "FROM department";

        private const string SqlInsert =
        "INSERT INTO department (department_id, name) " +
        "VALUES (@department_id, @name)" +
        "SELECT @@IDENTITY";


        private const string SqlUpdate =
        "UPDATE department" +
        "SET (@department_id, @name)" +


        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public ICollection<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelect, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Department dept = new Department
                        {
                            Id = Convert.ToInt32(reader["department_id"]),
                            Name = Convert.ToString(reader["name"])
                        };
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database " + ex.Message);
            }

            return departments;

        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment) //Insert
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsert, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    Department dept = new Department();
                    command.Parameters.AddWithValue("@department_id", dept.Id);
                    command.Parameters.AddWithValue("@name", dept.Name);

                    int id = Convert.ToInt32(command.ExecuteScalar());
                    return id;
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database " + ex.Message);
                return -1; // No department
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment) // Update
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlUpdate, conn);

                    Department dept = new Department();
                    command.Parameters.AddWithValue("@department_id", dept.Id);
                    command.Parameters.AddWithValue("@name", dept.Name);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database " + ex.Message);
            }
            return false;
        }
    }

}


