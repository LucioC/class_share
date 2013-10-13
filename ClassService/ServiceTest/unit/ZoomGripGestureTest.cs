using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using Moq;
using Microsoft.Kinect;
using ServiceTest;

namespace TestProject.Units
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

        [TestMethod()]
        public void IdentifyZoomInGesture()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
            
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1.1f, 1, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ZOOM_IN, target.Name);
        }


        [TestMethod()]
        public void IdentifyZoomInGestureDoneDiagnally()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1.049f, 1.049f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ZOOM_IN, target.Name);
        }
        
        [TestMethod()]
        public void DoNotIdentifyZoomInGestureDoneDiagnallyIfDistanceWasLessThan5cm()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1.03f, 1.03f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IdentifyZoomOutGesture()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.9f, 1, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.1f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ZOOM_OUT, target.Name);
        }

        [TestMethod()]
        public void ShouldNotIdentiftZoomOutIfHandsAreNotGripped()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.9f, 1, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.1f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ShouldNotIdentiftZoomIntIfHandsAreNotGripped()
        {
            ZoomGripGesture target = new ZoomGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.9f, 1, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.1f, 1, 0);
            actual = target.IdentifyZoomGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
        }
    }
}
