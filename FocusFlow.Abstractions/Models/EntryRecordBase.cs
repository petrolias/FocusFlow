namespace FocusFlow.Abstractions.Models
{
    public record EntryRecordBase : IEntryBase
    {
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
