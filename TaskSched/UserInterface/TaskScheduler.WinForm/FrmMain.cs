using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using TaskSched.DataStore;
using TaskScheduler.WinForm.Models;
using TaskScheduler.WinForm.NLogCustom;

namespace TaskScheduler.WinForm
{
    public partial class FrmMain : Form
    {

        bool _exitClicked = false;
        ScheduleManager _engineManager;
        ILoggerFactory _loggerFactory;

        ScheduleManagerConfig _config;

        ILogger _logger;

        public FrmMain()
        {
            InitializeComponent();

            //_loggerFactory = LoggerFactory.Create(builder => builder.AddNLog());
            _loggerFactory = LoggerFactory.Create(builder => builder.AddNLog(NLogBuilder));
            _logger = _loggerFactory.CreateLogger<FrmMain>();

            tsStop.Enabled = false;

            _logger.LogInformation("Starting the application");
        }


        private NLog.LogFactory NLogBuilder(IServiceProvider serviceProvider)
        {
            NLog.LogFactory factory = new NLog.LogFactory();
            var nlogConfig = new NLog.Config.XmlLoggingConfiguration("Nlog.config");

            factory.Configuration.AddTarget("InternalLogTarget", new NLogCustomTarget());
            factory.Configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "InternalLogTarget");
            //var factory = serviceProvider.GetService<NLog.LogFactory>();

            factory.Configuration = nlogConfig;

            return factory;
        }

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

        public void FrmMain_Load(object? sender, EventArgs e)
        {

            HideMainForm();

            //TaskSchedDbContextConfiguration dbConfig = new TaskSchedDbContextConfiguration()
            //{
            //    DataSource = Path.Combine(Directory.GetCurrentDirectory(), "TaskSched.sqLite")
            //};



            var config = GetConfiguration();

            _engineManager = new ScheduleManager(config, _loggerFactory);



            List<ITreeItem> items = _engineManager.GetAllRoots();

            schedulerTreeView.SetTreeviewCollection(items);


        }


        private ScheduleManagerConfig GetConfiguration()
        {

            _config = new ScheduleManagerConfig();

            IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("TaskScheduler.json")
                .Build();

            _configuration.Bind(_config);

            return _config;

        }


        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_exitClicked == false)
            {
                e.Cancel = true;
                HideMainForm();
            }
            else
            {
                //  let it ride
                if (_engineManager.IsRunning == true)
                {
                    _engineManager.Stop().GetAwaiter().GetResult();
                }
            }
        }


        #region Notify Events

        private void _notifyContextShow_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void _notifyContextCreate_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Create clicked");
        }

        private void _notifyContextStart_Click(object sender, EventArgs e)
        {
            StartEngine();


        }

        private void _notifyContextStop_Click(object sender, EventArgs e)
        {
            StopEngine();

        }

        private void _notifyContextExit_Click(object sender, EventArgs e)
        {
            _exitClicked = true;
            this.Close();
        }

        private void _notifyIcon_Click(object sender, EventArgs e)
        {
            _notifyIconMenu.Show(Cursor.Position);
        }

        #endregion




        private void schedulerTreeView_OnItemSelected(ITreeItem item)
        {
            canvasSelector.ViewItem(item);
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            _exitClicked = true;
            this.Close();
        }

        private void tsStart_Click(object sender, EventArgs e)
        {
            StartEngine();
        }

        private void tsStop_Click(object sender, EventArgs e)
        {
            StopEngine();
        }

        private void StartEngine()
        {
            _notifyContextStart.Enabled = false;
            tsStart.Enabled = false;
            if (_engineManager.IsRunning == false)
            {
                _engineManager.Start().GetAwaiter().GetResult();
            }
            _notifyContextStop.Enabled = true;
            tsStop.Enabled = true;
            _notifyIcon.Text = "Scheduler Started";

        }

        private void StopEngine()
        {
            _notifyContextStop.Enabled = false;
            tsStop.Enabled = false;
            if (_engineManager.IsRunning == true)
            {
                _engineManager?.Stop().GetAwaiter().GetResult();
            }
            _notifyContextStart.Enabled = true;
            tsStart.Enabled = true;
            _notifyIcon.Text = "Scheduler Stopped";

        }
    }
}
