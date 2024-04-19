using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.FieldValidator;

namespace TaskScheduler.WinForm.Models
{
    public class EventActivityFieldModel : INotifyPropertyChanged
    {
        private string fieldValue;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Name of the field")]
        [Category("ID")]
        public string Name { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("The current value of the field")]
        [Category("ID")]
        public string Value
        {
            get => fieldValue;
            set
            {
                fieldValue = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("The type of field")]
        [DisplayName("Field Type")]
        [Category("ID")]
        public FieldTypeEnum FieldType { get; set; }



        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        public Guid ActivityFieldId { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            InstanceChanged = true;
        }

    }
}
