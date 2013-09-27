using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ServiceCore;

namespace ImageZoom
{
    public class ImageCalcHelper
    {
        public ImageState CalculateZoomedAndRotatedImageState(ImageState imageState)
        {
            ImageState zoomedImageState = new ImageState();

            zoomedImageState.X = (int)(imageState.X * imageState.Zoom);
            zoomedImageState.Y = (int)(imageState.Y * imageState.Zoom);
            zoomedImageState.ScreenHeight = imageState.ScreenHeight;
            zoomedImageState.ScreenWidth = imageState.ScreenWidth;

            float angle = imageState.Angle;
            while (angle > 360) angle = angle - 360;
            while (angle < 0) angle = angle + 360;

            if (angle == 90 || angle == 270)
            {
                zoomedImageState.Height = (int)(imageState.Width * imageState.Zoom);
                zoomedImageState.Width = (int)(imageState.Height * imageState.Zoom);
            }
            else
            {
                zoomedImageState.Height = (int)(imageState.Height * imageState.Zoom);
                zoomedImageState.Width = (int)(imageState.Width * imageState.Zoom);
            }

            //zoomedImageState.Top = (int)(imageState.Top * imageState.Zoom);
            //zoomedImageState.Right = (int)(imageState.Right * imageState.Zoom);
            //zoomedImageState.Left = (int)(imageState.Left * imageState.Zoom);
            //zoomedImageState.Bottom = (int)(imageState.Right * imageState.Zoom);

            zoomedImageState.Zoom = 1;

            return zoomedImageState;
        }

    }
}
