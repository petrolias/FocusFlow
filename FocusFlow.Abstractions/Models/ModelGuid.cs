namespace FocusFlow.Abstractions.Models
{
    public class ModelGuid : IEntryBase
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}