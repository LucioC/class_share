﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using ServiceCore.Utils;

namespace ImageZoom
{
    public partial class ImageZoomMainForm : Form
    {
        Image img;
        Point mouseDown;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;

        ImageState imageState;

        bool mousepressed = false;  // true as long as left mousebutton is pressed
        float angle = 0;

        ImageUtils imageUtils;

        public ImageZoomMainForm(String imagePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            imageState = new ImageState();
            imageState.Zoom = 1f;
            imageState.X = 0;
            imageState.Y = 0;
                        
            string imagefilename = imagePath;
            loadImage(imagefilename);

            imageUtils = new ImageUtils();

            imageUtils.Center = getCenterPoint();
            imageUtils.Box = pictureBox;
            imageUtils.TargetImage = img;
                        
            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);

            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();

            Graphics g = this.CreateGraphics();

            setViewMinimumBounds(0,0,img.Width, img.Height, img.Height, img.Width, 0);
        }

        private void loadImage(string imagefilename)
        {
            //FromFile function was locking image file to be used a second time
            //img = Image.FromFile(imagefilename);
            Bitmap image = null;
            using (var bmpTemp = new Bitmap(imagefilename))
            {
                image = new Bitmap(bmpTemp);
            }

            float factorX = (image.Width > 1024) ? (float)image.Width / 1024 : 1;
            float factorY = (image.Height > 768) ? (float)image.Height / 768 : 1;
            float factor = Math.Max(factorX, factorY);
            factor = (float)(int)(factor);

            int newWidth = (int)(image.Width / factor);
            int newHeight = (int)(image.Height / factor);

            using (var bmpTemp = new Bitmap(image, newWidth, newHeight))
            {
                img = new Bitmap(bmpTemp);
            }
        }

        private void centralizeImage(float dpiy, float dpix)
        {
            int centerX = pictureBox.Width / 2 - img.Width / 2;
            int centery = pictureBox.Height / 2 - img.Height / 2;
            imageState.X += centerX;
            imageState.Y += centery;
            pictureBox.Refresh();
        }

        private void pictureBox_MouseMove(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imageState.X = (int)(startx + (deltaX / imageState.Zoom));  // calculate new offset of image based on the current zoom factor
                imageState.Y = (int)(starty + (deltaY / imageState.Zoom));

                pictureBox.Refresh();
            }
        }

        private void imageBox_MouseDown(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (!mousepressed)
                {
                    mousepressed = true;
                    mouseDown = mouse.Location;
                    startx = imageState.X;
                    starty = imageState.Y;
                }
            }
        }

        private void imageBox_MouseUp(object sender, EventArgs e)
        {
            mousepressed = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;
            zoomPicture(e.Delta, mousePosNow);
        }

        public void setPositionAndZoom(int x, int y, float zoom)
        {
            setZoom(zoom);
            imageState.X = x;
            imageState.Y = y;
            pictureBox.Refresh();
        }

        public Point getCenterPoint()
        {
            return new Point(Width / 2, Height / 2);
        }

        public void addToX(int x)
        {
            imageState.X += x;
            pictureBox.Refresh();
        }

        public void addToY(int y)
        {
            imageState.Y += y;
            pictureBox.Refresh();
        }

        public void setZoom(float newZoom)
        {
            imageState.Zoom = newZoom;
        }

        public void zoomPicture(float zoomDelta, Point pointerPosition)
        {
            float oldzoom = imageState.Zoom;

            if (zoomDelta > 0)
            {
                imageState.Zoom = Math.Min(imageState.Zoom + 0.1F, 10F);
            }
            else if (zoomDelta < 0)
            {
                imageState.Zoom = Math.Max(imageState.Zoom - 0.1F, 0.5F);
            }

            int x = pointerPosition.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
            int y = pointerPosition.Y - pictureBox.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / imageState.Zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / imageState.Zoom);

            imageState.X = newimagex - oldimagex + imageState.X;  // Where to move image to keep focus on one point
            imageState.Y = newimagey - oldimagey + imageState.Y;

            pictureBox.Refresh();  // calls imageBox_Paint
        }

        public void setAngle(float newAngle)
        {
            this.angle = newAngle;
            pictureBox.Refresh();
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(imageState.Zoom, imageState.Zoom);
            e.Graphics.TranslateTransform(imageState.X, imageState.Y);

            Image i = (Image)img.Clone();
            i = imageUtils.RotateImage(i, angle);
            
            e.Graphics.DrawImage(i, 1, 1);
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        imageState.X -= (int)(pictureBox.Width * 0.1F / imageState.Zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Left:
                        imageState.X += (int)(pictureBox.Width * 0.1F / imageState.Zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Down:
                        imageState.Y -= (int)(pictureBox.Height * 0.1F / imageState.Zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Up:
                        imageState.Y += (int)(pictureBox.Height * 0.1F / imageState.Zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageDown:
                        zoomPicture(-0.1f, new Point(imageState.X, imageState.Y));
                        pictureBox.Refresh();
                        break;

                    case Keys.PageUp:
                        zoomPicture(0.1f, new Point(imageState.X, imageState.Y));
                        pictureBox.Refresh();
                        break;

                    case Keys.End:
                        angle += 90F;
                        pictureBox.Refresh();
                        break;

                    case Keys.Home:
                        angle -= 90F;
                        pictureBox.Refresh();
                        break;

                    case Keys.Escape:
                        this.Close();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ImageZoomMainForm_Load(object sender, EventArgs e)
        {

        }

        private void ImageZoomMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            img.Dispose();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        public void setViewMinimumBounds(int left, int top, int right, int bottom, int otherImageHeight, int otherImageWidth, int rotation)
        {
            //Adjust difference in sizes from this and image source
            float otherImageScaleH = ((float)otherImageHeight / (float)img.Height);
            float otherImageScaleW = ((float)otherImageWidth / (float)img.Width);
            rotation = adjustAngle(rotation);

            
            if (rotation == 0 || rotation == 180)
            {
                imageState = imageUtils.adjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            }
            else
            {
                imageState = imageUtils.adjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleW, otherImageScaleH);
            }

            pictureBox.Refresh();

            //Output.Debug("POSITION", imgx + ":" + imgy);
            //Output.Debug("EXTRA", extrawidth + ":" + extraheight);
            //Output.Debug("SCALE", zoom + "");
        }

        private int adjustAngle(int rotation)
        {
            while (rotation < 0) rotation += 360;
            while (rotation > 360) rotation -= 360;

            if (rotation == 180)
            {
                this.angle = 180;
            }
            else if (rotation == 90 || rotation == 270)
            {
                this.angle = rotation;
            }
            return rotation;
        }
    }
}