using System.Collections.ObjectModel;

namespace TaskSched.Component.Cron
{
    public class CronComponentBase : ICronComponent
    {
        public CronComponentType ComponentType { get; internal set; }
        public List<int> AllowedRangeValues { get; protected set; } = new List<int>();
        public ObservableCollection<int> Range { get; } = new ObservableCollection<int>();


        int _repeatInterval;
        int _repeatStart;


        public CronComponentBase(int allowedRangeStart, int allowedRangeEnd) :
            this("*", allowedRangeStart, allowedRangeEnd)
        { }

        public CronComponentBase(string value, int allowedRangeStart, int allowedRangeEnd)
        {
            AllowedRangeValues.Clear();
            for (int i = allowedRangeStart; i <= allowedRangeEnd; i++)
            {
                AllowedRangeValues.Add(i);
            }

            Range.CollectionChanged += Range_CollectionChanged;
            DecodeIncomingValue(value);

        }



        internal virtual void DecodeIncomingValue(string value)
        {
            if (value == "*")
            {
                ComponentType = CronComponentType.AllowAny;
            }
            else if (value.Contains("/"))
            {
                string[] pieces = value.Split('/');
                if (pieces.Length == 2)
                {
                    _repeatStart = int.Parse(pieces[0]);
                    _repeatInterval = int.Parse(pieces[1]);
                    ComponentType = CronComponentType.Repeating;
                    SetRepeatingRange();
                }
                else
                {
                    throw new Exception($"Cannot recognize the piece - {value}");
                }
            }
            else
            {
                ComponentType = CronComponentType.Range;
                string[] items = value.Split(',');
                foreach (string item in items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Contains("-"))
                        {
                            string[] rangeParts = item.Split("-");
                            int startAt = int.Parse(rangeParts[0]);
                            int endAt = int.Parse(rangeParts[1]);

                            for (int i = startAt; i <= endAt; i++)
                            {
                                Range.Add(i);
                            }
                        }
                        else
                        {
                            Range.Add(int.Parse(item));
                        }
                    }
                }
            }
        }

        public int RepeatInterval
        {
            get
            {
                return _repeatInterval;
            }
            set
            {
                _repeatInterval = value;
                ComponentType = CronComponentType.Repeating;
                SetRepeatingRange();
            }
        }

        public int RepeatStart
        {
            get
            {
                return _repeatStart;

            }
            set
            {
                _repeatStart = value;
                ComponentType = CronComponentType.Repeating;
                SetRepeatingRange();
            }
        }

        private void SetRepeatingRange()
        {
            if ( _repeatInterval > 0)
            {
                Range.CollectionChanged -= Range_CollectionChanged;
                Range.Clear();
                int currentRangeItem = _repeatStart;
                while ( currentRangeItem < AllowedRangeValues.Max() )
                {
                    Range.Add( currentRangeItem );
                    currentRangeItem += _repeatInterval;
                }
                Range.CollectionChanged += Range_CollectionChanged;
            }
        }


        private void Range_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Range.CollectionChanged -= Range_CollectionChanged;

            ComponentType = CronComponentType.Range;

            Range.CollectionChanged += Range_CollectionChanged;
        }

        class FromTo
        {
            public int FromValue { get; set; } = 0;
            public int ToValue { get; set; }

            public override string ToString()
            {
                if (FromValue == ToValue)
                {
                    return $"{FromValue}";
                }
                else
                {
                    return $"{FromValue}-{ToValue}";
                }
            }
        }

        public virtual string GetPiece()
        {
            string piece;

            switch (ComponentType)
            {
                case CronComponentType.Repeating:
                    piece = $"{_repeatStart}/{_repeatInterval}";
                    break;

                case CronComponentType.AllowAny:
                    piece = "*";
                    break;

                case CronComponentType.Range:
                    {
                        List<int> range = Range.ToList();
                        range.Sort();

                        List<FromTo> spannedRangeItems = new List<FromTo>();

                        FromTo currentSpan = new FromTo() { FromValue = range[0], ToValue = range[0] };
                        spannedRangeItems.Add(currentSpan);

                        for (int i = 1; i < range.Count; i++)
                        {
                            if (range[i] == currentSpan.ToValue + 1)
                            {
                                currentSpan.ToValue = range[i];
                            }
                            else
                            {
                                currentSpan = new FromTo() { FromValue = range[i], ToValue = range[i] };
                                spannedRangeItems.Add(currentSpan);
                            }
                        }

                        piece = string.Join(",", spannedRangeItems);
                    }
                    break;
                default:
                    piece = "*";
                    break;
            }
            return piece;
        }

        public virtual int GetNext(int startAt = -1)
        {
            List<int> allowedValues = new List<int>();
            switch (ComponentType)
            {
                case CronComponentType.Repeating:
                    int currentValue = RepeatStart;
                    while (currentValue <= AllowedRangeValues.Max())
                    {
                        allowedValues.Add(currentValue);
                        currentValue += RepeatInterval;
                    }
                    break;
                case CronComponentType.AllowAny:
                    allowedValues.AddRange(AllowedRangeValues);
                    break;
                case CronComponentType.Range:
                    {
                        allowedValues.AddRange(this.Range);
                    }
                    break;
                default:
                    allowedValues.AddRange(AllowedRangeValues);
                    break;
            }

            //  now find the next
            allowedValues.Sort();
            for (int i = 0; i < allowedValues.Count; i++)
            {
                if (allowedValues[i] > startAt)
                {
                    return allowedValues[i];
                }
            }

            return -1;  //  not found

        }

        public virtual void SetAllowAny()
        {
            ComponentType = CronComponentType.AllowAny;
        }

        public virtual bool IsValid(DateTime currentDate)
        {
            throw new NotImplementedException();
        }
    }
}
