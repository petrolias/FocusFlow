namespace FocusFlow.Abstractions.Common
{
    public interface IValid
    {
        bool IsValid(out List<string> validationErrors);
    }
}
