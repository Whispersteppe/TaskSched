using System.ComponentModel;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{
    class EventActivityFieldModelDescriptor : PropertyDescriptor
    {
        public EventActivityFieldModelDescriptor(EventActivityFieldModel field, Attribute[]? attributes = null)
            : base(field.Name, attributes)
        {
            Field = field;
        }

        public EventActivityFieldModel Field { get; private set; }

        public override Type ComponentType => typeof(EventActivityFieldModel);

        public override bool IsReadOnly => false;

        public override Type PropertyType => typeof(string);

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return Field.Value;
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {
            Field.Value = (string)value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }

}
