namespace QAlbum
{
    partial class ScalableDemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadPictureButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.scalablePictureBox = new QAlbum.ScalablePictureBox();
            this.SuspendLayout();
            // 
            // loadPictureButton
            // 
            this.loadPictureButton.Location = new System.Drawing.Point(12, 3);
            this.loadPictureButton.Name = "loadPictureButton";
            this.loadPictureButton.Size = new System.Drawing.Size(75, 23);
            this.loadPictureButton.TabIndex = 1;
            this.loadPictureButton.Text = "Load Picture";
            this.loadPictureButton.UseVisualStyleBackColor = true;
            this.loadPictureButton.Click += new System.EventHandler(this.loadPictureButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "Image files(*.jpg, *.png, *.gif)|*.jpg;*.png;*.gif|All files(*.*)|*.*";
            // 
            // pictureBoxMediator
            // 
            this.scalablePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scalablePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scalablePictureBox.Location = new System.Drawing.Point(0, 43);
            this.scalablePictureBox.Name = "pictureBoxMediator";
            this.scalablePictureBox.Size = new System.Drawing.Size(650, 273);
            this.scalablePictureBox.TabIndex = 2;
            // 
            // ScalableDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 318);
            this.Controls.Add(this.scalablePictureBox);
            this.Controls.Add(this.loadPictureButton);
            this.Name = "ScalableDemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadPictureButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private ScalablePictureBox scalablePictureBox;
    }
}

