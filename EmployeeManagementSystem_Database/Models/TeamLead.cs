using System.Linq;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Models
{
    // Team Lead inherits from Employee and Interface 
    class TeamLead : Employee, IHasTeamSize
    {
        public string ManagerName { get; set; }

        public TeamLead(string name, string department, double salary, string managerName)
            : base(name, department, salary, "N/A")
        {
            ManagerName = managerName;
            Role = RoleType.TeamLead;
        }

        public int GetTeamSize()
        {
            return EmployeeManagementService.Employees
                .Count(e => e.TeamLeadName == Name);
        }

        public override void Display()
        {
            System.Console.WriteLine(
                $"[TeamLead] ID:{Id} | Name:{Name} | Dept:{Department} | Salary:{SalaryDetails.TotalSalary} | Manager:{ManagerName}");
        }
    }

}
