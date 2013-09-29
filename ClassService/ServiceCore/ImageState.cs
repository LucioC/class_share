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

        public ImageState(ImageState otherState)
        {
            this.Active = otherState.Active;
            this.Angle = otherState.Angle;
            this.Bottom = otherState.Bottom;
            this.Height = otherState.Height;
            this.Left = otherState.Left;
            this.Right = otherState.Right;
            this.ScreenHeight = otherState.ScreenHeight;
            this.ScreenWidth = otherState.ScreenWidth;
            this.Top = otherState.Top;
            this.Width = otherState.Width;
            this.X = otherState.X;
            this.Y = otherState.Y;
            this.Zoom = otherState.Zoom;
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
