using System.Collections.ObjectModel;

namespace TaskSched.Component.Cron
{
    public interface ICronComponent
    {
        string GetPiece();
        CronComponentType ComponentType { get; }

        void SetAllowAny();

        List<int> AllowedRangeValues { get; }
        int RepeatInterval { get; set; }
        int RepeatStart { get; set; }
        ObservableCollection<int> Range { get; }

        int GetNext(int startAt = -1);

        bool IsValid(DateTime currentDate);
    }
}
