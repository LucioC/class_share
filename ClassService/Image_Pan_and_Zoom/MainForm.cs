using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ImageZoom
{
    public partial class ImageZoomMainForm : Form
    {
        Image img;
        Point mouseDown;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;
        int imgx = 0;                         // current offset of image
        int imgy = 0;

        bool mousepressed = false;  // true as long as left mousebutton is pressed
        float zoom = 1;
        float angle = 0;

        public ImageZoomMainForm(String imagePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            //string imagefilename = @"..\..\test.tif";
            //string imagefilename = @"..\..\ponei.jpg";
            string imagefilename = imagePath;
                        
            img = Image.FromFile(imagefilename);

            Graphics g = this.CreateGraphics();

            // Fit whole image
            zoom = Math.Min(
                ((float)pictureBox.Height / (float)img.Height) * (img.VerticalResolution / g.DpiY),
                ((float)pictureBox.Width / (float)img.Width) * (img.HorizontalResolution / g.DpiX)
            );

            // Fit width
            //zoom = ((float)pictureBox.Width / (float)img.Width) * (img.HorizontalResolution / g.DpiX);

            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);
        }

        private void pictureBox_MouseMove(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                imgy = (int)(starty + (deltaY / zoom));

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
                    startx = imgx;
                    starty = imgy;
                }
            }
        }

        private void imageBox_MouseUp(object sender, EventArgs e)
        {
            mousepressed = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float oldzoom = zoom;

            if (e.Delta > 0)
            {
                zoom += 0.1F;
            }
            else if (e.Delta < 0)
            {
                zoom = Math.Max(zoom - 0.1F, 0.01F);
            }

            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
            int y = mousePosNow.Y - pictureBox.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / zoom);

            imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
            imgy = newimagey - oldimagey + imgy;

            pictureBox.Refresh();  // calls imageBox_Paint
        }

        protected void zoomPicture(float zoomDelta)
        {
            float oldzoom = zoom;

            if (zoomDelta > 0)
            {
                zoom += zoomDelta;
            }
            else if (zoomDelta < 0)
            {
                zoom = Math.Max(zoom + zoomDelta, 0.01F);
            }

            pictureBox.Refresh();  // calls imageBox_Paint
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(zoom, zoom);

            Image i = (Image)img.Clone();
            i = RotateImage(i, angle);

            e.Graphics.DrawImage(i, imgx, imgy);
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        imgx -= (int)(pictureBox.Width * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Left:
                        imgx += (int)(pictureBox.Width * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Down:
                        imgy -= (int)(pictureBox.Height * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Up:
                        imgy += (int)(pictureBox.Height * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageDown:
                        zoomPicture(-0.1f);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageUp:
                        zoomPicture(0.1f);
                        pictureBox.Refresh();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}