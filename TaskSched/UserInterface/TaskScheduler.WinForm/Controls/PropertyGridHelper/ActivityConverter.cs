using System.ComponentModel;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{
    public class ActivityConverter : StringConverter
    {
        List<ActivityModel> _handlers = [];

        public ActivityConverter()
        {
            var handlers = ScheduleManager.GlobalInstance.GetActivities().Result;
            foreach (var item in handlers)
            {
                _handlers.Add(item);
            }
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
        {
            return true;
        }

        public override StandardValuesCollection? GetStandardValues(ITypeDescriptorContext? context)
        {

            List<string> handlerNames = _handlers.Select(x => x.Name).ToList();
            return new StandardValuesCollection(handlerNames);
        }
    }

}
