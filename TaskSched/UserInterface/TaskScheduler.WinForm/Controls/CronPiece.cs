using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TaskSched.Component.Cron;

namespace TaskScheduler.WinForm.Controls
{

    public partial class CronPiece : UserControl
    {

        List<CronComponentTypes> componentTypeValues = [];

        public CronPiece()
        {
            InitializeComponent();

            //BuildComponentTypeValues();

        }

        ICronComponent _currentComponent;
        public void Initialize(ICronComponent cronComponent)
        {
            _currentComponent = cronComponent;
            BuildComponentTypeValues();

        }

        private void BuildComponentTypeValues()
        {
            componentTypeValues.Clear();

            if (_currentComponent != null)
            {

                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.AllowAny))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.AllowAny, Name = "Any", ComponentPanel = pnlAllowAny });
                }
                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.Range))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.Range, Name = "Range", ComponentPanel = pnlRange });
                }
                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.Repeating))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.Repeating, Name = "Repeating", ComponentPanel = pnlRepeating });
                }

                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.DaysOfWeekFromLast))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.DaysOfWeekFromLast, Name = "Last Day of week", ComponentPanel = pnlWeekday, });
                }
                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.DaysOfMonthFromLast))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.DaysOfMonthFromLast, Name = "Last day of month", ComponentPanel = pnlFromEndOfMonth });
                }
                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.NthWeekday))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.NthWeekday, Name = "Nth Week day", ComponentPanel = pnlWeekNthOccurance });
                }
                if (_currentComponent.AllowedComponentTypes.Contains(CronComponentType.Ignored))
                {
                    componentTypeValues.Add(new CronComponentTypes() { Action = CronComponentType.Ignored, Name = "Ignored", ComponentPanel = pnlIgnore });
                }

                lstComponentType.Items.Clear();
                foreach (var type in componentTypeValues)
                {
                    lstComponentType.Items.Add(type);
                }

                CronComponentTypes selectedComponentType = componentTypeValues.FirstOrDefault(x => x.Action == _currentComponent.ComponentType);
                lstComponentType.SelectedItem = selectedComponentType;
            }

        }


        private void lstComponentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (var type in componentTypeValues)
            {
                if (lstComponentType.SelectedItem == type)
                {
                    type.ComponentPanel.Visible = true;
                    type.ComponentPanel.Dock = DockStyle.Fill;
                }
                else
                {
                    type.ComponentPanel.Visible = false;
                }
            }
        }

        private void CronPiece_Load(object sender, EventArgs e)
        {
            lstComponentType_SelectionChangeCommitted(sender, e);
        }
    }

    public class CronComponentTypes
    {
        public CronComponentType Action { get; set; }
        public string Name { get; set; }
        public Panel ComponentPanel { get; set; }

                public override string ToString()
        {
            return Name;
        }
    }
}
