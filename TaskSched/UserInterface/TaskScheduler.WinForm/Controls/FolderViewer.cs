﻿using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class FolderViewer : UserControl, ICanvasItem<FolderModel>
    {
        readonly ScheduleManager _scheduleManager;
        readonly FolderModel _folderModel;
        
        bool _modelChanged;

        public FolderViewer(ScheduleManager scheduleManager, FolderModel item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;
            _folderModel = item;
            _folderModel.InstanceChanged = false;

            gridFolderProperties.SelectedObject = item;
            TreeItem = item;
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {

                ToolStripBuilder builder = new ToolStripBuilder();
                builder.AddButton("Save", TsSave_Click);
                builder.AddButton("Delete", TsDelete_Click);
                builder.AddButton("Add Folder", TsCreateChildFolder);
                builder.AddButton("Add Event", TsCreateChildEvent);
                builder.AddButton("Launch All", TsLaunchAllEvent);

                return builder.ToolStripItems;

            }
        }

        private async void TsLaunchAllEvent(object? sender, EventArgs e)
        {
            if (_folderModel != null && _folderModel.Events != null)
            {
                foreach (var item in _folderModel.Events)
                {
                    await _scheduleManager.LaunchEvent(item);
                }
            }

        }

        private async void TsCreateChildFolder(object? sender, EventArgs e)
        {
            var treeItem = await _scheduleManager.CreateModel(TreeItem, TreeItemTypeEnum.FolderItem);
        }

        private async void TsCreateChildEvent(object? sender, EventArgs e)
        {
            var treeItem = await _scheduleManager.CreateModel(TreeItem, TreeItemTypeEnum.EventItem);
        }


        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            if (await _scheduleManager.CanDeleteItem(_folderModel))
            {
                await _scheduleManager.DeleteItem(_folderModel);

            }

        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            await Save();
        }

        private async Task Save()
        {

            if (_folderModel.InstanceChanged == true)
            {
                _modelChanged = true;
            }

            if (_modelChanged == true)
            {
                var rslt = await _scheduleManager.SaveModel(_folderModel);
            }
        }


        public ITreeItem? TreeItem { get; private set; }

        public List<TreeItemTypeEnum> AllowedChildTypes => [TreeItemTypeEnum.FolderItem, TreeItemTypeEnum.EventItem];

        public async Task LeavingItem()
        {
            await Save();

        }


        public bool CanCreateChild(TreeItemTypeEnum itemType)
        {
            return true;
        }

        private void GridFolderProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _modelChanged = true;
        }
    }
}
