using System.ComponentModel;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{

    public class InternalDictionary<TKey, TData> : Dictionary<TKey, TData>
    {
        public string _title;

        public InternalDictionary()
        {
            _title = string.Empty;
        }
        public InternalDictionary(string title) 
        { 
            _title = title;
        }
        public override string ToString()
        {
            return _title;
        }
    }

    public class DictionaryConverter<TKey, TData> : TypeConverter
    {
        public override PropertyDescriptorCollection? GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

            //  get the current set of properties

            if (value is Dictionary<TKey, TData> stringDictionary)
            {

                foreach (KeyValuePair<TKey,TData> item in stringDictionary)
                {
                    PropertyDescriptor desc = new KvpDescriptor<TKey, TData>(item, [new CategoryAttribute("Fields")]);
                    properties.Add(desc);
                }

            }
            else
            {
                //  we can't do anything.  punt.
                return base.GetProperties(context, value, attributes);
            }


            PropertyDescriptorCollection propertyCollection = new PropertyDescriptorCollection(properties.ToArray());
            return propertyCollection;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext? context)
        {
            return true;
        }
    }

    public class KvpDescriptor<TKey, TData> : PropertyDescriptor
    {
        KeyValuePair<TKey, TData> _value;
        public KvpDescriptor(KeyValuePair<TKey, TData> value, Attribute[]? attributes = null)
            : base(value.Key.ToString(), attributes)
        {
            _value = value;
        }
        public override Type ComponentType => typeof(string);

        public override bool IsReadOnly => true;

        public override Type PropertyType => typeof(string);

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object? GetValue(object? component)
        {
            return _value.Value;
        }

        public override void ResetValue(object component)
        {
            
        }

        public override void SetValue(object? component, object? value)
        {
            
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

}
