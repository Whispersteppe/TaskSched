﻿namespace TaskScheduler.WinForm.Models
{
    public class CalendarRootModel : RootModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.CalendarRootItem;

        public CalendarRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            DisplayName = "Calendars";
            AllowedChildTypes = [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.EventItem];
        }




    }

}
