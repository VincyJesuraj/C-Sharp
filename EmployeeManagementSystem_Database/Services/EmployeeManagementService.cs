using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    static class EmployeeManagementService
    {
        private static readonly string connectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=EmployeeManagementDB;Trusted_Connection=True;";

        public static List<Employee> Employees = new();
        public static List<TeamLead> TeamLeads = new();
        public static List<Manager> Managers = new();

        // checks for the duplicate names 
        private static bool NameExists(string name)
        {
            if (Employees.Any(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                TeamLeads.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                Managers.Any(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return true;

            using SqlConnection con = new(connectionString);
            con.Open();

            string query = @"
                SELECT COUNT(*) FROM (
                    SELECT Name FROM Employees
                    UNION ALL
                    SELECT Name FROM TeamLeads
                    UNION ALL
                    SELECT Name FROM Managers
                ) X
                WHERE LOWER(Name) = LOWER(@Name)";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Name", name);

            return (int)cmd.ExecuteScalar() > 0;
        }


        public static void SyncAutoId()
        {
            using SqlConnection con = new(connectionString);
            con.Open();

            string query = @"
            SELECT ISNULL(MAX(Id),0) FROM (
                SELECT Id FROM Employees
                UNION ALL
                SELECT Id FROM TeamLeads
                UNION ALL
                SELECT Id FROM Managers
            ) X";

            SqlCommand cmd = new(query, con);
            int maxId = (int)cmd.ExecuteScalar();

            typeof(Employee)
                .GetField("_autoId",
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, maxId + 1);
        }

        public static void LoadAllData()
        {
            using SqlConnection con = new(connectionString);
            con.Open();

            SqlCommand cmd;

            cmd = new SqlCommand("SELECT * FROM Managers", con);
            using (SqlDataReader r = cmd.ExecuteReader())
                while (r.Read())
                    Managers.Add(new Manager(
                        r["Name"].ToString(),
                        r["Department"].ToString(),
                        (double)r["Salary"]));

            cmd = new SqlCommand("SELECT * FROM TeamLeads", con);
            using (SqlDataReader r = cmd.ExecuteReader())
                while (r.Read())
                    TeamLeads.Add(new TeamLead(
                        r["Name"].ToString(),
                        r["Department"].ToString(),
                        (double)r["Salary"],
                        r["ManagerName"].ToString()));

            cmd = new SqlCommand("SELECT * FROM Employees", con);
            using (SqlDataReader r = cmd.ExecuteReader())
                while (r.Read())
                    Employees.Add(new Employee(
                        r["Name"].ToString(),
                        r["Department"].ToString(),
                        (double)r["Salary"],
                        r["TeamLeadName"].ToString()));
        }

        // Adding Manager
        public static void AddManager()
        {
            string name;
            while (true)
            {
                name = ReadString("Name: ");
                if (!NameExists(name)) break;
                Console.WriteLine("Already exists. Enter another name.");
            }

            Manager m = new Manager(
                name,
                ReadString("Department: "),
                ReadDouble("Salary: "));

            Managers.Add(m);

            using SqlConnection con = new(connectionString);
            SqlCommand cmd = new SqlCommand("sp_AddManager", con);
            cmd.CommandType = CommandType.StoredProcedure; 

            cmd.Parameters.AddWithValue("@Id", m.Id);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Dept", m.Department);
            cmd.Parameters.AddWithValue("@Salary", m.Salary);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Manager added successfully");
        }

        // Adding Team Lead
        public static void AddTeamLead()
        {
            string name;
            while (true)
            {
                name = ReadString("Name: ");
                if (!NameExists(name)) break;
                Console.WriteLine("Already exists. Enter another name.");
            }

            TeamLead t = new TeamLead(
                name,
                ReadString("Department: "),
                ReadDouble("Salary: "),
                ReadString("Manager Name: "));

            TeamLeads.Add(t);

            using SqlConnection con = new(connectionString);
            SqlCommand cmd = new SqlCommand("sp_AddTeamLead", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", t.Id);
            cmd.Parameters.AddWithValue("@Name", t.Name);
            cmd.Parameters.AddWithValue("@Dept", t.Department);
            cmd.Parameters.AddWithValue("@Salary", t.Salary);
            cmd.Parameters.AddWithValue("@Mgr", t.ManagerName);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Team Lead added successfully");
        }

        //Adding Employee
        public static void AddEmployee()
        {
            string name;
            while (true)
            {
                name = ReadString("Name: ");
                if (!NameExists(name)) break;
                Console.WriteLine("Already exists. Enter another name.");
            }

            Employee e = new Employee(
                name,
                ReadString("Department: "),
                ReadDouble("Salary: "),
                ReadString("Team Lead Name: "));

            Employees.Add(e);

            using SqlConnection con = new(connectionString);
            SqlCommand cmd = new SqlCommand("sp_AddEmployee", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Name", e.Name);
            cmd.Parameters.AddWithValue("@Department", e.Department);
            cmd.Parameters.AddWithValue("@Salary", e.Salary);
            cmd.Parameters.AddWithValue("@TeamLeadName", e.TeamLeadName);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Employee added successfully");
        }

        // Updating Salary
        public static void UpdateSalary()
        {
            int id = ReadInt("Enter ID: ");
            double salary = ReadDouble("New Salary: ");

            using SqlConnection con = new(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("sp_UpdateSalary", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Salary", salary);

            cmd.ExecuteNonQuery();

            Console.WriteLine("Salary updated");
        }

        // To find number of employees under Team Lead
        public static void TeamSizeTeamLead()
        {
            string name = ReadString("Team Lead Name: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_GetTeamSize_TeamLead", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TeamLeadName", name);

            object result = cmd.ExecuteScalar();

            Console.WriteLine($"Team size = {result}");
        }


        // Team size of the manager = no. of employees + no. of Team Lead
        public static void TeamSizeManager()
        {
            string name = ReadString("Manager Name: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_GetTeamSize_Manager", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ManagerName", name);

            object result = cmd.ExecuteScalar();

            Console.WriteLine($"Team size = {result}");
        }


        // Showing Employees under Manager
        public static void ShowEmployeesUnderManager()
        {
            string manager = ReadString("Manager Name: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_GetEmployeesUnderManager", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ManagerName", manager);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Not found.");
                return;
            }

            while (reader.Read())
            {
                Console.WriteLine(
                    $"Name:{reader["Name"]} | Dept:{reader["Department"]} | Salary:{reader["Salary"]} | Role:{reader["Role"]}");
            }
        }


        // Showing Employees by Department
        public static void ShowEmployeesByDepartment()
        {
            string dept = ReadString("Department: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_GetEmployeesByDepartment", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Department", dept);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Not found.");
                return;
            }

            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID:{reader["Id"]} | Name:{reader["Name"]} | Dept:{reader["Department"]} | Salary:{reader["Salary"]} | Role:{reader["Role"]}");
            }
        }


        // Deleting Manager 
        public static void DeleteManagerByName()
        {
            string name = ReadString("Manager Name to delete: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand check = new(
                "SELECT COUNT(*) FROM Managers WHERE Name=@Name", conn);
            check.Parameters.AddWithValue("@Name", name);

            if ((int)check.ExecuteScalar() == 0)
            {
                Console.WriteLine("Manager not found.");
                return;
            }


            SqlCommand cmd = new SqlCommand("sp_DeleteManager", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);

            int rows = cmd.ExecuteNonQuery();


            Managers.RemoveAll(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            TeamLeads.Where(t => t.ManagerName == name)
                     .ToList()
                     .ForEach(t => t.ManagerName = null);

            ReSyncAutoId(conn);
            Console.WriteLine("Manager deleted successfully.");
        }

        // Deleting Team Lead
        public static void DeleteTeamLeadByName()
        {
            string name = ReadString("Team Lead Name to delete: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand check = new(
                "SELECT COUNT(*) FROM TeamLeads WHERE Name=@Name", conn);
            check.Parameters.AddWithValue("@Name", name);

            if ((int)check.ExecuteScalar() == 0)
            {
                Console.WriteLine("Team Lead not found.");
                return;
            }

            // Clear teamlead reference in SQL
            SqlCommand cmd = new SqlCommand("sp_DeleteTeamLead", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);

            int rows = cmd.ExecuteNonQuery();

            // REMOVE FROM MEMORY
            TeamLeads.RemoveAll(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Employees.Where(e => e.TeamLeadName == name)
                     .ToList()
                     .ForEach(e => e.TeamLeadName = null);

            ReSyncAutoId(conn);
            Console.WriteLine("Team Lead deleted successfully.");
        }

        // Deleting Employee
        public static void DeleteEmployeeByName()
        {
            string name = ReadString("Employee Name to delete: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_DeleteEmployee", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            // REMOVE FROM MEMORY
            Employees.RemoveAll(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            ReSyncAutoId(conn);
            Console.WriteLine("Employee deleted successfully.");
        }




        private static void ReSyncAutoId(SqlConnection conn)
        {
            string query = @"
                SELECT MAX(Id) FROM (
                    SELECT Id FROM Managers
                    UNION ALL
                    SELECT Id FROM TeamLeads
                    UNION ALL
                    SELECT Id FROM Employees
                ) AS AllIds";

            SqlCommand cmd = new(query, conn);
            object result = cmd.ExecuteScalar();

            int nextId = (result == DBNull.Value || result == null)
                ? 1
                : Convert.ToInt32(result) + 1;

            Employee.SyncAutoId(nextId);
        }


        // Input Validation 
        public static int ReadInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (int.TryParse(Console.ReadLine(), out int v))
                    return v;
                Console.WriteLine("Invalid number");
            }
        }

        public static double ReadDouble(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (double.TryParse(Console.ReadLine(), out double v))
                    return v;
                Console.WriteLine("Invalid number");
            }
        }

        public static string ReadString(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                string s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s) && !double.TryParse(s, out _))
                    return s;
                Console.WriteLine("Invalid text");
            }
        }
    }
}
