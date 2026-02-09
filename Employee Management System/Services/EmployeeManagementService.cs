using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Services
{
    static class EmployeeManagementService
    {
        public static List<Employee> Employees = new();
        public static List<TeamLead> TeamLeads = new();
        public static List<Manager> Managers = new();

        public static void AddManager()
        {
            Managers.Add(new Manager(
                ReadString("Name: "),
                ReadString("Department: "),
                ReadDouble("Salary: ")
            ));
            Console.WriteLine("Manager added successfully");
        }

        public static void AddTeamLead()
        {
            TeamLeads.Add(new TeamLead(
                ReadString("Name: "),
                ReadString("Department: "),
                ReadDouble("Salary: "),
                ReadString("Manager Name: ")
            ));
            Console.WriteLine("Team Lead added successfully");
        }

        public static void AddEmployee()
        {
            Employees.Add(new Employee(
                ReadString("Name: "),
                ReadString("Department: "),
                ReadDouble("Salary: "),
                ReadString("Team Lead Name: ")
            ));
            Console.WriteLine("Employee added successfully");
        }

        public static void UpdateSalary()
        {
            int id = ReadInt("Enter ID: ");
            double salary = ReadDouble("New Salary: ");

            var person = Employees.Cast<Employee>()
                .Concat(TeamLeads)
                .Concat(Managers)
                .FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                Console.WriteLine("Not found. Please enter another ID.");
                return;
            }

            person.Salary = salary;
            Console.WriteLine("Salary updated");
        }

        public static void TeamSizeTeamLead()
        {
            string name = ReadString("Team Lead Name: ");
            var tl = TeamLeads.FirstOrDefault(t => t.Name == name);

            if (tl == null)
            {
                Console.WriteLine("Not found. Please enter another Team Lead name.");
                return;
            }

            Console.WriteLine($"Team size = {tl.GetTeamSize()}");
        }

        public static void TeamSizeManager()
        {
            string name = ReadString("Manager Name: ");
            var mgr = Managers.FirstOrDefault(m => m.Name == name);

            if (mgr == null)
            {
                Console.WriteLine("Not found. Please enter another Manager name.");
                return;
            }

            Console.WriteLine($"Team size = {mgr.GetTeamSize()}");
        }

        public static void ShowEmployeesUnderManager()
        {
            string manager = ReadString("Manager Name: ");
            var teamLeads = TeamLeads.Where(t => t.ManagerName == manager).ToList();

            if (!teamLeads.Any())
            {
                Console.WriteLine("Not found. Please enter another Manager name.");
                return;
            }

            Console.WriteLine("\nTeam Leads:");
            teamLeads.ForEach(t => t.Display());

            Console.WriteLine("\nEmployees:");
            Employees.Where(e =>
                teamLeads.Any(t => t.Name == e.TeamLeadName))
                .ToList()
                .ForEach(e => e.Display());
        }

        public static void ShowEmployeesByDepartment()
        {
            string dept = ReadString("Department: ");
            var results = Employees.Cast<IDisplayable>()
                .Concat(TeamLeads)
                .Concat(Managers)
                .Where(p => (p as Employee).Department == dept)
                .ToList();

            if (!results.Any())
            {
                Console.WriteLine("Not found. Please enter another Department.");
                return;
            }

            results.ForEach(r => r.Display());
        }

        // -------------- Input helpers ----------------
        public static int ReadInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Invalid input. Enter a valid number.");
            }
        }

        public static double ReadDouble(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (double.TryParse(Console.ReadLine(), out double value))
                    return value;

                Console.WriteLine("Invalid input. Enter a valid number.");
            }
        }

        public static string ReadString(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && !double.TryParse(input, out _))
                    return input;

                Console.WriteLine("Invalid input. Enter text only.");
            }
        }
    }
}
