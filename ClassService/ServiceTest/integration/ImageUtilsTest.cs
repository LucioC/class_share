﻿using ImageZoom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using ServiceTest;

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



        /// <summary>
        ///A test for RotateImage
        ///</summary>
        [TestMethod()]
        public void RotateImageTest()
        {
            string imagePath = CommonTest.GetFileResourcePath("poneis_malditos.jpg");
            ImageUtils target = new ImageUtils();
            Image b = Image.FromFile(imagePath);
            float angle = 90F;

            Image actual;
            actual = target.RotateImage(b, angle);

            //Verify if dimensions were rotated
            Assert.AreEqual(b.Height, actual.Width);
            Assert.AreEqual(b.Width, actual.Height);
        }
    }
}
