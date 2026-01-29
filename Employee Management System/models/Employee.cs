using System;

class Employee
{
    private int id;
    private string name;
    private string department;
    private double salary;
    private string managerName;

    public Employee(int id, string name, string department, double salary, string managerName)
    {
        this.id = id;
        this.name = name;
        this.department = department;
        this.salary = salary;
        this.managerName = managerName;
    }

    public int Id
    {
        get { return id; }
    }

    public string Name
    {
        get { return name; }
    }

    public string ManagerName
    {
        get { return managerName; }
    }

    public string Department
    {
        get { return department; }
        set { department = value; }
    }

    public double Salary
    {
        get { return salary; }
        set { salary = value; }
    }

    public virtual void Display()
    {
        Console.WriteLine($"{id} | {name} | {department} | {salary} | Manager: {managerName}");
    }
}
