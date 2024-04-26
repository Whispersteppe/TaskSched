using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskScheduler.WinForm.NLogCustom;

namespace TaskScheduler.WinForm.Models
{

    public class LogViewModelConfig
    {
        public string Name { get; set; }
        public NLog.LogLevel MinLogLevel { get; set; }
        public int MaxLogCount { get; set; } = 100;
        public List<string> AllowedLoggerNames { get; set; } = [];
        public List<string> DeniedLoggerNames { get; set; } = [];

        public List<string> DeniedMessageText { get; set; } = [];
    }


    public class LogViewModel : ITreeItem
    {
        public event OnLogMessage OnLogMessage;

        public string DisplayName { get => _config.Name; }
        public virtual Guid? ID => Guid.Empty;

        public Guid? ParentId { get => null; }


        LogViewModelConfig _config;
        ILogEmitter _logEmitter;
        List<LogEventInfoExtended> _logItems = new List<LogEventInfoExtended>();

        public List<LogEventInfoExtended> LogItems { get => _logItems; }
        public LogViewModel(LogViewModelConfig config, ILogEmitter logEmitter)
        {
            _config = config;
            _logEmitter = logEmitter;

            _logEmitter.OnLogMessage += _logEmitter_OnLogMessage;
        }

        private void _logEmitter_OnLogMessage(LogEventInfoExtended logEventInfo)
        {
            if (logEventInfo.Extended.Level < _config.MinLogLevel) return;
            if (_config.AllowedLoggerNames.Count > 0)
            {
                bool loggerNameFound = false;
                foreach( string loggerName in _config.AllowedLoggerNames )
                {
                    if (logEventInfo.Extended.LoggerName.Contains( loggerName ) )
                    {
                        loggerNameFound = true;
                        break;
                    }
                }

                if (loggerNameFound == false) return;
            }

            if (_config.DeniedLoggerNames.Count > 0)
            {
                bool loggerNameFound = false;
                foreach (string loggerName in _config.DeniedLoggerNames )
                {
                    if (logEventInfo.Extended.LoggerName.Contains(loggerName))
                    {
                        loggerNameFound = true;
                        break;
                    }
                }
                if (loggerNameFound == true) return;  //  in this case, if found we want to leave
            }

            if (_config.DeniedMessageText.Count > 0)
            {
                bool textFound = false;
                foreach (string textString in _config.DeniedMessageText)
                {
                    if (logEventInfo.Message.Contains(textString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        textFound = true;
                        break;
                    }
                }
                if (textFound == true) return;  //  in this case, if found we want to leave

            }


            _logItems.Add(logEventInfo);
            while (_logItems.Count > _config.MaxLogCount)
            {
                _logItems.RemoveAt(0);
            }
            OnLogMessage?.Invoke(logEventInfo);



        }

        public  TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.LogViewItem;

        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [];

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [];

        public List<ITreeItem> Children { get; protected set; } = new List<ITreeItem>();





        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
            return false;
        }

        public bool CanHaveChildren()
        {
            if (AllowedChildTypes.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            return false;
        }

        public bool CanDeleteItem()
        {
            return false;
        }

        public bool CanEdit()
        {
            return false;
        }


        public bool ContainsText(string text)
        {
            return false;
        }

        public ContextMenuStrip? GetContextMenu()
        {

            return null;

        }
    }

}
