using System.Linq;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Models
{
    class TeamLead : Employee, IHasTeamSize
    {
        public string ManagerName { get; set; }

        public TeamLead(string name, string department, double salary, string managerName)
            : base(name, department, salary, "N/A")
        {
            ManagerName = managerName;
        }

        public int GetTeamSize()
        {
            return EmployeeManagementService.Employees.Count(e => e.TeamLeadName == Name);
        }

        public override void Display()
        {
            System.Console.WriteLine($"[TeamLead] ID:{Id} | Name:{Name} | Dept:{Department} | Salary:{Salary} | Manager:{ManagerName}");
        }
    }
}
