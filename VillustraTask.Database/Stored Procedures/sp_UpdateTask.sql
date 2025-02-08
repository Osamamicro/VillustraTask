CREATE PROCEDURE [dbo].[sp_UpdateTask]
    @TaskId INT,
    @TaskName NVARCHAR(255),
    @TaskDescription NVARCHAR(MAX),
    @TaskFile NVARCHAR(500) = NULL,
    @AssignedTo NVARCHAR(255),
    @TaskStatus NVARCHAR(100),
    @TaskPriority NVARCHAR(100),
    @UpdatedBy NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE dbo.Tasks
        SET 
            TaskName = @TaskName,
            TaskDescription = @TaskDescription,
            TaskFile = @TaskFile,
            AssignedTo = @AssignedTo,
            TaskStatus = @TaskStatus,
            TaskPriority = @TaskPriority,
            UpdatedBy = @UpdatedBy,
            UpdatedDate = GETDATE()
        WHERE TaskId = @TaskId;

        COMMIT TRANSACTION;

        -- Return the number of rows affected.
        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
