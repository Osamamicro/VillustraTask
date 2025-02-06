-- CreateUserloginTable.sql

USE VillustraTask;
GO

IF OBJECT_ID('dbo.Userlogin', 'U') IS NOT NULL
    DROP TABLE dbo.Userlogin;
GO

CREATE TABLE dbo.Userlogin
(
    UserId NVARCHAR(255) PRIMARY KEY, -- Email address as primary key
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    DesignationId INT,
    Islocked BIT NOT NULL DEFAULT(0),
    IsLoggedIn BIT NOT NULL DEFAULT(0),
    ProfilePicture NVARCHAR(500), -- Assuming path to profile picture
    CreatedBy NVARCHAR(255),
    CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
    UpdatedBy NVARCHAR(255),
    UpdatedDate DATETIME NULL
);
GO
