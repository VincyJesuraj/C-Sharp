using System;

class Program
{
    static void Main()
    {
        EmployeeManagementSystem ems = new EmployeeManagementSystem();
        int choice;

        do
        {
            Console.WriteLine("\n---- EMPLOYEE MANAGEMENT SYSTEM ----");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Add Manager");
            Console.WriteLine("3. Show Employees");
            Console.WriteLine("4. Update Salary");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Show Total Employees");
            Console.WriteLine("7. Exit");

            Console.Write("Enter choice: ");
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice");
                continue;
            }

            switch (choice)
            {
                case 1: 
                    ems.AddEmployee(); 
                    break;
                case 2: 
                    ems.AddManager(); 
                    break;
                case 3: 
                    ems.ShowEmployees(); 
                    break;
                case 4: 
                    ems.UpdateSalary(); 
                    break;
                case 5: 
                    ems.RemoveEmployee(); 
                    break;
                case 6: 
                    ems.TotalEmployees(); 
                    break;
                case 7: 
                    Console.WriteLine("Exiting program..."); 
                    break;
                default: Console.WriteLine("Invalid choice"); 
                    break;
            }

        } while (choice != 7);
    }
}
