using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public delegate void UpdateImageState(ImageState imageState);
    public class ImageState
    {
        public ImageState()
        {
            Active = true;
        }

        public float Zoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public int Angle { get; set; }
        public bool Active { get; set; }
    }
}
