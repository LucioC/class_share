using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using ServiceCore.Utils;
using ServiceCore;
using System.Globalization;

namespace ImageZoom
{
    public partial class ImageZoomMainForm : Form
    {
        Image img;
        Point mouseDown;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;
        
        //TODO this class is not fully used here, maybe create an specialized one
        ImageState imageState;

        bool mousepressed = false;  // true as long as left mousebutton is pressed

        ImageUtils imageUtils;

        private float maxZoom = 10;
        private float minZoom = 0.5f;

        InterceptKeyboard interceptor;
        protected event UpdateImageState UpdateImage;

        public void AddListenerForUpdateImageEvent(UpdateImageState UpdateEventCallback)
        {
            this.UpdateImage += UpdateEventCallback;
        }

        public ImageZoomMainForm(String imagePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            imageState = new ImageState();
            imageState.Zoom = 1f;
            imageState.X = 0;
            imageState.Y = 0;
            imageState.Angle = 0;
            
            string imagefilename = imagePath;
            loadImage(imagefilename);
            imageState.Width = img.Width;
            imageState.Height = img.Height;
                        
            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);

            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();
            
            imageUtils = new ImageUtils();
            imageUtils.Center = getCenterPoint();
            imageUtils.BoxSize = pictureBox.Size;
            imageUtils.ImageSize = img.Size;

            Graphics g = this.CreateGraphics();

            setViewMinimumBounds(0,0,img.Width, img.Height, img.Height, img.Width, 0);

            interceptor = new InterceptKeyboard();
            InterceptKeyboard.SetHook(interceptor.hook);
            interceptor.KeyEvent += keyDown;

            zoomLabel.Text = "zoom is: " + imageState.Zoom;
        }

        public void sendImageStateUpdateForListeners(ImageState imageState)
        {
            if (this.UpdateImage != null)
            {
                this.UpdateImage.BeginInvoke(imageState, null, null);
            }
        }

        public void keyDown(int keyCode)
        {
            if(!this.Focused)
            {
                Message msg = new Message();
                msg.Msg = 0x100;
                ProcessCmdKey(ref msg, (Keys)keyCode);
            }
        }

        private void loadImage(string imagefilename)
        {
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
            pictureBox.Refresh();
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

        ImageCalcHelper calcHelper = new ImageCalcHelper();

        public void addToX(int x)
        {
            imageState.X += x;
        }

        public void keepInsideBounds(ImageState zoomedImageState)
        {
            Graphics graphics = pictureBox.CreateGraphics();
            if (zoomedImageState.Height > pictureBox.Height)
            {
                if (zoomedImageState.Height + zoomedImageState.Y < graphics.VisibleClipBounds.Height)
                {
                    imageState.Y = (int)((graphics.VisibleClipBounds.Height - zoomedImageState.Height) / imageState.Zoom);
                }
                if (imageState.Y > 0)
                {
                    imageState.Y = 0;
                }
            }
            else
            {
                //If height is less than screen width, keep centralized
                int excess = (int)((imageState.ScreenHeight - zoomedImageState.Height) / imageState.Zoom);
                imageState.Y = (int)(excess / 2);
            }

            if (zoomedImageState.Width > pictureBox.Width)
            {
                if (zoomedImageState.Width + zoomedImageState.X < graphics.VisibleClipBounds.Width)
                {
                    imageState.X = (int)((graphics.VisibleClipBounds.Width - zoomedImageState.Width) / imageState.Zoom);
                }
                if (imageState.X > 0)
                {
                    imageState.X = 0;
                }
            }
            else
            {
                //If width is less than screen width, keep centralized
                int excess = (int)((imageState.ScreenWidth - zoomedImageState.Width) / imageState.Zoom);
                imageState.X = (int)(excess / 2);
            }
        }

        public void addToY(int y)
        {
            imageState.Y += y;
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
                float newZoom = imageState.Zoom + zoomDelta;
                imageState.Zoom = Math.Min(newZoom, maxZoom);
            }
            else if (zoomDelta < 0)
            {
                float newZoom = imageState.Zoom + zoomDelta;
                imageState.Zoom = Math.Max(newZoom, minZoom);
            }

            zoomLabel.Text = "zoom is: " + imageState.Zoom;

            int x = pointerPosition.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
            int y = pointerPosition.Y - pictureBox.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / imageState.Zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / imageState.Zoom);

