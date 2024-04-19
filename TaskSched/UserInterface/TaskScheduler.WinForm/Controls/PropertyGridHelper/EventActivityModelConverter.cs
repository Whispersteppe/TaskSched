using System.ComponentModel;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{
    public class EventActivityModelConverter : TypeConverter
    {
        public override PropertyDescriptorCollection? GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

            //  get the current set of properties

            if (value is EventActivityModel activityModel)
            {
                //var baseProperties = value.GetType().GetProperties();
                var baseProperties = TypeDescriptor.GetProperties(activityModel, [new BrowsableAttribute(true)]);

                foreach (PropertyDescriptor baseProp in baseProperties)
                {
                    if (baseProp.Name != "Fields")
                    {
                        properties.Add(baseProp);
                    }
                }
                //  now we add the fields

                foreach (EventActivityFieldModel field in activityModel.Fields)
                {
                    PropertyDescriptor desc = new EventActivityFieldModelDescriptor(field, [new CategoryAttribute("Fields")]);
                    // TypeDescriptor.CreateProperty(typeof(EventActivityFieldModel), field.Name, typeof(string));
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

}
