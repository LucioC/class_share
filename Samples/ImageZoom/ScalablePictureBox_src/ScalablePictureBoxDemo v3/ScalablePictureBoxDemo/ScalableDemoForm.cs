using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace QAlbum
{
    public partial class ScalableDemoForm : Form
    {
        public ScalableDemoForm()
        {
            InitializeComponent();
            postInitialize();
        }

        private void postInitialize()
        {
            this.scalablePictureBox.Picture = Util.GetImageFromEmbeddedResource("QAlbum.samplePicture.jpg");
            this.ActiveControl = this.scalablePictureBox.PictureBox;
        }

        private void loadPictureButton_Click(object sender, EventArgs e)
        {
            if ( this.openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            String imageFileName = this.openFileDialog.FileName;
            try
            {
                this.scalablePictureBox.Picture = Image.FromFile(imageFileName);
                this.ActiveControl = this.scalablePictureBox.PictureBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}