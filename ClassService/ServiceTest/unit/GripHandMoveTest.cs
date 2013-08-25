using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using ServiceTest;

namespace TestProject.Units
{
    
    
    /// <summary>
    ///This is a test class for GripHandMoveTest and is intended
    ///to contain all GripHandMoveTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GripHandMoveTest
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
        public void WhenRightHandIsGrippedAndLeftHandIsRestedThenMoveCanHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.State);
        }
        
        [TestMethod()]
        public void WhenRightHandIsGrippedAndLeftHandIsOnFrontThenMoveCantHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, -0.2f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, target.State);
        }

        [TestMethod()]
        public void WhenRightHandIsNotGrippedThenMoveCantHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, false);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, target.State);
        }

        [TestMethod()]
        public void GivenRightHandIsGrippedLeftHandIsRestedWhenRightHandMoveToTheRight10cmThenMoveHasHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.State);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1.1f, 1f, -0.2f);
            expected = true;
            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(GestureEvents.MOVE_RIGHT, target.Name);
        }
        
        [TestMethod()]
        public void GivenRightHandIsGrippedLeftHandIsRestedWhenRightHandMoveToTheLeft10cmThenMoveHasHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.State);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(0.9f, 1f, -0.2f);
            expected = true;
            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(GestureEvents.MOVE_LEFT, target.Name);
        }

        [TestMethod()]
        public void GivenRightHandIsGrippedLeftHandIsRestedWhenRightHandMoveToUp10cmThenMoveHasHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.State);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1.1f, 0.2f);
            expected = true;
            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(GestureEvents.MOVE_UP, target.Name);
        }
        
        [TestMethod()]
        public void GivenRightHandIsGrippedLeftHandIsRestedWhenRightHandMoveDown10cmThenMoveHasHappen()
        {
            GripHandMove target = new GripHandMove();

            bool actual;
            bool expected = false;

            var rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 1, -0.2f);
            var leftHand = CommonTest.CreateDummyJointWithSkeletonPoint(0f, 1, 0f);
            var spine = CommonTest.CreateDummyJointWithSkeletonPoint(0.5f, 0.5f, 0);

            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.State);

            rightHand = CommonTest.CreateDummyJointWithSkeletonPoint(1f, 0.9f, -0.2f);
            expected = true;
            actual = target.IdentifyGrippedMove(rightHand, leftHand, spine, true);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(GestureEvents.MOVE_DOWN, target.Name);
        }
    }
}
