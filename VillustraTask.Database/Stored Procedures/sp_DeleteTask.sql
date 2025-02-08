CREATE PROCEDURE [dbo].[sp_DeleteTask]
    @TaskId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM dbo.Tasks
        WHERE TaskId = @TaskId;

        COMMIT TRANSACTION;
        -- No SELECT statement here; the DELETE’s affected row count will be returned.
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
