using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class DefaultsConfig
    {
        public string NewTaskCronExpression { get; set; }
        public WindowPositionConfig MainWindow { get; set; } = new WindowPositionConfig() { Height = 400, Width = 800, Left = 100, Top = 100 };
    }


    public class WindowPositionConfig
    {
        public float Height { get; set; } = 400;
        public float Width { get; set; } = 800;
        public float Left { get; set; } = 100;
        public float Top { get; set; } = 100;
    }
}
