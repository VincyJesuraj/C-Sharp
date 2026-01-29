using System;

class Manager : Employee
{
    public string Level { get; set; }

    public Manager(int id, string name, string department, double salary, string level)
        : base(id, name, department, salary, "None")
    {
        Level = level;
    }

    public void DisplayWithTeamSize(int teamSize)
    {
        Console.WriteLine(
            $"Manager -> {Id} | {Name} | {Department} | {Salary} | Level: {Level} | Team Size: {teamSize}"
        );
    }
}
