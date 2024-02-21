namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// validate a number
    /// </summary>
    /// <remarks>
    /// this is only doing integer numbers at this time.
    /// </remarks>
    public class NumberValidator : IFieldValidator<int>
    {
        public bool CanHandle(FieldTypeEnum type)
        {
            return type == FieldTypeEnum.Number;
        }

        public int ConvertValue(string fieldData)
        {
            if (int.TryParse(fieldData, out int value))
            {
                return value;
            }

            throw new InvalidDataException($"{fieldData} is not an int");
        }

        public bool IsValid(string fieldData)
        {
            return int.TryParse(fieldData, out int value);
            
        }
    }
}
