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
            SqlCommand cmd = new(
                "INSERT INTO Managers VALUES (@Id,@Name,@Dept,@Salary)", con);

            cmd.Parameters.AddWithValue("@Id", m.Id);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Dept", m.Department);
            cmd.Parameters.AddWithValue("@Salary", m.Salary);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Manager added successfully");
        }

        
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
            SqlCommand cmd = new(
                "INSERT INTO TeamLeads VALUES (@Id,@Name,@Dept,@Salary,@Mgr)", con);

            cmd.Parameters.AddWithValue("@Id", t.Id);
            cmd.Parameters.AddWithValue("@Name", t.Name);
            cmd.Parameters.AddWithValue("@Dept", t.Department);
            cmd.Parameters.AddWithValue("@Salary", t.Salary);
            cmd.Parameters.AddWithValue("@Mgr", t.ManagerName);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Team Lead added successfully");
        }

        
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

        
        public static void TeamSizeTeamLead()
        {
            string name = ReadString("Team Lead Name: ");
            var tl = TeamLeads.FirstOrDefault(t => t.Name == name);

            if (tl == null)
            {
                Console.WriteLine("Not found. Enter another name.");
                return;
            }

            Console.WriteLine($"Team size = {tl.GetTeamSize()}");
        }

        
        public static void TeamSizeManager()
        {
            string name = ReadString("Manager Name: ");
            var m = Managers.FirstOrDefault(x => x.Name == name);

            if (m == null)
            {
                Console.WriteLine("Not found. Enter another name.");
                return;
            }

            Console.WriteLine($"Team size = {m.GetTeamSize()}");
        }

       
        public static void ShowEmployeesUnderManager()
        {
            string manager = ReadString("Manager Name: ");
            var tls = TeamLeads.Where(t => t.ManagerName == manager).ToList();

            if (!tls.Any())
            {
                Console.WriteLine("Not found.");
                return;
            }

            tls.ForEach(t => t.Display());

            Employees.Where(e => tls.Any(t => t.Name == e.TeamLeadName))
                     .ToList()
                     .ForEach(e => e.Display());
        }

        
        public static void ShowEmployeesByDepartment()
        {
            string dept = ReadString("Department: ");

            var result = Employees.Cast<IDisplayable>()
                .Concat(TeamLeads)
                .Concat(Managers)
                .Where(x => (x as Employee).Department == dept)
                .ToList();

            if (!result.Any())
            {
                Console.WriteLine("Not found.");
                return;
            }

            result.ForEach(x => x.Display());
        }


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

           
            SqlCommand clearRefs = new(
                "UPDATE TeamLeads SET ManagerName = NULL WHERE ManagerName=@Name", conn);
            clearRefs.Parameters.AddWithValue("@Name", name);
            clearRefs.ExecuteNonQuery();

            
            SqlCommand delete = new(
                "DELETE FROM Managers WHERE Name=@Name", conn);
            delete.Parameters.AddWithValue("@Name", name);
            delete.ExecuteNonQuery();

            
            Managers.RemoveAll(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            TeamLeads.Where(t => t.ManagerName == name)
                     .ToList()
                     .ForEach(t => t.ManagerName = null);

            ReSyncAutoId(conn);
            Console.WriteLine("Manager deleted successfully.");
        }

        
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

            
            SqlCommand clearRefs = new(
                "UPDATE Employees SET TeamLeadName = NULL WHERE TeamLeadName=@Name", conn);
            clearRefs.Parameters.AddWithValue("@Name", name);
            clearRefs.ExecuteNonQuery();

            
            SqlCommand delete = new(
                "DELETE FROM TeamLeads WHERE Name=@Name", conn);
            delete.Parameters.AddWithValue("@Name", name);
            delete.ExecuteNonQuery();


            TeamLeads.RemoveAll(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Employees.Where(e => e.TeamLeadName == name)
                     .ToList()
                     .ForEach(e => e.TeamLeadName = null);

            ReSyncAutoId(conn);
            Console.WriteLine("Team Lead deleted successfully.");
        }

        
        public static void DeleteEmployeeByName()
        {
            string name = ReadString("Employee Name to delete: ");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            SqlCommand delete = new(
                "DELETE FROM Employees WHERE Name=@Name", conn);
            delete.Parameters.AddWithValue("@Name", name);

            int rows = delete.ExecuteNonQuery();

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
