using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using Moq;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ZoomGripGestureTest and is intended
    ///to contain all ZoomGripGestureTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZoomGripGestureTest
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
        ///A test for IdentifyGesture
        ///</summary>
        [TestMethod()]
        public void IdentifyGestureTest()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            UserSkeletonState userState = null;
            bool expected = false;
            bool actual;

            var skeletonMock = new Mock<ISkeleton>();

            actual = target.IdentifyGesture(userState);


            Assert.AreEqual(expected, actual);
        }
    }
}
