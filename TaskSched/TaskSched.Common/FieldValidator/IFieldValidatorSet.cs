namespace TaskSched.Common.FieldValidator
{
    public interface IFieldValidatorSet
    {
        IFieldValidator? GetFieldValidator(FieldTypeEnum fieldType);
        IFieldValidator<T>? GetFieldValidator<T>(FieldTypeEnum fieldType);
        bool ValidateField(string fieldData, FieldTypeEnum fieldType);
    }
}
