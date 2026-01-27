class Manager : Employee
{
    public int TeamSize { get; set; }
    public string Level { get; set; }

    public Manager(int id, string name, string department, double salary,
                   int teamSize, string level)
        : base(id, name, department, salary)
    {
        TeamSize = teamSize;
        Level = level;
      
    }

    // Polymorphism
    public override void Display()
    {
        Console.WriteLine(
            "Manager -> " +
            Id + " | " +
            Name + " | " +
            Department + " | " +
            Salary + " | " +
            "Team Size: " + TeamSize + " | " +
            "Level: " + Level + " | " 
        );
    }

    public void Promote(string newLevel)
    {
        Level = newLevel;
        Console.WriteLine("Manager promoted to " + newLevel);
    }

    public void UpdateTeamSize(int newSize)
    {
        TeamSize = newSize;
        Console.WriteLine("Team size updated");
    }

    
}

