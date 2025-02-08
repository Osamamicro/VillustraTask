CREATE PROCEDURE [dbo].[sp_GetTaskById]
    @TaskId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        TaskId,
        TaskName,
        TaskDescription,
        TaskFile,
        AssignedTo,
        TaskStatus,
        TaskPriority,
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate
    FROM dbo.Tasks
    WHERE TaskId = @TaskId;
END;
GO
