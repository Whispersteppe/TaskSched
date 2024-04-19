using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.FieldValidator;

namespace TaskScheduler.WinForm.Models
{
    public class ActivityFieldModel : INotifyPropertyChanged
    {

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("the name of the field")]
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

        string fieldValue = "";

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("the value of the field")]
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

        [ReadOnly(false)]
        [Browsable(true)]
        [DisplayName("Field Type")]
        [Description("the type of field")]
        [Category("ID")]
        public FieldTypeEnum FieldType 
        { 
            get => fieldType;
            set
            {
                fieldType = value;
                OnPropertyChanged();
            }
        }


        [ReadOnly(false)]
        [Browsable(true)]
        [DisplayName("Read only")]
        [Description("indicator that this field cannot be removed")]
        [Category("ID")]
        public bool IsReadOnly { get; set; } //  read only makes it fixed for the activity, such as an executable path.

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool instanceChanged;
        private string name;
        private FieldTypeEnum fieldType;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged
        {
            get => instanceChanged;
            set => instanceChanged = value;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InstanceChanged = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public override string ToString()
        {
            return Name;
        }

    }

}
