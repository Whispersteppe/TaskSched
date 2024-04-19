using System.Collections.ObjectModel;

namespace TaskSched.Component.Cron
{
    public interface ICronComponent
    {
        string GetPiece();
        CronComponentType ComponentType { get; }

        public List<CronComponentType> AllowedComponentTypes { get; }

        ReadOnlyCollection<int> AllowedRangeValues { get; }
        ReadOnlyCollection<int> Range { get; }

        void SetRange(IEnumerable<int> values);
        void RemoveRange(IEnumerable<int> values);
        void ClearRange();

        string Text { get; }

        int RepeatInterval { get;  }
        int RepeatStart { get; }

        int GetNext(int startAt = -1);
        public void SetRepeating(int repeatStart, int repeatInterval);
        void SetAllowAny();

        bool IsValid(DateTime currentDate);
    }
}
