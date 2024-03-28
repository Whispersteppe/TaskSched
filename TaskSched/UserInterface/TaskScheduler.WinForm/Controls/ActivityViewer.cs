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
using TaskSched.Common.FieldValidator;
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

        ActivityField _currentActivityField;


        public ActivityViewer()
        {
            InitializeComponent();
        }


        public async Task Initialize(ScheduleManager scheduleManager, ActivityModel o)
        {
            _scheduleManager = scheduleManager;
            _activityModel = o;
            TreeItem = o;

            _activity = _activityModel;

            this.txtName.Text = o.Name;

            //cmbFieldType
            cmbFieldType.Items.Clear();
            foreach (var fieldType in Enum.GetValues<FieldTypeEnum>())
            {
                cmbFieldType.Items.Add(fieldType);
            }

            //lvFields 
            foreach (var field in _activity.DefaultFields)
            {
                lstFields.Items.Add(field);
            }

            if (lstFields.Items.Count > 0)
            {
                lstFields.SelectedIndex = 0;
            }



            //cmbActivityHandler
            cmbActivityHandler.Items.Clear();

            foreach (var handler in await _scheduleManager.GetHandlerInfo())
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
            if (await _scheduleManager.CanDeleteItem(_activityModel))
            {
                await _scheduleManager.DeleteItem(_activityModel);
            }
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            await Save();
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

            _currentActivityField = field;

        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {

                    if (field.IsReadOnly == false)
                    {
                        _currentActivityField = null;

                        //  remove it
                        lstFields.Items.Remove(lstFields.SelectedItem);
                        if (TreeItem is Activity activity)
                        {
                            activity.DefaultFields.Remove(field);
                        }
                    }
                }

            }

        }

        //private void lstFields_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (lstFields.SelectedItem != null)
        //    {
        //        if (lstFields.SelectedItem is ActivityField field)
        //        {
        //            txtFieldName.Text = field.Name;
        //            cmbFieldType.SelectedItem = field.FieldType;
        //            txtFieldDefault.Text = field.Value;
        //            chkFieldRequiredByHandler.Checked = field.IsReadOnly;
        //        }

        //    }
        //    else
        //    {
        //        txtFieldName.Text = "";
        //        cmbFieldType.SelectedItem = FieldTypeEnum.String;
        //        txtFieldDefault.Text = "";
        //        chkFieldRequiredByHandler.Checked = false;
        //    }
        //}

        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveActivityField();

            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {

                    txtFieldName.Text = field.Name;
                    cmbFieldType.SelectedItem = field.FieldType;
                    chkFieldRequiredByHandler.Checked = field.IsReadOnly;
                    txtFieldDefault.Text = field.Value;

                    if (field.IsReadOnly)
                    {
                        txtFieldName.Enabled = false;
                        cmbFieldType.Enabled = false;

                    }
                    else
                    {
                        txtFieldName.Enabled = true;
                        cmbFieldType.Enabled = true;
                    }


                    _currentActivityField = field;

                }
            }
        }



        private void btnSaveField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItem != null)
            {
                if (lstFields.SelectedItem is ActivityField field)
                {
                    field.Name = txtFieldName.Text;
                    field.FieldType = (cmbFieldType.SelectedItem is FieldTypeEnum) ? (FieldTypeEnum)cmbFieldType.SelectedItem : FieldTypeEnum.String;
                    field.Value = txtFieldDefault.Text;
                    field.IsReadOnly = chkFieldRequiredByHandler.Checked;
                }

            }
        }

        #endregion

        #endregion

        public async Task Save()
        {
            SaveActivityField();

            //todo - there's more that needs to go in here.  look at EventViewer for more details
            _activityModel.Name = txtName.Text;
            await _scheduleManager.SaveModel(_activityModel.ParentItem, _activityModel);
        }


        public async Task LeavingItem()
        {
            await Save();
        }



        private void cmbActivityHandler_SelectedValueChanged(object sender, EventArgs e)
        {
            //make sure the properties on the selected handler in the combobox exist in the field list

            if (cmbActivityHandler.SelectedItem is ExecutionHandlerInfo handlerInfo)
            {
                foreach (var requiredfield in handlerInfo.RequiredFields)
                {
                    bool foundField = false;
                    //  find in the current field list
                    foreach (var field in lstFields.Items)
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

        private void SaveActivityField()
        {
            if (_currentActivityField != null)
            {

                _currentActivityField.Value = txtFieldDefault.Text;
                _currentActivityField.Name = txtFieldName.Text;
                _currentActivityField.FieldType = (cmbFieldType.SelectedItem is FieldTypeEnum) ? (FieldTypeEnum)cmbFieldType.SelectedItem : FieldTypeEnum.String;
                _currentActivityField.IsReadOnly = chkFieldRequiredByHandler.Checked;
            }
        }

    }
}
