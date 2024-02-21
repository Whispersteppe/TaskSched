namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// field validator interface with conversion
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public interface IFieldValidator<TValueType> : IFieldValidator
    {
        TValueType ConvertValue(string fieldData);
    }
}
