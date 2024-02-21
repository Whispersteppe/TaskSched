namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// validates a path
    /// </summary>
    public class ExecutablePathValidator : IFieldValidator<string>
    {
        public bool CanHandle(FieldTypeEnum type)
        {
            return type == FieldTypeEnum.ExecutablePath;
        }

        public string ConvertValue(string fieldData)
        {
            if (IsValid(fieldData))
            {
                return fieldData;
            }

            throw new InvalidDataException($"{fieldData} is not a valid executable path");
        }

        public bool IsValid(string fieldData)
        {
            return File.Exists(fieldData);
        }
    }
}
