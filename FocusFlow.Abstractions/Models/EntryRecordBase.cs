using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FocusFlow.Abstractions.Models
{
    /// <summary>
    /// contains basic properties for all entry records
    /// </summary>
    public record EntryRecordBase : IEntryBase, IEntryRecordBaseUser
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        public string CreatedByInfo { get; set; } = string.Empty;
        public string UpdatedByInfo { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public string? LastUpdatedByInfo() => string.IsNullOrEmpty(UpdatedByInfo) ? CreatedByInfo : UpdatedByInfo;
        public string? LastUpdatedDateTime() => UpdatedAt > CreatedAt ? UpdatedAtFormatted() : CreatedAtFormatted();
        public string? CreatedAtFormatted() => CreatedAt == DateTimeOffset.MinValue ? "Not available" : CreatedAt.ToString("u");
        public string? UpdatedAtFormatted() => UpdatedAt == DateTimeOffset.MinValue ? "Not available" : UpdatedAt.ToString("u");        
    }
}