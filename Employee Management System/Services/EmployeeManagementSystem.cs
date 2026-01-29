using System;
using System.Collections.Generic;

class EmployeeManagementSystem
{
    private List<Employee> employees = new List<Employee>();
    private int nextId = 1;

    // INPUT VALIDATION
    private int ReadInt(string message)
    {
        int value;
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out value))
                return value;

            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    private double ReadDouble(string message)
    {
        double value;
        while (true)
        {
            Console.Write(message);
            if (double.TryParse(Console.ReadLine(), out value))
                return value;

            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private string ReadString(string message)
    {
        string input;
        while (true)
        {
            Console.Write(message);
            input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input) && !int.TryParse(input, out _))
                return input;

            Console.WriteLine("Invalid input. Please enter text only.");
        }
    }

    // Adding employee
    public void AddEmployee()
    {
        int id = nextId++;

        string name = ReadString("Name: ");
        string dept = ReadString("Department: ");
        double salary = ReadDouble("Salary: ");
        string managerName = ReadString("Manager Name : ");

        employees.Add(new Employee(id, name, dept, salary, managerName));
        Console.WriteLine($"Employee added with ID: {id} | NAME: {name}");
    }

    // Adding Manager
    public void AddManager()
    {
        int id = nextId++;

        string name = ReadString("Name: ");
        string dept = ReadString("Department: ");
        double salary = ReadDouble("Salary: ");
        string level = ReadString("Level (Junior/Senior/Lead): ");

        employees.Add(new Manager(id, name, dept, salary, level));
        Console.WriteLine($"Manager added with ID: {id} | NAME: {name}");
    }

    // Display all employees
    public void ShowEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found");
            return;
        }

        foreach (Employee emp in employees)
        {
            if (emp is Manager mgr)
            {
                int teamSize = CalculateTeamSize(mgr.Name);
                mgr.DisplayWithTeamSize(teamSize);
            }
            else
            {
                emp.Display();
            }
        }
    }

    // Total team size of each manager
    private int CalculateTeamSize(string managerName)
    {
        int count = 0;
        foreach (Employee emp in employees)
        {
            if (emp.ManagerName.Equals(managerName, StringComparison.OrdinalIgnoreCase))
                count++;
        }
        return count;
    }

    public void UpdateSalary()
    {
        int id = ReadInt("Enter Employee ID: ");
        double salary = ReadDouble("New Salary: ");

        foreach (Employee emp in employees)
        {
            if (emp.Id == id)
            {
                emp.Salary = salary;
                Console.WriteLine("Salary updated");
                return;
            }
        }
        Console.WriteLine("Employee not found");
    }

    public void RemoveEmployee()
    {
        int id = ReadInt("Enter Employee ID: ");
        int removed = employees.RemoveAll(e => e.Id == id);

        Console.WriteLine(removed > 0 ? "Employee removed" : "Employee not found");
    }

    public void TotalEmployees()
    {
        Console.WriteLine($"Total Employees: {employees.Count}");
    }
}
