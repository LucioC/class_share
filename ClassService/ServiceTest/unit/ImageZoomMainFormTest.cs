using ImageZoom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestProject1
{   
    /// <summary>
    ///This is a test class for ImageZoomMainFormTest and is intended
    ///to contain all ImageZoomMainFormTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImageZoomMainFormTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        
        /// <summary>
        ///A test for RotateImage
        ///</summary>
        [TestMethod()]
        public void RotateImageTest()
        {
            string imagePath = string.Empty; // TODO: Initialize to an appropriate value
            ImageZoomMainForm target = new ImageZoomMainForm(imagePath); // TODO: Initialize to an appropriate value
            Image b = null; // TODO: Initialize to an appropriate value
            float angle = 0F; // TODO: Initialize to an appropriate value
            Image expected = null; // TODO: Initialize to an appropriate value
            Image actual;
            actual = target.RotateImage(b, angle);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for imageBox_MouseDown
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ImageZoom.exe")]
        public void imageBox_MouseDownTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImageZoomMainForm_Accessor target = new ImageZoomMainForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.imageBox_MouseDown(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for imageBox_MouseUp
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ImageZoom.exe")]
        public void imageBox_MouseUpTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImageZoomMainForm_Accessor target = new ImageZoomMainForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.imageBox_MouseUp(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for imageBox_Paint
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ImageZoom.exe")]
        public void imageBox_PaintTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImageZoomMainForm_Accessor target = new ImageZoomMainForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            PaintEventArgs e = null; // TODO: Initialize to an appropriate value
            target.imageBox_Paint(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for pictureBox_MouseMove
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ImageZoom.exe")]
        public void pictureBox_MouseMoveTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImageZoomMainForm_Accessor target = new ImageZoomMainForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.pictureBox_MouseMove(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for zoomPicture
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ImageZoom.exe")]
        public void zoomPictureTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImageZoomMainForm_Accessor target = new ImageZoomMainForm_Accessor(param0); // TODO: Initialize to an appropriate value
            float zoomDelta = 0F; // TODO: Initialize to an appropriate value
            target.zoomPicture(zoomDelta);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
