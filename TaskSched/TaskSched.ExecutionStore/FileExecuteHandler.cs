using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using Activity = TaskSched.Common.DataModel.Activity;
using ActivityContext = TaskSched.Common.DataModel.ActivityContext;

namespace TaskSched.ExecutionStore
{
    /// <summary>
    /// File execute handler
    /// </summary>
    internal class FileExecuteHandler : IExecutionHandler
    {

        ILogger _logger;

        public FileExecuteHandler(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// readonly info chunck
        /// </summary>
        readonly ExecutionHandlerInfo info = new ExecutionHandlerInfo()
        {
            HandlerId = new Guid("00000000-0000-0000-0000-000000000001"),
            Name = "File Execute Handler",
            RequiredFields = new List<ActivityField>()
                    {
                        new ActivityField() { Name="ExecutablePath", IsReadOnly=true,},
                        new ActivityField() { Name="CommandLine", IsReadOnly=true,},
                    }
        };


        /// <summary>
        /// get the handler info
        /// </summary>
        public ExecutionHandlerInfo HandlerInfo
        {
            get
            {
                return info;

            }
        }


        /// <summary>
        /// validate the activity
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<ExpandedResult<bool>> ValidateActivity(Activity? activity)
        {
            ExpandedResult<bool> result = new ExpandedResult<bool>() { Result = true, Messages= new List<ResultMessage>() };

            if (activity == null)
            {
                result.Result = false;
                result.Messages.Add(new ResultMessage() { Message= "activity is null", Severity= ResultMessageSeverity.Error });
                return result;
            }

            if (activity.ActivityHandlerId != HandlerInfo.HandlerId)
            {
                result.Result = false;
                result.Messages.Add(new ResultMessage() { Message = "ActivityHandlerId is not correct", Severity = ResultMessageSeverity.Error });
            }

            //  check missing parameters
            foreach(var field in HandlerInfo.RequiredFields)
            {
                if (activity.DefaultFields.Any(x=>x.Name == field.Name) == false)
                {
                    result.Result = false;
                    result.Messages.Add(new ResultMessage() { Message = $"Missing field {field.Name}", Severity = ResultMessageSeverity.Error });
                }
            }

            return result;

        }

        /// <summary>
        /// validate the event activity
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="eventActivity"></param>
        /// <returns></returns>
        public async Task<ExpandedResult<bool>> ValidateEventActivity(Activity? activity, EventActivity? eventActivity)
        {
            var result = await ValidateActivity(activity);
            if (result.Result == false)
            {
                result.Messages.Add(new ResultMessage() { Message = "Errors with activity.  cannot validate the event activity", Severity = ResultMessageSeverity.Error });
                return result;

            }

            if (eventActivity == null)
            {
                result.Result = false;
                result.Messages.Add(new ResultMessage() { Message = "Event activity is null", Severity = ResultMessageSeverity.Error });
                return result;
            }

            if (eventActivity.ActivityId != activity.Id)
            {
                result.Result = false;
                result.Messages.Add(new ResultMessage() { Message = "ActivityId is not correct", Severity = ResultMessageSeverity.Error });
            }

            //  check missing parameters
            foreach (var field in activity.DefaultFields)
            {
                //  only need to check readonly = false fields.  IsReadOnly == true is at the activity level
                if (field.IsReadOnly == false)
                {
                    if (eventActivity.Fields.Any(x => x.Name == field.Name) == false)
                    {
                        result.Result = false;
                        result.Messages.Add(new ResultMessage() { Message = $"Missing field {field.Name}", Severity = ResultMessageSeverity.Error });
                    }

                }
                //  if it's not a readonly field, it should be in here.
            }

            return result;

        }

        /// <summary>
        /// handle the activity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<ActivityContext> HandleActivity(ActivityContext context)
        {
            _logger.LogInformation($"Executing activity {context.EventItem.Name}");

            string commandLine = context.Activity.DefaultFields.FirstOrDefault(x => x.Name == "CommandLine").Value;
            string executablePath = context.Activity.DefaultFields.FirstOrDefault(x => x.Name == "ExecutablePath").Value;

            Dictionary<string, object> arguments = new Dictionary<string, object>();
            foreach(var field in context.Activity.DefaultFields)
            {
                arguments[field.Name] = field.Value;
            }

            foreach(var field in context.EventActivity.Fields)
            {
                arguments[field.Name] = field.Value;
            }


            try
            {
                string cmdLine = commandLine.FormatDynamic(arguments);

                await Task.Run(() =>
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo()
                        {
                            FileName = executablePath,
                            Arguments = cmdLine
                        };

                        Debug.WriteLine($"{startInfo.FileName} {startInfo.Arguments}");

                        var process = Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error starting the FileExecuteHandler process - {executablePath} {commandLine}");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in running the FileExecuteHandler process - {executablePath} {commandLine}");
            }


            return context;
        }


    }

    public static class DynamicFormat
    {
        public static string FormatDynamic(this string formatString, Dictionary<string, object> parameters)
        {
            List<object> values = new List<object>();

            int currentIdx = -1;
            foreach (string key in parameters.Keys)
            {
                values.Add(parameters[key]);
                currentIdx++;

                formatString = formatString.Replace($"{{{key}", $"{{{currentIdx}");

            }

            string formattedString = String.Format(formatString, values.ToArray());
            return formattedString;
        }
    }
}
