-- CreateDatabase.sql

IF DB_ID('VillustraTask') IS NOT NULL
BEGIN
    PRINT 'Database already exists. Dropping database...'
    ALTER DATABASE VillustraTask SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VillustraTask;
END
GO

CREATE DATABASE VillustraTask;
GO

USE VillustraTask;
GO
