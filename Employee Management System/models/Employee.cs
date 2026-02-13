using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    class Employee : IDisplayable
    {
        private static int _autoId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; }
        public string TeamLeadName { get; set; }

        public Employee(string name, string department, double salary, string teamLeadName)
        {
            Id = _autoId++;
            Name = name;
            Department = department;
            Salary = salary;
            TeamLeadName = teamLeadName;
        }

        public virtual void Display()
        {
            System.Console.WriteLine($"ID:{Id} | Name:{Name} | Dept:{Department} | Salary:{Salary} | TeamLead:{TeamLeadName}");
        }
    }
}
