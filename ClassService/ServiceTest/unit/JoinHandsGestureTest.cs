using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using ServiceTest;

namespace TestProject.Units
{    
    
    /// <summary>
    ///This is a test class for JoinHandsGestureTest and is intended
    ///to contain all JoinHandsGestureTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JoinHandsGestureTest
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
        public void IdentifyGestureTest()
        {
            JoinHandsGesture target = new JoinHandsGesture();

            //Hands together
            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0.5f);
            bool isRightHandGripped = false;
            bool isLeftHandGripped = false;
            IJoint rightShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.5f);
            IJoint leftShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.5f);
            bool expected = false;
            bool actual;
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //Open hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);
            
            //Join hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ShouldNotIdentifyGestureIfHandsAreTooCloseToSpine()
        {
            JoinHandsGesture target = new JoinHandsGesture();

            //Hands together
            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0.04f);
            bool isRightHandGripped = false;
            bool isLeftHandGripped = false;
            IJoint rightShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.04f);
            IJoint leftShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.04f);
            bool expected = false;
            bool actual;
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //Open hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //Join hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ShouldIdentifyGestureIfHandsOpenOnMiddleOfJoinMovement()
        {
            JoinHandsGesture target = new JoinHandsGesture();

            //Hands together
            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0.04f);
            bool isRightHandGripped = false;
            bool isLeftHandGripped = false;
            IJoint rightShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.04f);
            IJoint leftShoulder = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0.04f);
            bool expected = false;
            bool actual;
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //Open hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //middle of movement with hands closed 
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.7f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.3f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, true, true, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);

            //Join hands
            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 1f, 0f);
            actual = target.IdentifyGesture(rightHand, leftHand, isRightHandGripped, isLeftHandGripped, spine, rightShoulder, leftShoulder);
            Assert.AreEqual(expected, actual);
        }
    }
}
