CREATE TABLE dbo.Userlogin
(
    UserId NVARCHAR(255) PRIMARY KEY, 
    Password NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    DesignationId INT NULL,

    IsLocked BIT NOT NULL DEFAULT(0),
    IsLoggedIn BIT NOT NULL DEFAULT(0),

    ProfilePicture NVARCHAR(500) NULL,
    
    CreatedBy NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE()),

    UpdatedBy NVARCHAR(255) NULL,
    UpdatedDate DATETIME2 NULL
);
GO
