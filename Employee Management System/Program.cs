// Employee Management System - Week 1 & 2 Assessment

using System;

class Program
{
    static void Main()
    {
        EmployeeManagementSystem ems = new EmployeeManagementSystem();
        int choice;

        do
        {
            Console.WriteLine("\n1. Add Employee");
            Console.WriteLine("2. Add Manager");
            Console.WriteLine("3. Show Employees");
            Console.WriteLine("4. Update Salary");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Search Employee by Name");
            Console.WriteLine("7. Show Total Employees");
            Console.WriteLine("8. Show Employees by Department");
            Console.WriteLine("9. Exit");
            Console.Write("Enter choice: ");

            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Id: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Department: ");
                    string dept = Console.ReadLine();
                    Console.Write("Salary: ");
                    double sal = double.Parse(Console.ReadLine());

                    ems.AddEmployee(new Employee(id, name, dept, sal));
                    break;

                case 2:
                    Console.Write("Id: ");
                    int mid = int.Parse(Console.ReadLine());
                    Console.Write("Name: ");
                    string mname = Console.ReadLine();
                    Console.Write("Department: ");
                    string mdept = Console.ReadLine();
                    Console.Write("Salary: ");
                    double msal = double.Parse(Console.ReadLine());
                    Console.Write("Team Size: ");
                    int teamSize = int.Parse(Console.ReadLine());
                    Console.Write("Level (Junior/Senior/Lead): ");
                    string level = Console.ReadLine();

                    Manager mgr = new Manager(mid, mname, mdept, msal, teamSize, level);
                    ems.AddEmployee(mgr);
                    break;


                case 3:
                    ems.ShowEmployees();
                    break;

                case 4:
                    Console.Write("Enter Id: ");
                    int uid = int.Parse(Console.ReadLine());
                    Console.Write("New Salary: ");
                    double nsal = double.Parse(Console.ReadLine());
                    ems.UpdateSalary(uid, nsal);
                    break;

                case 5:
                    Console.Write("Enter Id: ");
                    int did = int.Parse(Console.ReadLine());
                    ems.RemoveEmployee(did);
                    break;

                case 6:
                    Console.Write("Enter Name: ");
                    ems.SearchByName(Console.ReadLine());
                    break;

                case 7:
                    ems.TotalEmployees();
                    break;

                case 8:
                    Console.Write("Enter Department: ");
                    ems.ShowByDepartment(Console.ReadLine());
                    break;
            }

        } while (choice != 9);
    }
}
