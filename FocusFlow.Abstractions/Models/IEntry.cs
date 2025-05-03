namespace FocusFlow.Abstractions.Models
{
    public interface IEntry
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string UpdatedBy{ get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}
