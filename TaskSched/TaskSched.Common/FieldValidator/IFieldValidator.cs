namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// field validator interface
    /// </summary>
    public interface IFieldValidator
    {
        bool CanHandle(FieldTypeEnum type);
        bool IsValid(string fieldData);

    }
}
