using TaskSched.Common.FieldValidator;

namespace TaskSched.Common.DataModel
{
    public class EventActivityField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public FieldTypeEnum FieldType { get; set; }


        public Guid ActivityFieldId { get; set; }

    }

}
