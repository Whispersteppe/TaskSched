using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls.PropertyGridHelper
{
    public class ExecutionHandlerInfoConverter : StringConverter
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

            List<string> handlerNames = _handlers.Select(x=>x.Name).ToList();
            return new StandardValuesCollection(handlerNames);
        }
    }

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
