namespace FocusFlow.Abstractions.Models
{
    public interface IEntryBase
    {
        public Guid Id { get; set; }
        string CreatedBy { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string UpdatedBy { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}