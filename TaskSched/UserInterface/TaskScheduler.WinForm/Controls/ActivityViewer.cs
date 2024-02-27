using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskSched.Common.DataModel;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class ActivityViewer : UserControl, ICanvasItem<ActivityModel>, ICanvasItemCanEdit, ICanvasItemCanDelete
    {

        public ITreeItem TreeItem { get; private set; }
        ActivityModel _activityModel;
        Activity _activity;
        ScheduleManager? _scheduleManager;

        public ActivityViewer()
        {
            InitializeComponent();
        }

        public void SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }

        public void ShowItem(object o)
        {
            ShowItem((ActivityModel)o);
        }

        public void ShowItem(ActivityModel o)
        {
            TreeItem = o;

            _activityModel = o as ActivityModel;
            _activity = _activityModel.Item;

            this.txtName.Text = o.Name;
            //this.lvFields 
            //this.cmbActivityHandler
            foreach (var field in _activity.DefaultFields)
            {
                lstFields.Items.Add(field);
            }

            if (lstFields.Items.Count > 0)
            {
                lstFields.SelectedItem = lstFields.Items[0];
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

        public void Revert()
        {
            MessageBox.Show("Revert");
        }

        public async void Save()
        {
            _activityModel.Item.Name = txtName.Text;
            var rslt = await _scheduleManager.SaveModel(_activityModel.ParentItem, _activityModel);

        }

        public async void Delete()
        {
            await _scheduleManager.DeleteItem(_activityModel);
        }
    }
}
