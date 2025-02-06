USE VillustraTask;
GO

IF OBJECT_ID('dbo.sp_GetTasks', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetTasks;
GO

CREATE PROCEDURE dbo.sp_GetTasks
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT *
        FROM dbo.Tasks;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
