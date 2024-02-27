

using Newtonsoft.Json;

namespace TaskScheduler.WinForm.Configuration
{
    public class WindowCoordinates
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 820;
        public int Height { get; set; } = 500;

        [JsonIgnore]
        public Point Location 
        { 
            get { return new Point(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        [JsonIgnore]
        public Size Size 
        { 
            get { return new Size(Width, Height);} 
            set { Width = value.Width; Height = value.Height; }
        }

    }
}
