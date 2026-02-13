-- Creating Database

CREATE DATABASE EmployeeManagementDB;

USE EmployeeManagementDB;

-- creating table - Managers
CREATE TABLE Managers (
    Id INT PRIMARY KEY,
    Name NVARCHAR(50),
    Department NVARCHAR(50),
    Salary FLOAT
);

-- Creating table - TeamLeads
CREATE TABLE TeamLeads (
    Id INT PRIMARY KEY,
    Name NVARCHAR(50),
    Department NVARCHAR(50),
    Salary FLOAT,
    ManagerName NVARCHAR(50)
);

-- Creating table - Employees
CREATE TABLE Employees (
    Id INT PRIMARY KEY,
    Name NVARCHAR(50),
    Department NVARCHAR(50),
    Salary FLOAT,
    TeamLeadName NVARCHAR(50)
);

-- Giving Unique constraints for Name
ALTER TABLE Managers
ADD CONSTRAINT UQ_Managers_Name UNIQUE (Name);

ALTER TABLE TeamLeads
ADD CONSTRAINT UQ_TeamLeads_Name UNIQUE (Name);

ALTER TABLE Employees
ADD CONSTRAINT UQ_Employees_Name UNIQUE (Name);

-- Allowing NULL references

ALTER TABLE TeamLeads
ALTER COLUMN ManagerName NVARCHAR(100) NULL;

ALTER TABLE Employees
ALTER COLUMN TeamLeadName NVARCHAR(100) NULL;

-- Adding FOREIGN KEYS with ON DELETE SET NULL

-- TeamLead → Manager
ALTER TABLE TeamLeads
ADD CONSTRAINT FK_TeamLeads_Manager
FOREIGN KEY (ManagerName)
REFERENCES Managers(Name)
ON DELETE SET NULL;

-- Employee → TeamLead
ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_TeamLead
FOREIGN KEY (TeamLeadName)
REFERENCES TeamLeads(Name)
ON DELETE SET NULL;
