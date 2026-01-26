using System.Collections.Generic;

class EmployeeManagementSystem
{
    List<Employee> employees = new List<Employee>();

    public void AddEmployee(Employee emp)
    {
        employees.Add(emp);
        Console.WriteLine("Employee added");
    }

    public void ShowEmployees()
    {
        foreach (Employee emp in employees)
        {
            emp.Display();
        }
    }

    public void UpdateSalary(int id, double salary)
    {
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

    public void RemoveEmployee(int id)
    {
        employees.RemoveAll(emp => emp.Id == id);
        Console.WriteLine("Employee removed");
    }
}
