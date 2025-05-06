namespace FocusFlow.Abstractions.Models
{
    public interface IHistory
    {
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; } // "Modified" or "Deleted"
    }
}
