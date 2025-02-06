
CREATE PROCEDURE dbo.sp_InsertUserlogin
    @UserId NVARCHAR(255),
    @Password NVARCHAR(255),
    @FullName NVARCHAR(255),
    @DesignationId INT = NULL,
    @ProfilePicture NVARCHAR(500) = NULL,
    @CreatedBy NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Userlogin
        (
            UserId, Password, FullName, DesignationId,
            Islocked, IsLoggedIn, ProfilePicture, CreatedBy, CreatedDate
        )
        VALUES
        (
            @UserId, @Password, @FullName, @DesignationId,
            0, 0, @ProfilePicture, @CreatedBy, GETDATE()
        );
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
