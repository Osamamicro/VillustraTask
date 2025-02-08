﻿namespace VillustraTask.Api.Models
{
    public class CreateTaskRequest
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskFile { get; set; }
        public string AssignedTo { get; set; }
        public string TaskStatus { get; set; }
        public string TaskPriority { get; set; }
        public string CreatedBy { get; set; }
    }
}
