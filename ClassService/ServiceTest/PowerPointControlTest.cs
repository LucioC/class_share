using PowerPointPresentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServiceTest
{   
    /// <summary>
    ///This is a test class for PowerPointControlTest and is intended
    ///to contain all PowerPointControlTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PowerPointControlTest
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

        private static PowerPointControl powerPointControl;
        private static String correctFileName;

        [TestInitialize()]
        public void TestInitialize()
        {
            correctFileName = @"C:\Users\lucioc\Desktop\class_share\Samples\WCF\WcfService1\TestProject1\resources\PEP_posM.pptx";

            powerPointControl = new PowerPointControl();
            powerPointControl.PreparePresentation(correctFileName);
            powerPointControl.StartPresentation();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            powerPointControl.ClosePresentation();
        }

        /// <summary>
        ///A test for goToSlideNumber
        ///</summary>
        [TestMethod()]
        public void GoToSlideNumberTest()
        {
            int slideNumber = 5; 
            powerPointControl.GoToSlideNumber(slideNumber);

            Assert.AreEqual(slideNumber, powerPointControl.CurrentSlide());
        }

        /// <summary>
        ///A test for goToSlideNumber
        ///</summary>
        [TestMethod()]
        public void GoToFirstSlideTest()
        {
            powerPointControl.GoToFirstSlide();

            Assert.AreEqual(1, powerPointControl.CurrentSlide());
        }

        /// <summary>
        ///A test for goToSlideNumber
        ///</summary>
        [TestMethod()]
        public void GoToLastSlideTest()
        {
            powerPointControl.GoToLastSlide();

            Assert.AreEqual(powerPointControl.TotalSlides(), powerPointControl.CurrentSlide());
        }

        /// <summary>
        ///A test for goToSlideNumber
        ///</summary>
        [TestMethod()]
        public void GoToNextSlideTest()
        {
            powerPointControl.GoToFirstSlide();
            powerPointControl.GoToNextSlide();

            Assert.AreEqual(2, powerPointControl.CurrentSlide());
        }

        /// <summary>
        ///A test for goToSlideNumber
        ///</summary>
        [TestMethod()]
        public void GoToPrevioustSlideTest()
        {
            powerPointControl.GoToSlideNumber(5);
            powerPointControl.GoToPreviousSlide();

            Assert.AreEqual(4, powerPointControl.CurrentSlide());
        }

    }
}
