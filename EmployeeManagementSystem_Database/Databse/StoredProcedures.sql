-- Creating procedure to Add Employee
CREATE PROCEDURE sp_AddEmployee
    @Id INT,
    @Name NVARCHAR(100),
    @Department NVARCHAR(100),
    @Salary FLOAT,
    @TeamLeadName NVARCHAR(100)
AS
BEGIN
    INSERT INTO Employees (Id, Name, Department, Salary, TeamLeadName)
    VALUES (@Id, @Name, @Department, @Salary, @TeamLeadName)
END
Go
-- Creating Procedure for updating salary
CREATE PROCEDURE sp_UpdateSalary
    @Id INT,
    @Salary FLOAT
AS
BEGIN
    UPDATE Employees SET Salary=@Salary WHERE Id=@Id
    UPDATE TeamLeads SET Salary=@Salary WHERE Id=@Id
    UPDATE Managers SET Salary=@Salary WHERE Id=@Id
END
