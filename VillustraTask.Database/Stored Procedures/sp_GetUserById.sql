CREATE PROCEDURE dbo.sp_GetUserById
    @UserId NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT UserId, FullName, DesignationId, ProfilePicture, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
        FROM dbo.Userlogin
        WHERE UserId = @UserId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
