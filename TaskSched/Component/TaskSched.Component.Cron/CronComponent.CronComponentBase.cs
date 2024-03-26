using System.Collections.ObjectModel;

namespace TaskSched.Component.Cron
{
    /// <summary>
    /// base component for all of the individual pieces
    /// </summary>
    public class CronComponentBase : ICronComponent
    {
        #region internal fields

        protected List<int> _allowedValues = new List<int>();
        protected List<int> _rangeValues = new List<int>();
        int _repeatInterval;
        int _repeatStart;

        #endregion


        public CronComponentBase(int allowedRangeStart, int allowedRangeEnd) :
            this("*", allowedRangeStart, allowedRangeEnd)
        { }

        /// <summary>
        /// constructor for the base class
        /// </summary>
        /// <param name="value">the incoming cron segment</param>
        /// <param name="allowedRangeStart">the start of the allowed value range</param>
        /// <param name="allowedRangeEnd">the end of the allowed value range</param>
        public CronComponentBase(string value, int allowedRangeStart, int allowedRangeEnd)
        {
            _allowedValues.Clear();
            for (int i = allowedRangeStart; i <= allowedRangeEnd; i++)
            {
                _allowedValues.Add(i);
            }

            DecodeIncomingValue(value);

        }


        /// <summary>
        /// the type of component - range, allow any, and so on.
        /// </summary>
        public CronComponentType ComponentType { get; internal set; }

        /// <summary>
        /// the set of values that are allowed on this component
        /// </summary>
        public ReadOnlyCollection<int> AllowedRangeValues
        {
            get
            {
                return new ReadOnlyCollection<int>(_allowedValues);
            }
        }

        /// <summary>
        /// The set of items selected for calculating the next value
        /// </summary>
        public ReadOnlyCollection<int> Range
        {
            get
            {
                return new  ReadOnlyCollection<int>(_rangeValues);
            }
        }

        /// <summary>
        /// given a string, decode it.  
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// this handles the base cases for everything - any, range, iteration. other types are handled in the individual
        /// classes themselves.
        /// </remarks>
        internal virtual void DecodeIncomingValue(string value)
        {
            value = value.ToUpper();

            if (value == "*")
            {
                ComponentType = CronComponentType.AllowAny;
            }
            else if (value.Contains("/"))
            {
                string[] pieces = value.Split('/');
                if (pieces.Length == 2)
                {
                    SetRepeating(int.Parse(pieces[0]), int.Parse(pieces[1]));
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
                List<int> itemsToAdd = new List<int>();

                foreach (string item in items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Contains('-'))
                        {
                            string[] rangeParts = item.Split("-");
                            int startAt = int.Parse(rangeParts[0]);
                            int endAt = int.Parse(rangeParts[1]);

                            for (int i = startAt; i <= endAt; i++)
                            {
                                itemsToAdd.Add(i);
                            }
                        }
                        else
                        {
                            itemsToAdd.Add(int.Parse(item));
                        }
                    }
                }

                SetRange(itemsToAdd);
            }
        }

        /// <summary>
        /// set a repeating range
        /// </summary>
        /// <param name="repeatStart"></param>
        /// <param name="repeatInterval"></param>
        /// <exception cref="Exception"></exception>
        public virtual void SetRepeating(int repeatStart, int repeatInterval)
        {
            if (AllowedRangeValues.Contains(repeatStart) == false)
            {
                throw new Exception($"RepeatStart value is not allowed - {repeatStart}");
            }

            if (repeatInterval <= 0)
            {
                throw new Exception($"RepeatInterval must be greater than zero - {repeatInterval}");
            }

            _repeatInterval = repeatInterval;
            _repeatStart = repeatStart;
            ComponentType = CronComponentType.Repeating;
            SetRepeatingRange();

        }

        /// <summary>
        /// the repeat interval
        /// </summary>
        public int RepeatInterval
        {
            get
            {
                return _repeatInterval;
            }

        }

        /// <summary>
        /// the start of the repeat
        /// </summary>
        public int RepeatStart
        {
            get
            {
                return _repeatStart;

            }
        }

        public List<CronComponentType> AllowedComponentTypes { get; } = new List<CronComponentType>();

        /// <summary>
        /// internal function to set a repeating range.  
        /// </summary>
        /// <remarks>
        /// we've got this separated from the SetRepeating so that we can use it within the 
        /// Decode method.
        /// </remarks>
        private void SetRepeatingRange()
        {
            if ( _repeatInterval > 0)
            {
                List<int> rangeParts = new List<int>();

                int currentRangeItem = _repeatStart;

                while ( currentRangeItem < AllowedRangeValues.Max() )
                {
                    rangeParts.Add(currentRangeItem);
                    currentRangeItem += _repeatInterval;
                }

                AddRangeItems(rangeParts);
            }
        }

        /// <summary>
        /// internal class used to decode from-to ranges
        /// </summary>
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

        /// <summary>
        /// return the string representation of this segment of the cron string
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// get the next value based in the component type
        /// </summary>
        /// <param name="startAt"></param>
        /// <returns></returns>
        /// <remarks>
        /// this will work within a particular segment.  if the next item falls outside the accepted range, then 
        /// a -1 is returned.
        /// </remarks>
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

        /// <summary>
        /// makes this component an '*'
        /// </summary>
        public virtual void SetAllowAny()
        {
            ComponentType = CronComponentType.AllowAny;
        }

        /// <summary>
        /// checks validity of the date against this component
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <remarks>
        /// this will need to be overridden in a child class.
        /// TODO possibly make this an abstract class
        /// </remarks>
        public virtual bool IsValid(DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// the internal addition of range items
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// this allows reuse from the SetRange and Decode methods
        /// </remarks>
        private void AddRangeItems(IEnumerable<int> values)
        {
            foreach (var value in values)
            {
                if (AllowedRangeValues.Contains(value) == false)
                {
                    throw new Exception($"Value is not allowed - {value}");
                }

                if (_rangeValues.Contains(value) == false)
                {
                    _rangeValues.Add(value);
                }
            }

            _rangeValues.Sort();
        }

        /// <summary>
        /// sets the range 
        /// </summary>
        /// <param name="values"></param>
        public void SetRange(IEnumerable<int> values)
        {
            AddRangeItems(values);
            ComponentType = CronComponentType.Range;
        }

        /// <summary>
        /// internal remove the given items from the range
        /// </summary>
        /// <param name="values"></param>
        private void RemoveRangeItems(IEnumerable<int> values)
        {
            foreach (var value in values)
            {
                if (_rangeValues.Contains(value) == true)
                {
                    _rangeValues.Remove(value);
                }
            }

            _rangeValues.Sort();
        }

        /// <summary>
        /// remove items from the range
        /// </summary>
        /// <param name="values"></param>
        public void RemoveRange(IEnumerable<int> values)
        {
            RemoveRangeItems(values);
            ComponentType = CronComponentType.Range;
        }

        /// <summary>
        /// clears all items from the range.
        /// </summary>
        public void ClearRange()
        {
            _rangeValues.Clear();
        }
    }
}
