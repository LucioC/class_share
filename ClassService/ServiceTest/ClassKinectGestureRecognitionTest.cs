using KinectPowerPointControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ClassKinectGestureRecognitionTest and is intended
    ///to contain all ClassKinectGestureRecognitionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClassKinectGestureRecognitionTest
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
        ///A test for ClassKinectGestureRecognition Constructor
        ///</summary>
        [TestMethod()]
        public void ClassKinectGestureRecognitionAddGestureTest()
        {
            IGestureRecognizer gestureRecognition = new RightGestureRecognizer();
            ClassKinectGestureRecognition target = new ClassKinectGestureRecognition();
            target.AddGestureRecognition(gestureRecognition);

            ArrayList list = target.Gestures;
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        ///A test for ClassKinectGestureRecognition
        ///</summary>
        [TestMethod()]
        public void ClassKinectGestureRecognitionAddTwoDifferentGesturesTest()
        {
            IGestureRecognizer rightGesture = new RightGestureRecognizer();
            IGestureRecognizer leftGesture = new LeftGestureRecognizer();
            ClassKinectGestureRecognition target = new ClassKinectGestureRecognition();
            target.AddGestureRecognition(rightGesture);
            target.AddGestureRecognition(leftGesture);

            ArrayList list = target.Gestures;
            Assert.AreEqual(2, list.Count);
        }

        /// <summary>
        ///A test for ClassKinectGestureRecognition
        ///</summary>
        [TestMethod()]
        public void ClassKinectGestureRecognitionAddTwoEqualGesturesTest()
        {
            IGestureRecognizer rightGesture1 = new RightGestureRecognizer();
            IGestureRecognizer rightGesture2 = new RightGestureRecognizer();
            ClassKinectGestureRecognition target = new ClassKinectGestureRecognition();
            target.AddGestureRecognition(rightGesture1);
            target.AddGestureRecognition(rightGesture2);

            ArrayList list = target.Gestures;
            Assert.AreEqual(1, list.Count);
        }
    }
}
