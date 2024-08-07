using CommunityToolkit.WinUI.Notifications;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Configuration;
using TaskScheduler.WinForm.Models;
using TaskScheduler.WinForm.NLogCustom;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TaskScheduler.WinForm
{
    public partial class FrmMain : Form
    {

        bool _exitClicked = false;
        readonly ScheduleManager _engineManager;
        readonly TaskSchedConfigurationHandler _config;

        readonly ILogger _logger;

        readonly NLogWrapper _loggerWrapper;

        public FrmMain()
        {
            InitializeComponent();



            _config = new TaskSchedConfigurationHandler();


            _loggerWrapper = new NLogWrapper(_config.NLogConfig);

            _logger = _loggerWrapper.CreateLogger<FrmMain>();

            tsStop.Enabled = false;

            _engineManager = new ScheduleManager(_config.Configuration, _loggerWrapper.LoggerFactory, _loggerWrapper.LogEmitter);
            _engineManager.OnStartEvent += _engineManager_OnStartEvent;
            _engineManager.OnFinishEvent += _engineManager_OnFinishEvent;
            _engineManager.OnStartActivity += _engineManager_OnStartActivity;
            _engineManager.OnFinishActivity += _engineManager_OnFinishActivity;

            _engineManager.OnTreeItemRemoving += _engineManager_OnTreeItemRemoving;

            _logger.LogInformation("Starting the application");
        }

        private void _engineManager_OnFinishActivity(TaskSched.Common.DataModel.ActivityContext context)
        {
            //throw new NotImplementedException();
        }

        private void _engineManager_OnStartActivity(TaskSched.Common.DataModel.ActivityContext context)
        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new MethodInvoker(() => _engineManager_OnStartActivity(context)));
//            }
//            else
//            {
//                ToastContentBuilder builder = new ToastContentBuilder();
//                builder.AddText($"Running {context.EventItem.Name} {context.Activity.Name}");
//                builder.Show(x =>
//                {
//                    x.ExpirationTime = DateTime.Now.AddSeconds(5);
//                });
//            }
        }

        private void _engineManager_OnFinishEvent(TaskSched.Common.DataModel.Event context)
        {
            //throw new NotImplementedException();
        }

        private void _engineManager_OnStartEvent(TaskSched.Common.DataModel.Event context)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => _engineManager_OnStartEvent(context)));
            }
            else
            {
//                ToastContentBuilder builder = new ToastContentBuilder();
//                builder.AddText($"Running {context.Name}");
//                builder.Show(x =>
//                {
//                    x.ExpirationTime = DateTime.Now.AddSeconds(5);
//                });
            }
        }

        private void _engineManager_OnTreeItemRemoving(ITreeItem treeItem, EventCancellationArgs cancelArgs)
        {
            var rslt = MessageBox.Show($"Delete {treeItem.DisplayName}?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (rslt == DialogResult.Cancel)
            {
                cancelArgs.Cancel = true;
                cancelArgs.AddReason("User cancelled");
            }
        }

        #region NLog setup




        #endregion

        private void ShowMainForm()
        {
            Visible = true;
            Opacity = 100;
            ShowInTaskbar = true;

        }

        private void HideMainForm()
        {
            Visible = false;
            Opacity = 0;
            ShowInTaskbar = false;
        }

        public async void FrmMain_Load(object? sender, EventArgs e)
        {

            HideMainForm();

            this.Location = _config.Configuration.DisplayConfig.MainWindowLocation.Location;
            this.Size = _config.Configuration.DisplayConfig.MainWindowLocation.Size;
            mainSplit.SplitterDistance = _config.Configuration.DisplayConfig.MainSplitterDistance;

            await schedulerTreeView.SetScheduleManager(_engineManager);
            await canvasSelector.SetScheduleManager(_engineManager);

            if (_config.Configuration.EngineConfig.StartOnStartup == true)
            {
                await StartEngine();
            }

            if (_config.Configuration.DisplayConfig.ShowOnStartup == true)
            {
                ShowMainForm();
            }


        }


        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_exitClicked == false)
            {
                //  let it ride.  we just hide things.
                e.Cancel = true;
                HideMainForm();
            }
            else
            {
                if (_engineManager.ExecutionStatus == ExecutionStatusEnum.Running)
                {
                    await _engineManager.Stop();
                }

                await canvasSelector.CloseCurrentItem();

                //  save the last location of the form
                _config.Configuration.DisplayConfig.MainWindowLocation.Location = this.Location;
                _config.Configuration.DisplayConfig.MainWindowLocation.Size = this.Size;
                _config.Configuration.DisplayConfig.MainSplitterDistance = mainSplit.SplitterDistance;

                _config.SaveConfiguration();
            }
        }


        #region Notify Events

        private async void notifyContextShow_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { });

            ShowMainForm();
        }

        private async void notifyContextCreate_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { });

            Debug.WriteLine("Create clicked");
        }

        private async void notifyContextStart_Click(object sender, EventArgs e)
        {
            await StartEngine();


        }

        private async void notifyContextStop_Click(object sender, EventArgs e)
        {
            await StopEngine();

        }

        private async void notifyContextExit_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { });

            _exitClicked = true;
            this.Close();
        }

        private async void notifyIcon_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { });

            _notifyIconMenu.Show(Cursor.Position);
        }

        #endregion


        private async void tsExit_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { });

            _exitClicked = true;
            this.Close();
        }

        private async void tsStart_Click(object sender, EventArgs e)
        {
            await StartEngine();
        }

        private async void tsStop_Click(object sender, EventArgs e)
        {
            await StopEngine();
        }

        private async Task StartEngine()
        {
            _notifyContextStart.Enabled = false;
            tsStart.Enabled = false;
            if (_engineManager.ExecutionStatus == ExecutionStatusEnum.Stopped)
            {
                await _engineManager.Start();
            }
            _notifyContextStop.Enabled = true;
            tsStop.Enabled = true;
            _notifyIcon.Text = "Scheduler Started";

        }

        private async Task StopEngine()
        {
            _notifyContextStop.Enabled = false;
            tsStop.Enabled = false;
            if (_engineManager.ExecutionStatus == ExecutionStatusEnum.Running)
            {
                await _engineManager.Stop();
            }
            _notifyContextStart.Enabled = true;
            tsStart.Enabled = true;
            _notifyIcon.Text = "Scheduler Stopped";

        }

    }
}
