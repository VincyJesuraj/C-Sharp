-- Creating procedure to Add Manager
CREATE PROCEDURE sp_AddManager
    @Id INT,
    @Name NVARCHAR(100),
    @Department NVARCHAR(100),
    @Salary FLOAT
AS
BEGIN
    INSERT INTO Managers (Id, Name, Department, Salary)
    VALUES (@Id, @Name, @Department, @Salary)
END
GO

-- Creating procedure to Add TeamLead
CREATE PROCEDURE sp_AddTeamLead
    @Id INT,
    @Name NVARCHAR(100),
    @Department NVARCHAR(100),
    @Salary FLOAT,
    @ManagerName NVARCHAR(100)
AS
BEGIN
    INSERT INTO TeamLeads (Id, Name, Department, Salary, ManagerName)
    VALUES (@Id, @Name, @Department, @Salary, @ManagerName)
END
GO


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
GO

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
GO

-- Creating procedure to delete Manager
CREATE OR ALTER PROCEDURE sp_DeleteManager
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Managers WHERE Name = @Name)
    BEGIN
        PRINT 'Manager not found';
        RETURN;
    END

    UPDATE TeamLeads
    SET ManagerName = NULL
    WHERE ManagerName = @Name;

    DELETE FROM Managers
    WHERE Name = @Name;
END
GO


-- Creating procedure to delete TeamLead
CREATE PROCEDURE sp_DeleteTeamLead
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM TeamLeads WHERE Name = @Name)
    BEGIN
        PRINT 'Team Lead not found';
        RETURN;
    END

    BEGIN TRANSACTION;

    UPDATE Employees
    SET TeamLeadName = NULL
    WHERE TeamLeadName = @Name;

    DELETE FROM TeamLeads
    WHERE Name = @Name;

    COMMIT TRANSACTION;

    PRINT 'Team Lead deleted successfully';
END
GO

-- Creating procedure to delete Employee
CREATE OR ALTER PROCEDURE sp_DeleteEmployee
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Employees WHERE Name = @Name)
    BEGIN
        PRINT 'Employee not found';
        RETURN;
    END

    DELETE FROM Employees
    WHERE Name = @Name;

    PRINT 'Employee deleted successfully';
END
GO

-- Creating procedure to show all the employees under the manager
CREATE OR ALTER PROCEDURE sp_GetEmployeesUnderManager
    @ManagerName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Name, Department, Salary, 'TeamLead' AS Role
    FROM TeamLeads
    WHERE ManagerName = @ManagerName

    UNION ALL

    SELECT E.Name, E.Department, E.Salary, 'Employee' AS Role
    FROM Employees E
    INNER JOIN TeamLeads T 
        ON E.TeamLeadName = T.Name
    WHERE T.ManagerName = @ManagerName;
END
GO

-- Creating procedure to show all the employees under the department
CREATE OR ALTER PROCEDURE sp_GetEmployeesByDepartment
    @Department NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Department, Salary, 'Employee' AS Role
    FROM Employees
    WHERE Department = @Department

    UNION ALL

    SELECT Id, Name, Department, Salary, 'TeamLead' AS Role
    FROM TeamLeads
    WHERE Department = @Department

    UNION ALL

    SELECT Id, Name, Department, Salary, 'Manager' AS Role
    FROM Managers
    WHERE Department = @Department;
END
GO

-- Team size of the teamlead
CREATE OR ALTER PROCEDURE sp_GetTeamSize_TeamLead
    @TeamLeadName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TeamSize
    FROM Employees
    WHERE TeamLeadName = @TeamLeadName;
END
GO

-- Team Size of Manager
CREATE OR ALTER PROCEDURE sp_GetTeamSize_Manager
    @ManagerName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        (SELECT COUNT(*) 
         FROM TeamLeads 
         WHERE ManagerName = @ManagerName)
        +
        (SELECT COUNT(*) 
         FROM Employees E
         INNER JOIN TeamLeads T 
            ON E.TeamLeadName = T.Name
         WHERE T.ManagerName = @ManagerName)
        AS TeamSize;
END
GO



