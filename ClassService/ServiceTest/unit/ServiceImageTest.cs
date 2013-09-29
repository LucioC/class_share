using ClassService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.IO;
using Moq;
using ServiceCore;
using KinectPowerPointControl;
using ServiceTest;

namespace TestProject.Units
{    
    /// <summary>
    ///This is a test class for ServiceTest and is intended
    ///to contain all ServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceImageTest
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
        ///A test for CloseCurrentImage
        ///</summary>
        [TestMethod()]
        public void CloseImageWindowTest()
        {
            Service target = CommonTest.CreateAMockedService();

            var imageControlMock = new Mock<IImageService>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            target.CloseCurrentImage();

            imageControlMock.Verify(x => x.StopThread(), Times.Exactly(1));
            kinectControlMock.Verify(x => x.StopThread(), Times.Exactly(1));
        }
        
        [TestMethod()]
        public void OpenImageTest()
        {
            Service target = CommonTest.CreateAMockedService();

            var imageControlMock = new Mock<IImageService>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            target.OpenImage(new ClassService.File(""));

            imageControlMock.Verify(x => x.StartThread(), Times.Exactly(1));
            kinectControlMock.Verify(x => x.StartThread(), Times.Exactly(1));
        }

        [TestMethod()]
        public void OpenSecondImageShouldCloseFirstTest()
        {
            Service target = CommonTest.CreateAMockedService();

            var imageControlMock = new Mock<IImageService>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            //If a thread is already running, it should be closed
            imageControlMock.Setup(foo => foo.IsThreadRunning()).Returns(true);

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            target.OpenImage(new ClassService.File(""));

            //Verify if running thread was closed
            imageControlMock.Verify(x => x.StopThread(), Times.Exactly(1));
            imageControlMock.Verify(x => x.StartThread(), Times.Exactly(1));
            kinectControlMock.Verify(x => x.StartThread(), Times.Exactly(1));
        }

        [TestMethod()]
        public void ImageCommandTest()
        {
            Service target = CommonTest.CreateAMockedService();

            var imageControlMock = new Mock<IImageService>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            ImageAction action = new ImageAction(ServiceCommands.IMAGE_ZOOM);
            target.ImageCommand(action);
        }

    }
}
