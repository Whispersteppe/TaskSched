using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class ActivityViewer : UserControl, ICanvasItem<ActivityModel>
    {

        public ITreeItem TreeItem { get; private set; }
        ActivityModel _activityModel;
        Activity _activity;
        ScheduleManager? _scheduleManager;

        public ActivityViewer()
        {
            InitializeComponent();
        }


        public async Task Initialize(ScheduleManager scheduleManager, ActivityModel o)
        {
            _scheduleManager = scheduleManager;
            _activityModel = o;
            TreeItem = o;

            _activity = _activityModel.Item;

            this.txtName.Text = o.Name;

            //this.lvFields 
            foreach (var field in _activity.DefaultFields)
            {
                lstFields.Items.Add(field);
            }

            if (lstFields.Items.Count > 0)
            {
                lstFields.SelectedItem = lstFields.Items[0];
            }

            //this.cmbActivityHandler
            cmbActivityHandler.Items.Clear();

            foreach(var handler in await _scheduleManager.GetHandlerInfo())
            {
                cmbActivityHandler.Items.Add(handler);
            }


            foreach (var item in cmbActivityHandler.Items)
            {
                if (item is ExecutionHandlerInfo handlerInfo)
                {
                    if (handlerInfo.HandlerId == _activity.ActivityHandlerId)
                    {
                        cmbActivityHandler.SelectedItem = item;
                        break;
                    }
                }
            }

            if (cmbActivityHandler.SelectedItem == null)
            {
                cmbActivityHandler.SelectedIndex = 0;
            }

        }

        public async Task Initialize(ScheduleManager scheduleManager, object treeItem)
        {
            await Initialize(scheduleManager, treeItem as ActivityModel);
        }

        public List<ToolStripItem> ToolStripItems
        {


            get
            {
                ToolStripBuilder builder = new ToolStripBuilder();
                builder.AddButton("Save", TsSave_Click);
                builder.AddButton("Delete", TsDelete_Click);

                return builder.ToolStripItems;
            }
        }

        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            await _scheduleManager.DeleteItem(_activityModel);
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _activityModel.Item.Name = txtName.Text;
            var rslt = await _scheduleManager.SaveModel(_activityModel.ParentItem, _activityModel);
        }

        public async Task SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;

            var infoItems = await _scheduleManager.GetHandlerInfo();

            foreach (var infoItem in infoItems)
            {
                cmbActivityHandler.Items.Add(infoItem);
            }


        }


        #region handlers

        #region field handlers

        private void btnAddField_Click(object sender, EventArgs e)
        {
            ActivityField field = new ActivityField()
            {
                FieldType = TaskSched.Common.FieldValidator.FieldTypeEnum.String,
                IsReadOnly = false,
                Name = "not set",
                Value = "not set"
            };

            _activity.DefaultFields.Add(field);
            lstFields.Items.Add(field);
            lstFields.SelectedItem = field;
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {
                    if (field.IsReadOnly == false)
                    {
                        //  remove it
                        lstFields.Items.Remove(lstFields.SelectedItem);
                        if (TreeItem.UnderlyingItem is Activity activity)
                        {
                            activity.DefaultFields.Remove(field);
                        }
                    }
                }

            }

        }

        private void lstFields_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {
                    txtFieldName.Text = field.Name;
                    txtFieldType.Text = field.FieldType.ToString();
                    txtFieldDefault.Text = field.Value;
                    chkFieldRequiredByHandler.Checked = field.IsReadOnly;
                }

            }
            else
            {
                txtFieldName.Text = "";
                txtFieldType.Text = "";
                txtFieldDefault.Text = "";
                chkFieldRequiredByHandler.Checked = false;
            }
        }
        private void btnSaveField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {
                    field.Name = txtFieldName.Text;
                    field.FieldType = TaskSched.Common.FieldValidator.FieldTypeEnum.String; //TODO will probably want a dropdown here
                    field.Value = txtFieldDefault.Text;
                    field.IsReadOnly = chkFieldRequiredByHandler.Checked;
                }

            }
        }

        #endregion

        #endregion

        public bool CanClose()
        {
            return true;
        }


        private void cmbActivityHandler_SelectedValueChanged(object sender, EventArgs e)
        {
            //make sure the properties on the selected handler in the combobox exist in the field list

            if (cmbActivityHandler.SelectedItem is ExecutionHandlerInfo handlerInfo)
            {
                foreach(var requiredfield in handlerInfo.RequiredFields)
                {
                    bool foundField = false;
                    //  find in the current field list
                    foreach(var field in lstFields.Items)
                    {
                        if (field is ActivityField activityField)
                        {
                            if (activityField.Name == requiredfield.Name)
                            {
                                //  we've found this one.
                                foundField = true;
                                break;
                            }
                        }
                    }

                    if (foundField == false)
                    {
                        ActivityField field = new ActivityField()
                        {
                            FieldType = requiredfield.FieldType,
                            IsReadOnly = true,
                            Name = requiredfield.Name,
                            Value = "not set"
                        };

                        _activity.DefaultFields.Add(field);
                        lstFields.Items.Add(field);
                        lstFields.SelectedItem = field;
                    }
                    
                }

            }


        }

    }
}
