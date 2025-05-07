namespace FocusFlow.Abstractions.Models
{
    /// <summary>
    /// defines user related fields needed for users
    /// </summary>
    public interface IEntryRecordBaseUser
    {
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        string CreatedByInfo { get; set; }
        string UpdatedByInfo { get; set; }
    }
}