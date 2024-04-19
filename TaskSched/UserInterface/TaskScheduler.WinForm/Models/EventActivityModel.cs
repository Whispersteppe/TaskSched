using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{
    [TypeConverter(typeof(EventActivityModelConverter))]
    public class EventActivityModel : INotifyPropertyChanged
    {
        private string name;
        private bool instanceChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("Local name of the activity")]
        [Category("ID")]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }


        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        //        [TypeConverter(typeof(ActivityIdConverter))]
        public Guid ActivityId { get; set; }


        [ReadOnly(false)]
        [Browsable(true)]
        [DisplayName("Activity Name")]
        [Description("The activity handler that will be taking care of this activity.  You can select any one of the activities.")]
        [TypeConverter(typeof(ActivityConverter))]
        public string ActivityName
        {
            get
            {
                var items = ScheduleManager.GlobalInstance.GetActivities().Result;
                var selectedItem = items.FirstOrDefault(x => x.ID == ActivityId);
                return selectedItem.Name;
            }
            set
            {
                var items = ScheduleManager.GlobalInstance.GetActivities().Result;
                var selectedItem = items.FirstOrDefault(x => x.Name == value);
                ActivityId = selectedItem.ID.Value;
                OnPropertyChanged();

                //TODO will also need to reswizzle fields as needed
            }


        }


        [ReadOnly(false)]
        [Browsable(false)]
        [Description("The set of fields that are associated with this activity. These are set in the request.")]
//        [TypeConverter(typeof(EventActivityFieldConverter))]
//https://stackoverflow.com/questions/30878928/properly-display-list-of-custom-objects-in-windows-forms-property-grid
        [Category("Fields")]
        public List<EventActivityFieldModel>? Fields { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged
        {
            get
            {
                if (instanceChanged == true) return true;
                foreach (var field in Fields)
                {
                    if (field.InstanceChanged == true) return true;
                }

                return false;
            }
            set
            {
                instanceChanged = value;
                if (Fields != null)
                {
                    foreach (var field in Fields)
                    {
                        field.InstanceChanged = value;
                    }
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            InstanceChanged = true;
        }
    }

}
