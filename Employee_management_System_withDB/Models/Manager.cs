using System.Linq;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Models
{
    class Manager : Employee, IHasTeamSize
    {
        public Manager(string name, string department, double salary)
            : base(name, department, salary, "N/A")
        {
            Role = RoleType.Manager;
        }

        public int GetTeamSize()
        {
            int teamLeads = EmployeeManagementService.TeamLeads
                .Count(t => t.ManagerName == Name);

            int employees = EmployeeManagementService.Employees.Count(e =>
                EmployeeManagementService.TeamLeads.Any(t =>
                    t.Name == e.TeamLeadName && t.ManagerName == Name));

            return teamLeads + employees;
        }

        public override void Display()
        {
            System.Console.WriteLine(
                $"[Manager] ID:{Id} | Name:{Name} | Dept:{Department} | Salary:{SalaryDetails.TotalSalary}");
        }
    }

}
