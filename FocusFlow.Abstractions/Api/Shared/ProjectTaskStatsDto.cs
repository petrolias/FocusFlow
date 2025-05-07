namespace FocusFlow.Abstractions.Api.Shared
{
    public class ProjectTaskStatsDto
    {        
        public List<TaskItemDto> OverdueTasks { get; set; }        
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Overdue { get; set; }
    }
}
