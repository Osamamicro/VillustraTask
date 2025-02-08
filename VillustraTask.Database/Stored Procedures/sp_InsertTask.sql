Create PROCEDURE [dbo].[sp_InsertTask]
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
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
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

        COMMIT TRANSACTION;

        -- Return number of rows inserted (should be 1 if successful)
        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
