CREATE PROCEDURE dbo.sp_InsertUserlogin
    @UserId NVARCHAR(255),
    @Password NVARCHAR(500),  -- Adjusted size to match table
    @FullName NVARCHAR(255),
    @DesignationId INT = NULL,
    @ProfilePicture NVARCHAR(500) = NULL,
    @CreatedBy NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON; -- Ensures the transaction is fully rolled back on error

    BEGIN TRANSACTION;
    BEGIN TRY
        -- Check if User already exists
        IF EXISTS (SELECT 1 FROM dbo.Userlogin WHERE UserId = @UserId)
        BEGIN
            RAISERROR('User with this UserId already exists.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Ensure password is strong (at least 8 characters)
        IF LEN(@Password) < 8
        BEGIN
            RAISERROR('Password must be at least 8 characters long.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insert new user
        INSERT INTO dbo.Userlogin
        (
            UserId, Password, FullName, DesignationId,
            IsLocked, IsLoggedIn, ProfilePicture, CreatedBy, CreatedDate
        )
        VALUES
        (
            @UserId, @Password, @FullName, @DesignationId,
            0, 0, @ProfilePicture, @CreatedBy, GETDATE()
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
