using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

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

        ImageUtils imageUtils;

        public ImageZoomMainForm(String imagePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            string imagefilename = imagePath;

            //FromFile function was locking image file to be used a second time
            //img = Image.FromFile(imagefilename);
            Image image = Image.FromFile(imagefilename);

            float factorX = (image.Width > 1024) ? (float)image.Width/1024 : 1;
            float factorY = (image.Height > 740) ? (float)image.Height/740 : 1;
            float factor = Math.Max(factorX, factorY);

            int newWidth = (int)(image.Width / factor);
            int newHeight = (int)(image.Height / factor);

            using (var bmpTemp = new Bitmap(image, newWidth, newHeight))
            {
                img = new Bitmap(bmpTemp);
            }
                        
            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);

            imageUtils = new ImageUtils();

            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();

            Graphics g = this.CreateGraphics();
            centralizeImage(g.DpiX, g.DpiY);
        }

        private void centralizeImage(float dpiy, float dpix)
        {
            int centerX = pictureBox.Width / 2 - img.Width / 2;
            int centery = pictureBox.Height / 2 - img.Height / 2;
            imgx += centerX;
            imgy += centery;
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
            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;
            zoomPicture(e.Delta, mousePosNow);
        }

        public void addToX(int x)
        {
            imgx += x;
            pictureBox.Refresh();
        }

        public void addToY(int y)
        {
            imgy += y;
            pictureBox.Refresh();
        }

        public void zoomPicture(float zoomDelta, Point pointerPosition)
        {
            float oldzoom = zoom;

            if (zoomDelta > 0)
            {
                zoom = Math.Min(zoom + 0.1F, 10F);
            }
            else if (zoomDelta < 0)
            {
                zoom = Math.Max(zoom - 0.1F, 0.5F);
            }

            int x = pointerPosition.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
            int y = pointerPosition.Y - pictureBox.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / zoom);

            imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
            imgy = newimagey - oldimagey + imgy;

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
            e.Graphics.ScaleTransform(zoom, zoom);

            Image i = (Image)img.Clone();
            i = imageUtils.RotateImage(i, angle);           
            
            e.Graphics.DrawImage(i, imgx, imgy);
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
                        zoomPicture(-0.1f, new Point(imgx, imgy));
                        pictureBox.Refresh();
                        break;

                    case Keys.PageUp:
                        zoomPicture(0.1f, new Point(imgx, imgy));
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
    }
}