using ImageZoom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Windows.Forms;
using Moq;
using ServiceCore;

namespace TestProject1
{   
    /// <summary>
    ///This is a test class for ImageUtilsTest and is intended
    ///to contain all ImageUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImageUtilsTest
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

        #region squared_not_rotated
        [TestMethod()]
        public void imageShouldBeShownAllIfFitsAndNoShift()
        {
            ImageUtils target = new ImageUtils();
            int left = 0; 
            int top = 0;
            int right = 100; 
            int bottom = 100; 
            float otherImageScaleH = 1; 
            float otherImageScaleW = 1; 
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = 0;
            expected.Y = 0;
            expected.Zoom = 1f;

            target.ImageSize = new Size(100,100);
            target.BoxSize = new Size(100, 100);            

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeShiftedToRightAsRequestedAndCantFitAll()
        {
            ImageUtils target = new ImageUtils();
            int left = 100;
            int top = 0;
            int right = 200;
            int bottom = 100;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = 0;
            expected.Zoom = 1f;

            target.ImageSize = new Size(1000, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeShiftedToBottomAsRequestedAndCantFitAll()
        {
            ImageUtils target = new ImageUtils();
            int left = 0;
            int top = 100;
            int right = 100;
            int bottom = 200;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = 0;
            expected.Y = -top;
            expected.Zoom = 1f;
            expected.Angle = 0;

            target.ImageSize = new Size(100, 1000);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }
        
        [TestMethod()]
        public void imageShouldBeZommedIfDesiredPartCanBeShownBigger()
        {
            ImageUtils target = new ImageUtils();
            int left = 50;
            int top = 50;
            int right = 75;
            int bottom = 75;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = -top;
            expected.Zoom = 4f;

            target.ImageSize = new Size(100, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        #endregion

        #region squared_and_rotated

        [TestMethod()]
        public void imageShouldBeRotatedAsRequestedIfMultipleOf90()
        {
            ImageUtils target = new ImageUtils();
            int left = 0;
            int top = 0;
            int right = 100;
            int bottom = 100;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = 0;
            expected.Y = 0;
            expected.Zoom = 1f;
            expected.Angle = 180;

            target.ImageSize = new Size(100, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 180);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeShiftedRightSameWayWhenIsRotated180()
        {
            ImageUtils target = new ImageUtils();
            int left = 100;
            int top = 0;
            int right = 200;
            int bottom = 100;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = 0;
            expected.Zoom = 1f;
            expected.Angle = 180;

            target.ImageSize = new Size(1000, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 180);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

       
        [TestMethod()]
        public void imageShouldBeShiftedToBottomAsRequestedAndCantFitAllWhenRotated90()
        {
            ImageUtils target = new ImageUtils();
            int left = 0;
            int top = 100;
            int right = 100;
            int bottom = 200;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = 0;
            expected.Y = -top;
            expected.Zoom = 1f;
            expected.Angle = 90;

            target.ImageSize = new Size(100, 1000);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 90);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeZommedWithSamePositioningWhenRotated180()
        {
            ImageUtils target = new ImageUtils();
            int left = 50;
            int top = 50;
            int right = 75;
            int bottom = 75;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = -top;
            expected.Zoom = 4f;
            expected.Angle = 180;

            target.ImageSize = new Size(100, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 180);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeZommedWithSamePositioningWhenRotated90()
        {
            ImageUtils target = new ImageUtils();
            int left = 50;
            int top = 50;
            int right = 75;
            int bottom = 75;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = -top;
            expected.Zoom = 4f;
            expected.Angle = 90;

            target.ImageSize = new Size(100, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 90);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        #endregion

        [TestMethod()]
        public void imageShouldBeShiftedToTwiceRightWhenScaledWidthIsDoubled()
        {
            ImageUtils target = new ImageUtils();
            int left = 100;
            int top = 0;
            int right = 300;
            int bottom = 200;
            float otherImageScaleH = 2f;
            float otherImageScaleW = 2f;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -50;
            expected.Y = 0;
            expected.Zoom = 1f;
            expected.Angle = 0;

            target.ImageSize = new Size(200, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageShouldBeShiftedToTwiceBottomWhenScaledRightIsDoubled()
        {
            ImageUtils target = new ImageUtils();
            int left = 0;
            int top = 100;
            int right = 200;
            int bottom = 300;
            float otherImageScaleH = 2f;
            float otherImageScaleW = 2f;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = 0;
            expected.Y = -50;
            expected.Zoom = 1f;
            expected.Angle = 0;

            target.ImageSize = new Size(100, 200);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }
        
        [TestMethod()]
        public void imageShouldBeZommedWithSamePositioningWhenRotated90AndIsNotSquared()
        {
            ImageUtils target = new ImageUtils();
            int left = 50;
            int top = 50;
            int right = 75;
            int bottom = 75;
            float otherImageScaleH = 1;
            float otherImageScaleW = 1;
            ImageState actual;
            ImageState expected = new ImageState();
            expected.X = -left;
            expected.Y = -top;
            expected.Zoom = 4f;
            expected.Angle = 90;

            target.ImageSize = new Size(200, 100);
            target.BoxSize = new Size(100, 100);

            actual = target.AdjustPositionAndScale(ref left, ref top, ref right, ref bottom, otherImageScaleH, otherImageScaleW, 90);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.Zoom, actual.Zoom);
            Assert.AreEqual(expected.Angle, actual.Angle);
        }

        [TestMethod()]
        public void imageWidthMultiplierShouldBeTwoIfWidthIsTwiceTheOriginalImage()
        {
            ImageUtils target = new ImageUtils();

            float expectedMultiplier = 2;
            float actualMultiplier = target.MultiplierToSameWidth(100, 200);

            Assert.AreEqual(expectedMultiplier, actualMultiplier);
        }

        [TestMethod()]
        public void imageWidthMultiplierShouldBeHalfIfWidthTwiceTheHalfImage()
        {
            ImageUtils target = new ImageUtils();

            float expectedMultiplier = 0.5f;
            float actualMultiplier = target.MultiplierToSameWidth(100, 50);

            Assert.AreEqual(expectedMultiplier, actualMultiplier);
        }

        [TestMethod()]
        public void imageHeightMultiplierShouldBeHalfIfHeightTwiceTheHalfImage()
        {
            ImageUtils target = new ImageUtils();

            float expectedMultiplier = 0.5f;
            float actualMultiplier = target.MultiplierToSameHeight(100, 50);

            Assert.AreEqual(expectedMultiplier, actualMultiplier);
        }

        [TestMethod()]
        public void imageHeightMultiplierShouldBeTwoIfHeightIsTwiceTheOriginalImage()
        {
            ImageUtils target = new ImageUtils();

            float expectedMultiplier = 2;
            float actualMultiplier = target.MultiplierToSameWidth(100, 200);

            Assert.AreEqual(expectedMultiplier, actualMultiplier);
        }

        [TestMethod()]
        public void imageSizeMultipliersShouldBeTwiceIfOtherImageIsTwiceTheSize()
        {
            ImageUtils target = new ImageUtils();
            Size imageSize = new Size(200,100);
            Size otherImageSize = new Size(400,200);

            float[] multipliers = target.MultipliersToSameSize(imageSize, otherImageSize,0);

            Assert.AreEqual(2, multipliers[0]);
            Assert.AreEqual(2, multipliers[1]);
        }

        [TestMethod()]
        public void imageSizeComesRotated90DegreesAsIndicatedMultipliersShouldCompareCorrectDimensions()
        {
            ImageUtils target = new ImageUtils();
            Size imageSize = new Size(200, 100); //width, height zero degrees
            Size otherImageSize = new Size(200, 100); //width,height 90 degrees, turn as height, width, zero degrees

            float[] multipliers = target.MultipliersToSameSize(imageSize, otherImageSize, 90);

            Assert.AreEqual(2, multipliers[0]);
            Assert.AreEqual(0.5f, multipliers[1]);
        }

        [TestMethod()]
        public void imageSizeMultipliersShouldCompareCorrectDimensions()
        {
            ImageUtils target = new ImageUtils();
            Size imageSize = new Size(200, 100); 
            Size otherImageSize = new Size(200, 100); 

            float[] multipliers = target.MultipliersToSameSize(imageSize, otherImageSize, 0);

            Assert.AreEqual(1, multipliers[0]);
            Assert.AreEqual(1, multipliers[1]);
        }
    }
}
