using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PictureBox_Zoom
{
    public partial class MainForm : Form
    {
        #region Constructor

        /// <summary>
        /// Default constructor for MainForm
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Synchronize some private members with the form's values.
            _ZoomFactor = trbZoomFactor.Value;
            _BackColor = picImage.BackColor;

            // Set the sizemode of both pictureboxes. These modes are important
            // to the functionality and should not be changed.
            picImage.SizeMode = PictureBoxSizeMode.CenterImage;
            picZoom.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        #endregion // Constructor

        #region Private members

        /// <summary>
        /// Stores the zoomfactor of the picZoom picturebox
        /// </summary>
        private int _ZoomFactor;
        /// <summary>
        /// Stores the color used to fill any areas not covered by an image
        /// </summary>
        private Color _BackColor;
        /// <summary>
        /// Stores an instance of the originally loaded image
        /// </summary>
        private Image _OriginalImage;

        #endregion // Private members

        #region Control Event Handlers

        /// <summary>
        /// Shows an OpenFileDialog to let the user select an image to load.
        /// </summary>
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.AddExtension = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Supported Image File|*.jpg;*.jpeg;*.bmp;*.png;*.dib;*.gif";
            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowHelp = true;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = "Select an image...";
            openFileDialog.ValidateNames = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _OriginalImage = Image.FromFile(openFileDialog.FileName);
                    ResizeAndDisplayImage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured loading the image " +
                                    openFileDialog.FileName + "\r\n" +
                                    ex.Message +
                                    "Please ensure you select a supported image type.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }

            openFileDialog.Dispose();
        }

        /// <summary>
        /// Shows a color picker to set the background color.
        /// It will redraw the image to match the new background color.
        /// </summary>
        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.Color = _BackColor;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.SolidColorOnly = false;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _BackColor = colorDialog.Color;
                ResizeAndDisplayImage();
            }

            colorDialog.Dispose();
        }

        /// <summary>
        /// Set the _ZoomFactor value to match the trbZoomFactor control's value
        /// and display the selected value to the user
        /// </summary>
        private void trbZoomFactor_ValueChanged(object sender, EventArgs e)
        {
            _ZoomFactor = trbZoomFactor.Value;
            lblZoomFactor.Text = string.Format("x{0}", _ZoomFactor);
        }

        /// <summary>
        /// When the mouse is moved over the picImage picturebox, the picZoom
        /// picturebox must reflect the change and adjust its image to the portion
        /// of the image the mouse is over
        /// </summary>
        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            // If no picture is loaded, return
            if (picImage.Image == null)
                return;

            UpdateZoomedImage(e);
        }

        #endregion // Control Event Handlers

        #region Private Methods

        /// <summary>
        /// Resizes the image stored in _OriginalImage to fit in picImage,
        /// maintaining the aspect ratios and displays it.
        /// </summary>
        private void ResizeAndDisplayImage()
        {
            // Set the backcolor of the pictureboxes
            picImage.BackColor = _BackColor;
            picZoom.BackColor = _BackColor;

            // If _OriginalImage is null, then return. This situation can occur
            // when a new backcolor is selected without an image loaded.
            if (_OriginalImage == null)
                return;

            picImage.Image = _OriginalImage;
            return;
            // sourceWidth and sourceHeight store the original image's width and height
            // targetWidth and targetHeight are calculated to fit into the picImage picturebox.
            int sourceWidth = _OriginalImage.Width;
            int sourceHeight = _OriginalImage.Height;
            int targetWidth;
            int targetHeight;
            double ratio;

            // Calculate targetWidth and targetHeight, so that the image will fit into
            // the picImage picturebox without changing the proportions of the image.
            if (sourceWidth > sourceHeight)
            {
                // Set the new width
                targetWidth = picImage.Width;
                // Calculate the ratio of the new width against the original width
                ratio = (double)targetWidth / sourceWidth;
                // Calculate a new height that is in proportion with the original image
                targetHeight = (int)(ratio * sourceHeight);
            }
            else if (sourceWidth < sourceHeight)
            {
                // Set the new height
                targetHeight = picImage.Height;
                // Calculate the ratio of the new height against the original height
                ratio = (double)targetHeight / sourceHeight;
                // Calculate a new width that is in proportion with the original image
                targetWidth = (int)(ratio * sourceWidth);
            }
            else
            {
                // In this case, the image is square and resizing is easy
                targetHeight = picImage.Height;
                targetWidth = picImage.Width;
            }

            // Calculate the targetTop and targetLeft values, to center the image
            // horizontally or vertically if needed
            int targetTop = (picImage.Height - targetHeight) / 2;
            int targetLeft = (picImage.Width - targetWidth) / 2;
            
            // Create a new temporary bitmap to resize the original image
            // The size of this bitmap is the size of the picImage picturebox.
            Bitmap tempBitmap = new Bitmap(picImage.Width, picImage.Height, PixelFormat.Format24bppRgb);

            // Set the resolution of the bitmap to match the original resolution.
            tempBitmap.SetResolution(_OriginalImage.HorizontalResolution, _OriginalImage.VerticalResolution);

            // Create a Graphics object to further edit the temporary bitmap
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // First clear the image with the current backcolor
            bmGraphics.Clear(_BackColor);

            // Set the interpolationmode since we are resizing an image here
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the original image on the temporary bitmap, resizing it using
            // the calculated values of targetWidth and targetHeight.
            bmGraphics.DrawImage(_OriginalImage,
                                 new Rectangle(targetLeft, targetTop, targetWidth, targetHeight),
                                 new Rectangle(0, 0, sourceWidth, sourceHeight),
                                 GraphicsUnit.Pixel);

            // Dispose of the bmGraphics object
            bmGraphics.Dispose();

            // Set the image of the picImage picturebox to the temporary bitmap
            picImage.Image = tempBitmap;
        }

        /// <summary>
        /// Updates the picZoom image to show the portion of the main image
        /// the mouse is currently over.
        /// </summary>
        private void UpdateZoomedImage(MouseEventArgs e)
        {
            // Calculate the width and height of the portion of the image we want
            // to show in the picZoom picturebox. This value changes when the zoom
            // factor is changed.
            int zoomWidth = picZoom.Width / _ZoomFactor;
            int zoomHeight = picZoom.Height / _ZoomFactor;

            // Calculate the horizontal and vertical midpoints for the crosshair
            // cursor and correct centering of the new image
            int halfWidth = zoomWidth / 2;
            int halfHeight = zoomHeight / 2;

            // Create a new temporary bitmap to fit inside the picZoom picturebox
            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);

            // Create a temporary Graphics object to work on the bitmap
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // Clear the bitmap with the selected backcolor
            bmGraphics.Clear(_BackColor);

            // Set the interpolation mode
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the portion of the main image onto the bitmap
            // The target rectangle is already known now.
            // Here the mouse position of the cursor on the main image is used to
            // cut out a portion of the main image.
            bmGraphics.DrawImage(picImage.Image,
                                 new Rectangle(0, 0, zoomWidth, zoomHeight),
                                 new Rectangle(e.X - halfWidth, e.Y - halfHeight, zoomWidth, zoomHeight),
                                 GraphicsUnit.Pixel);

            // Draw the bitmap on the picZoom picturebox
            picZoom.Image = tempBitmap;

            // Draw a crosshair on the bitmap to simulate the cursor position
            bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight - 4, halfWidth + 1, halfHeight - 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight + 6, halfWidth + 1, halfHeight + 3);
            bmGraphics.DrawLine(Pens.Black, halfWidth - 4, halfHeight + 1, halfWidth - 1, halfHeight + 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 6, halfHeight + 1, halfWidth + 3, halfHeight + 1);

            // Dispose of the Graphics object
            bmGraphics.Dispose();

            // Refresh the picZoom picturebox to reflect the changes
            picZoom.Refresh();
        }

        #endregion // Private Methods
    }
}