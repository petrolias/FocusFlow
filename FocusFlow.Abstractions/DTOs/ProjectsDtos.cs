namespace FocusFlow.Abstractions.DTOs
{
    public record ProjectCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    };

    public record ProjecUpdateDto : ProjectDto
    {
    }

    public record ProjectDto : ProjectCreateDto
    {
        public Guid Id { get; set; }
    }
}