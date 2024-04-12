using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class ConfigViewer : UserControl, ICanvasItem<FolderModel>
    {

        readonly ScheduleManager _scheduleManager;
        readonly ConfigurationRootModel _configModel;


        bool _modelChanged;

        public ConfigViewer(ScheduleManager scheduleManager, ConfigurationRootModel item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;
            _configModel = item;
            _configModel.InstanceChanged = false;

            gridConfig.SelectedObject = _configModel.ScheduleManagerConfiguration;
            TreeItem = item;
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {

                ToolStripBuilder builder = new ToolStripBuilder();
                builder.AddButton("Save", TsSave_Click);

                return builder.ToolStripItems;

            }
        }

        public ITreeItem? TreeItem { get; private set; } 

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            await Save();
        }

        private async Task Save()
        {

            if (_configModel.InstanceChanged == true)
            {
                _modelChanged = true;
            }

            if (_modelChanged == true)
            {
                //var rslt = await _scheduleManager.SaveModel(_configModel);
            }
        }

        public async Task LeavingItem()
        {
            await Save();
        }
    }
}
