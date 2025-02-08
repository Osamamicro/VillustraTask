-- SeedData.sql
USE VillustraTask;
GO

-- Insert a sample user
INSERT INTO dbo.Userlogin (UserId, Password, FullName, DesignationId, IsLocked, IsLoggedIn, CreatedBy)
VALUES ('sampleuser@example.com', '$2a$12$EXAMPLEHASHVALUE1234567890abcdefghi', 'Sample User', 1, 0, 0, 'system');
GO

-- Insert a sample task
INSERT INTO dbo.Tasks (TaskName, TaskDescription, TaskFile, AssignedTo, TaskStatus, TaskPriority, CreatedBy)
VALUES ('Initial Task', 'This is a sample task description.', 'uploads/sample.txt', 'sampleuser@example.com', 'New', 'High', 'system');
GO
