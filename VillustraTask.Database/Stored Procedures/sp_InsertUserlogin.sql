Create PROCEDURE [dbo].[sp_InsertUserlogin]
    @UserId NVARCHAR(255),
    @Password NVARCHAR(500),  
    @FullName NVARCHAR(255),
    @DesignationId INT = NULL,
    @ProfilePicture NVARCHAR(500) = NULL,
    @CreatedBy NVARCHAR(255) = NULL,
    @IsLocked BIT = 0,      
    @IsLoggedIn BIT = 0 
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- Check if the user already exists
        IF EXISTS (SELECT 1 FROM dbo.Userlogin WHERE UserId = @UserId)
        BEGIN
            RAISERROR('User with this UserId already exists.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Validate password length
        IF LEN(@Password) < 8
        BEGIN
            RAISERROR('Password must be at least 8 characters long.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insert user
        INSERT INTO dbo.Userlogin
        (
            UserId, Password, FullName, DesignationId,
            IsLocked, IsLoggedIn, ProfilePicture, CreatedBy, CreatedDate
        )
        VALUES
        (
            @UserId, @Password, @FullName, @DesignationId,
            @IsLocked, @IsLoggedIn, @ProfilePicture, @CreatedBy, GETDATE()
        );

        COMMIT TRANSACTION;

        -- Return a success indicator (1)
        SELECT 1 AS RowsAffected;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
