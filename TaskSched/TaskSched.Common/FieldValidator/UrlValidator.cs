namespace TaskSched.Common.FieldValidator
{
    /// <summary>
    /// validates a Uri
    /// </summary>
    public class UrlValidator : IFieldValidator<Uri>
    {
        public bool CanHandle(FieldTypeEnum type)
        {
            return type == FieldTypeEnum.Url;
        }

        public Uri ConvertValue(string fieldData)
        {
            UriCreationOptions options = new UriCreationOptions()
            { DangerousDisablePathAndQueryCanonicalization = true };

            if (Uri.TryCreate(fieldData, options, out Uri value))
            {
                return value;
            }

            throw new InvalidDataException($"{fieldData} is not a Uri");
        }

        public bool IsValid(string fieldData)
        {
            UriCreationOptions options = new UriCreationOptions()
            { DangerousDisablePathAndQueryCanonicalization = false };
            return Uri.TryCreate(fieldData, options, out Uri value);
        }
    }
}