            imageState.X = newimagex - oldimagex + imageState.X;  // Where to move image to keep focus on one point
            imageState.Y = newimagey - oldimagey + imageState.Y;
        }

        public void setAngle(int newAngle)
        {
            imageState.Angle = newAngle;
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(imageState.Zoom, imageState.Zoom);
            e.Graphics.TranslateTransform(imageState.X, imageState.Y);

            Image i = (Image)img.Clone();
            i = imageUtils.RotateImage(i, imageState.Angle);

            e.Graphics.DrawImage(i, 0, 0);
        }
        
        public void CommandExecutor(String command, String param)
        {
            if (command == ServiceCommands.IMAGE_SET_VISIBLE_PART)
            {
                String[] paramArray = param.Split(Char.Parse(":"));
                int left = 0;
                int bottom = 0;
                int right = 0;
                int top = 0;
                int imageH = 0;
                int imageW = 0;
                int rotation = 0;
                if (paramArray.Length > 1)
                {
                    left = Int32.Parse(paramArray[0]);
                    top = Int32.Parse(paramArray[1]);
                    right = Int32.Parse(paramArray[2]);
                    bottom = Int32.Parse(paramArray[3]);
                    imageH = Int32.Parse(paramArray[4]);
                    imageW = Int32.Parse(paramArray[5]);
                    rotation = Int32.Parse(paramArray[6]);
                }
                this.Invoke((MethodInvoker)delegate
                {
                    this.setViewMinimumBounds(left, top, right, bottom, imageH, imageW, rotation);
                });           
            }
            else
            {
                float value = float.Parse(param, CultureInfo.InvariantCulture);
                ProcessCommand(command, value);
            }
        }

        public void ProcessCommand(String command, float value)
        {
            imageState.Width = img.Width;
            imageState.Height = img.Height;
            imageState.ScreenHeight = pictureBox.Height;
            imageState.ScreenWidth = pictureBox.Width;

            switch (command)
            {
                case ServiceCommands.IMAGE_MOVE_X:
                    addToX((int)(pictureBox.Width * value / imageState.Zoom));
                    break;

                case ServiceCommands.IMAGE_MOVE_Y:
                    addToY((int)(pictureBox.Height * value / imageState.Zoom));
                    break;

                case ServiceCommands.IMAGE_ZOOM:
                    zoomPicture(value, new Point(pictureBox.Width/2, pictureBox.Height/2));
                    break;

                case ServiceCommands.IMAGE_ROTATE:
                    imageState.Angle += (int)value;
                    break;

                case ServiceCommands.CLOSE_IMAGE:
                    ImageState closedState = new ImageState();
                    closedState.Active = false;
                    sendImageStateUpdateForListeners(closedState);
                    return;
            }

            ImageState zoomedImageState = calcHelper.CalculateZoomedAndRotatedImageState(imageState);
            keepInsideBounds(zoomedImageState);
            
            sendImageStateUpdateForListeners(GetImageState());

            Invoke((MethodInvoker)delegate
            {
                pictureBox.Refresh();
            });         
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            imageState.Width = img.Width;
            imageState.Height = img.Height;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        ProcessCommand(ServiceCommands.IMAGE_MOVE_X, -0.1f);
                        break;

                    case Keys.Left:
                        ProcessCommand(ServiceCommands.IMAGE_MOVE_X, 0.1f);
                        break;

                    case Keys.Down:
                        ProcessCommand(ServiceCommands.IMAGE_MOVE_Y, -0.1f);
                        break;

                    case Keys.Up:
                        ProcessCommand(ServiceCommands.IMAGE_MOVE_Y, 0.1f);
                        break;

                    case Keys.PageDown:
                        ProcessCommand(ServiceCommands.IMAGE_ZOOM, -0.1f);
                        break;

                    case Keys.PageUp:
                        ProcessCommand(ServiceCommands.IMAGE_ZOOM, 0.1f);
                        break;

                    case Keys.End:
                        ProcessCommand(ServiceCommands.IMAGE_ROTATE, 90);
                        break;

                    case Keys.Home:
                        ProcessCommand(ServiceCommands.IMAGE_ROTATE, -90);
                        break;

                    case Keys.Escape:
                        ProcessCommand(ServiceCommands.CLOSE_IMAGE, 0f);
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        public ImageState GetImageState()
        {
            ImageState imageState = new ImageState(this.imageState);

            Size imageSize = img.Size;
            //if image is rotated its dimensions are inverted
            if (imageState.Angle == 90 || imageState.Angle == 270)
            {
                imageSize.Height = img.Width;
                imageSize.Width = img.Height;
            }

            RectangleF displayArea = pictureBox.CreateGraphics().VisibleClipBounds;

            int zoomedHeight = (int)(imageSize.Height * imageState.Zoom);
            int zoomedWidth = (int)(imageSize.Width * imageState.Zoom);

            int zoomedX = (int)(imageState.X * imageState.Zoom);
            int zoomedY = (int)(imageState.Y * imageState.Zoom);

            imageState.Left = (zoomedX >= 0) ? 0 : (int)-imageState.X;
            imageState.Right = (displayArea.Width >= zoomedWidth + zoomedX) ? imageSize.Width : (int)((displayArea.Width - zoomedX)/imageState.Zoom);

            imageState.Top = (zoomedY >= 0) ? 0 : (int)-imageState.Y;
            imageState.Bottom = (displayArea.Height >= zoomedHeight + zoomedY) ? imageSize.Height : (int)((displayArea.Height - zoomedY)/imageState.Zoom);

            imageState.ScreenHeight = (int)displayArea.Height;
            imageState.ScreenWidth = (int)displayArea.Width;

            this.imageState = new ImageState(imageState);
            this.imageState.Width = img.Width;
            this.imageState.Height = img.Height;
            
            return imageState;
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
            Output.Debug("SetView", left + ":" + top + ":" + right + ":" + bottom);

            Size otherImageSize = new Size(otherImageWidth, otherImageHeight);

            imageState = imageUtils.adjustAngle(rotation, imageState);

            float[] dimensionMultiplier = imageUtils.MultipliersToSameSize(img.Size, otherImageSize, imageState.Angle);
            imageState = imageUtils.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, dimensionMultiplier[0], dimensionMultiplier[1], rotation);

            pictureBox.Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}