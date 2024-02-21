using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;

namespace TaskSched.Common.FieldValidator
{

    /// <summary>
    /// the set of validators and how to get to them
    /// </summary>
    public class FieldValidatorSet : IFieldValidatorSet
    {

        List<IFieldValidator> _validators;
        public FieldValidatorSet()
        {
            _validators = new List<IFieldValidator>()
            {
                new StringValidator(),
                new UrlValidator(),
                new ExecutablePathValidator(),
                new NumberValidator(),
                new DateTimeValidator(),
            };

        }

        /// <summary>
        /// validate a particular field
        /// </summary>
        /// <param name="fieldData"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public bool ValidateField(string fieldData, FieldTypeEnum fieldType)
        {
            IFieldValidator validator = GetFieldValidator(fieldType);
            return validator.IsValid(fieldData);
        }

        /// <summary>
        /// get a validator for a particular field type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public IFieldValidator<T>? GetFieldValidator<T>(FieldTypeEnum fieldType)
        {
            IFieldValidator validator = GetFieldValidator(fieldType);
            if (validator == null) return null;

            return validator as IFieldValidator<T>;
        }

        /// <summary>
        /// get a generic validator for a particular field type
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public IFieldValidator? GetFieldValidator(FieldTypeEnum fieldType)
        {
            foreach (var validator in _validators)
            {
                if (validator.CanHandle(fieldType))
                {
                    return validator;
                }
            }

            return null;

        }
    }
}
