using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll =
            "SELECT project_id, name, from_date, to_date " +
            "FROM project";

        private const string SqlAddProject =
            "INSERT INTO project (name, from_date, to_date)" +
            "VALUES (@name, @from_date, @to_date) " +
            "SELECT @@IDENTITY;";

        private const string SqlAddEmployee =
            "INSERT INTO project_employee (project_id, employee_id)" +
            "VALUES (@project_id, @employee_id)";


        private const string SqlRemoveEmployee =
            "DELETE FROM project_employee " +
            "WHERE employee_id = @employee_id AND project_id = @project_id";

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects()
        {
            List<Project> results = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Project pjt = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["project_id"]),
                            Name = Convert.ToString(reader["name"]),
                            StartDate = Convert.ToDateTime(reader["from_date"]),
                            EndDate = Convert.ToDateTime(reader["to_date"])
                        };

                        results.Add(pjt);
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
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlAddEmployee, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);

                    command.ExecuteNonQuery(); 
                }
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlRemoveEmployee, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlAddProject, conn);
                    command.Parameters.AddWithValue("@name", newProject.Name);
                    command.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    command.Parameters.AddWithValue("@to_date", newProject.EndDate);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return -1;
        }

    }
}
