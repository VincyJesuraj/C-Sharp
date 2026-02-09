using System;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n----- EMPLOYEE MANAGEMENT SYSTEM -----");
                Console.WriteLine("1. Add Manager");
                Console.WriteLine("2. Add Team Lead");
                Console.WriteLine("3. Add Employee");
                Console.WriteLine("4. Update Salary");
                Console.WriteLine("5. Team Size of Team Lead");
                Console.WriteLine("6. Team Size of Manager");
                Console.WriteLine("7. Show Employees under Manager");
                Console.WriteLine("8. Show Employees by Department");
                Console.WriteLine("9. Exit");

                int choice = EmployeeManagementService.ReadInt("Enter choice: ");

                switch (choice)
                {
                    case 1: 
                        EmployeeManagementService.AddManager(); 
                        break;
                    case 2: 
                        EmployeeManagementService.AddTeamLead(); 
                        break;
                    case 3: 
                        EmployeeManagementService.AddEmployee(); 
                        break;
                    case 4: 
                        EmployeeManagementService.UpdateSalary(); 
                        break;
                    case 5: 
                        EmployeeManagementService.TeamSizeTeamLead(); 
                        break;
                    case 6: 
                        EmployeeManagementService.TeamSizeManager(); 
                        break;
                    case 7: 
                        EmployeeManagementService.ShowEmployeesUnderManager(); 
                        break;
                    case 8: 
                        EmployeeManagementService.ShowEmployeesByDepartment(); 
                        break;
                    case 9: 
                        return;
                    default: Console.WriteLine("Invalid choice"); 
                        break;
                }
            }
        }
    }
}
