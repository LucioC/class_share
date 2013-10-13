using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using ServiceTest;

namespace TestProject.Units
{
    /// <summary>
    ///This is a test class for RotationGripGestureTest and is intended
    ///to contain all RotationGripGestureTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RotationGripGestureTest
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
        public void GivenRightHandStartAboveLeftWhenItGoBelowLeftThenRightRotationIsIdentifiedAndStateShouldStayAsExecutedTest()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.2f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ROTATE_RIGHT, target.Name);
            Assert.AreEqual(2, target.State);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 0.95f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.25f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            expected = false;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(2, target.State);
        }

        [TestMethod()]
        public void GivenLeftHandStartAboveRightWhenItGoBelowRightThenLeftRotationIsIdentifiedTest()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
            
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.2f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ROTATE_LEFT, target.Name);
        }
        
        [TestMethod()]
        public void GivenRightHandIsAboveLeftWhenAngleChangeMoreThan60DegreesToRightThenRightRotationIsIdentified()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1.1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ROTATE_RIGHT, target.Name);
        }

        [TestMethod()]
        public void GivenRightHandIsAboveLeftWhenAngleChangeLessThan60DegreesToRightThenRightRotationIsNotIdentified()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.1f, 1.1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GivenRightHandIsAboveLeftWhenAngleChangeMoreThan60DegreesToLeftThenLeftRotationIsIdentified()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1.5f, 1.2f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.2f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            expected = true;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(GestureEvents.ROTATE_LEFT, target.Name);
        }

        [TestMethod()]
        //Distance minimum equals 12cm
        public void ShouldNotIdentifyRotationIfHandsAreCloseInY()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.11f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.11f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ShouldNotIdentifyRotationIfRightHandIsNotGripped()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.2f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ShouldNotIdentifyRotationIfLeftHandIsNotGripped()
        {
            RotationGripGesture target = new RotationGripGesture();
            bool expected = false;
            bool actual;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1.2f, 0);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, false);
            Assert.AreEqual(expected, actual);

            //Closed hands
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, true, true);
            Assert.AreEqual(expected, actual);
            
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.2f, 0);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0);
            actual = target.IdentifyRotationGesture(rightHand, leftHand, spine, false, true);
            Assert.AreEqual(expected, actual);
        }
    }
}
