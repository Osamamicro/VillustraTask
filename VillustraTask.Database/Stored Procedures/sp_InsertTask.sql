USE VillustraTask;
GO

IF OBJECT_ID('dbo.sp_InsertTask', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_InsertTask;
GO

CREATE PROCEDURE dbo.sp_InsertTask
    @TaskName NVARCHAR(255),
    @TaskDescription NVARCHAR(MAX),
    @TaskFile NVARCHAR(500) = NULL,
    @AssignedTo NVARCHAR(255),
    @TaskStatus NVARCHAR(100) = 'New',
    @TaskPriority NVARCHAR(100) = 'Normal',
    @CreatedBy NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Tasks
        (
            TaskName, TaskDescription, TaskFile, AssignedTo,
            TaskStatus, TaskPriority, CreatedBy, CreatedDate
        )
        VALUES
        (
            @TaskName, @TaskDescription, @TaskFile, @AssignedTo,
            @TaskStatus, @TaskPriority, @CreatedBy, GETDATE()
        );
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
