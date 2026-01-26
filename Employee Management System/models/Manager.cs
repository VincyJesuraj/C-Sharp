class Manager : Employee
{
    public Manager(int id, string name, string department, double salary)
        : base(id, name, department, salary)
    {
    }

    public override void Display()
    {
        Console.WriteLine("Manager -> " + Id + " | " + Name + " | " + Department + " | " + Salary);
    }
}
