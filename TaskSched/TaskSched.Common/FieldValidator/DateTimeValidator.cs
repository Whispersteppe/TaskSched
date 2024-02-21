namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// validates a date time
    /// </summary>
    public class DateTimeValidator : IFieldValidator<DateTime>
    {
        public bool CanHandle(FieldTypeEnum type)
        {
            return type == FieldTypeEnum.DateTime;

        }

        public DateTime ConvertValue(string fieldData)
        {
            if (DateTime.TryParse(fieldData, out DateTime value))
            {
                return value;
            }

            throw new InvalidDataException($"{fieldData} is not a valid DateTime");
        }

        public bool IsValid(string fieldData)
        {
            return DateTime.TryParse(fieldData, out DateTime value);
        }
    }
}
