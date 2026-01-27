using System;

class Employee
{
    //encapsulation
    private int id;
    private string name;
    private string department;
    private double salary;

    //Constructor
    public Employee(int id, string name, string department, double salary)
    {
        this.id = id;
        this.name = name;
        this.department = department;
        this.salary = salary;
    }

    //properties(getter and setter) 
    public int Id { get { return id; } }
    public string Name { get { return name; } }
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

    //can be overridden by child class
    public virtual void Display()
    {
        Console.WriteLine(id + " | " + name + " | " + department + " | " + salary);
    }
}
