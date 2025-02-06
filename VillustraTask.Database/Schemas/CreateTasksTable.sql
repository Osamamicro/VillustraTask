-- CreateTasksTable.sql

USE VillustraTask;
GO

IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL
    DROP TABLE dbo.Tasks;
GO

CREATE TABLE dbo.Tasks
(
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    TaskName NVARCHAR(255) NOT NULL,
    TaskDescription NVARCHAR(MAX),
    TaskFile NVARCHAR(500), -- File path
    AssignedTo NVARCHAR(255) NOT NULL, -- Reference to Userlogin.UserId (email)
    TaskStatus NVARCHAR(100),
    TaskPriority NVARCHAR(100),
    CreatedBy NVARCHAR(255),
    CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
    UpdatedBy NVARCHAR(255),
    UpdatedDate DATETIME NULL
);
GO
