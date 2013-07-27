﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ServiceCore.Utils;

namespace ImageZoom
{
    public class ImageUtils
    {

        public ImageUtils()
        {

        }

        public Image RotateImage(Image b, float angle)
        {
            //Make it positive
            while (angle < 0) angle = angle + 360;

            int l = b.Width;
            int h = b.Height;
            double an = angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(an));
            double sin = Math.Abs(Math.Sin(an));
            int nl = (int)(l * cos + h * sin);
            int nh = (int)(l * sin + h * cos);
            Bitmap returnBitmap = new Bitmap(nl, nh);
            Graphics g = Graphics.FromImage(returnBitmap);
            g.TranslateTransform((float)(nl - l) / 2, (float)(nh - h) / 2);
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }

        public Size ImageSize { get; set; }
        public Size BoxSize { get; set; }
        public Point Center { get; set; }

        public ImageState AdjustPositionAndScale(ref int left, ref int top, ref int right, ref int bottom, float otherImageScaleH, float otherImageScaleW)
        {
            left = (int)(left / otherImageScaleW);
            right = (int)(right / otherImageScaleW);
            top = (int)(top / otherImageScaleH);
            bottom = (int)(bottom / otherImageScaleH);

            Output.Debug("RECEIVED", left + ":" + top + ":" + right + ":" + bottom);
            Output.Debug("IMAGE", ImageSize.Width + ":" + ImageSize.Height);

            int width = right - left;
            int height = bottom - top;

            if (width > ImageSize.Width) width = ImageSize.Width;
            if (height > ImageSize.Height) height = ImageSize.Height;

            float scaleh = (float)BoxSize.Height / height;
            float scalew = (float)BoxSize.Width / width;

            float minScale = Math.Min(scaleh, scalew);
            //float minScale = scalew;

            Point center = Center;

            ImageState imageState = new ImageState();

            imageState.Zoom = minScale;
            imageState.X = -left;// +screenPosition.X + pictureBox.Margin.Left;
            imageState.Y = -top;// +screenPosition.Y + pictureBox.Margin.Top;

            int scaledWidth = (int)(width * minScale);
            int scaledHeight = (int)(height * minScale);

            int imageWidth = (int)(imageState.Zoom * ImageSize.Width);
            imageState = adjustCenter(minScale, scaledWidth, scaledHeight, imageState);

            return imageState;
        }

        private ImageState adjustCenter(float minScale, int scaledWidth, int scaledHeight, ImageState imageState)
        {
            int pictureWidth = BoxSize.Width;
            int pictureHeight = BoxSize.Height;

            float extrawidth = 0;
            float extraheight = 0;
            if (pictureWidth > scaledWidth)
            {
                extrawidth = (pictureWidth - scaledWidth) / 2;
                extrawidth = (extrawidth > 0) ? extrawidth / minScale : 0;
                if (extrawidth > 0) imageState.X += (int)extrawidth;
            }
            if (pictureHeight > scaledHeight)
            {
                extraheight = (pictureHeight - scaledHeight) / 2;
                extraheight = (extraheight > 0) ? extraheight / minScale : 0;
                if (extraheight > 0) imageState.Y += (int)extraheight;
            }

            return imageState;
        }
    }
}
