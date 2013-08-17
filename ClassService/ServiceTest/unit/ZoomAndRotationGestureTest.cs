using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using Moq;
using KinectPowerPointControl.Gesture;
using ServiceTest;

namespace TestProject.Units
{    
    /// <summary>
    ///This is a test class for ZoomAndRotationGestureTest and is intended
    ///to contain all ZoomAndRotationGestureTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZoomAndRotationGestureTest
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
        public void WhenPeopleIsRotatingThenZoomShouldNotBeTrigged()
        {
            var zoomRecognizer = new Mock<IGestureRecognizer>();
            var rotationRecognizer = new Mock<IRotationGripGesture>();

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(20f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);

            ZoomAndRotationGesture target = new ZoomAndRotationGesture(zoomRecognizer.Object, rotationRecognizer.Object);
            
            bool expected = false;
            bool actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WhenPeopleIsNotRotatingThenZoomCanBeTrigged()
        {
            var zoomRecognizer = new Mock<IGestureRecognizer>();
            var rotationRecognizer = new Mock<IRotationGripGesture>();

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(0f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);
            String eventName = GestureEvents.ZOOM_IN;
            zoomRecognizer.Setup(z => z.Name).Returns(eventName);

            ZoomAndRotationGesture target = new ZoomAndRotationGesture(zoomRecognizer.Object, rotationRecognizer.Object);

            bool expected = true;
            bool actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(eventName, target.Name);
        }

        [TestMethod()]
        public void WhenNoRotationOrNoZoomThenNothingIsRecognized()
        {
            var zoomRecognizer = new Mock<IGestureRecognizer>();
            var rotationRecognizer = new Mock<IRotationGripGesture>();

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(0f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);

            ZoomAndRotationGesture target = new ZoomAndRotationGesture(zoomRecognizer.Object, rotationRecognizer.Object);

            bool expected = false;
            bool actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GivenZoomWasIdentifiedWhenAngleChangeAndZoomStateIsStillTrackingThenZoomCouldBeTriggedAgainAndNoRotationShouldBeFired()
        {
            var zoomRecognizer = new Mock<IGestureRecognizer>();
            var rotationRecognizer = new Mock<IRotationGripGesture>();

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(0f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);
            zoomRecognizer.Setup(z => z.State).Returns(1);
            String eventName = GestureEvents.ZOOM_IN;
            zoomRecognizer.Setup(z => z.Name).Returns(eventName);

            ZoomAndRotationGesture target = new ZoomAndRotationGesture(zoomRecognizer.Object, rotationRecognizer.Object);

            bool expected = true;
            bool actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(eventName, target.Name);

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(65f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);
            rotationRecognizer.Setup(z => z.Name).Returns(GestureEvents.ROTATE_RIGHT);

            actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(eventName, target.Name );
        }

        [TestMethod()]
        public void GivenZoomWasIdentifiedWhenZoomStateIsResetedThenRotationCanBeFired()
        {
            var zoomRecognizer = new Mock<IGestureRecognizer>();
            var rotationRecognizer = new Mock<IRotationGripGesture>();

            ZoomAndRotationGesture target = new ZoomAndRotationGesture(zoomRecognizer.Object, rotationRecognizer.Object);

            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(0f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);
            zoomRecognizer.Setup(z => z.State).Returns(1);
            String eventName = GestureEvents.ZOOM_IN;
            zoomRecognizer.Setup(z => z.Name).Returns(eventName);

            bool expected = true;
            bool actual = target.IdentifyGesture(null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(eventName, target.Name);

            zoomRecognizer.Setup(z => z.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(false);
            zoomRecognizer.Setup(z => z.State).Returns(0);

            expected = false;
            actual = target.IdentifyGesture(null);
            Assert.AreEqual(expected, actual);

            eventName = GestureEvents.ROTATE_RIGHT;
            rotationRecognizer.Setup(r => r.CurrentAngleDelta).Returns(65f);
            rotationRecognizer.Setup(r => r.IdentifyGesture(It.IsAny<UserSkeletonState>())).Returns(true);
            rotationRecognizer.Setup(z => z.Name).Returns(eventName);
            actual = target.IdentifyGesture(null);

            expected = true;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(eventName, target.Name);
        }
    }
}
