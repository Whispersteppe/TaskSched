namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// validate a string
    /// </summary>
    public class StringValidator : IFieldValidator<string>
    {
        public bool CanHandle(FieldTypeEnum type)
        {
            return type == FieldTypeEnum.String;
        }

        public string ConvertValue(string fieldData)
        {
            return fieldData;
        }

        public bool IsValid(string fieldData)
        {
            return true;
        }
    }
}
