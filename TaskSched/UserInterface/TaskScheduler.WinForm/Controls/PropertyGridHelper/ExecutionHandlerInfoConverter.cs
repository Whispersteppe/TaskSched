using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{
    public class ExecutionHandlerInfoConverter : TypeConverter
    {
        List<ExecutionHandlerInfo> _handlers = [];

        public ExecutionHandlerInfoConverter() 
        {
            var handlers = ScheduleManager.GlobalInstance.GetExecutionHandlerInfo().Result;
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

            //List<Guid> handlerIDs = _handlers.Select(x=>x.HandlerId).ToList();
            return new StandardValuesCollection(_handlers);
        }
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is string oData)
            {
                if (destinationType == typeof(ExecutionHandlerInfo))
                {
                    var foundExecutionType = _handlers.FirstOrDefault(x=>x.Name == oData);
                    return foundExecutionType;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }


    }

    public class ActivityIdConverter : GuidConverter
    {
        List<ActivityModel> _handlers = [];

        public ActivityIdConverter()
        {
            var activities = ScheduleManager.GlobalInstance.GetActivities().Result;
            foreach (var item in activities)
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
            List<Guid> handlerIDs = _handlers.Select(x => x.Id).ToList();
            return new StandardValuesCollection(handlerIDs);
        }
    }
}
