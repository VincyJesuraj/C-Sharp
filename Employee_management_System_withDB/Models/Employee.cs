using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    public enum RoleType
    {
        Employee = 1,
        TeamLead,
        Manager
    }

    public struct SalaryInfo
    {
        public double BaseSalary { get; set; }
        public double Bonus { get; set; }

        public double TotalSalary => BaseSalary + Bonus;

        public SalaryInfo(double baseSalary, double bonus)
        {
            BaseSalary = baseSalary;
            Bonus = bonus;
        }
    }
    class Employee : IDisplayable
    {
        private static int _autoId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Department { get; set; }

        
        public double Salary { get; set; }

        
        public SalaryInfo SalaryDetails { get; set; }

        public string TeamLeadName { get; set; }

        
        public RoleType Role { get; set; }

        public Employee(string name, string department, double salary, string teamLeadName)
        {
            Id = _autoId++;
            Name = name;
            Department = department;
            Salary = salary;

            // Initialize struct
            SalaryDetails = new SalaryInfo(salary, 0);

            TeamLeadName = teamLeadName;

            Role = RoleType.Employee;
        }

        public static void SyncAutoId(int nextId)
        {
            _autoId = nextId;
        }

        public virtual void Display()
        {
            System.Console.WriteLine(
                $"ID:{Id} | Name:{Name} | Dept:{Department} | Salary:{SalaryDetails.TotalSalary} | TeamLead:{TeamLeadName}");
        }
    }
}
