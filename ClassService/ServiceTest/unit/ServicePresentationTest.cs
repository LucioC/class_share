﻿using ClassService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.IO;
using ImageZoom;
using KinectPowerPointControl;
using Moq;
using ServiceCore;
using ServiceTest;

namespace TestProject.Units
{    
    /// <summary>
    ///This is a test class for ServiceTest and is intended
    ///to contain all ServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServicePresentationTest
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
        ///A test for Service Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceConstructorTest()
        {
            Service target = new Service();
            Assert.IsNotNull(Service.KinectWindow);
            Assert.IsNotNull(Service.ImageForm);
            Assert.IsNotNull(Service.PresentationControl);
        }

        #region PresentationTest

        [TestMethod()]
        public void StartPresentationTest()
        {
            Service target = CommonTest.CreateAMockedService();

            var presentationControlMock = new Mock<IPowerPointControl>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.PresentationControl = presentationControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            ClassService.File file = new ClassService.File("");
            target.PreparePresentation(file);
            target.StartPresentation();

            presentationControlMock.Verify(x => x.PreparePresentation(It.IsAny<String>()), Times.Exactly(1));
            presentationControlMock.Verify(x => x.StartPresentation(), Times.Exactly(1));
            kinectControlMock.Verify(x => x.StartThread(), Times.Exactly(1));
        }
        
        [TestMethod()]
        public void PresentationNextCommand()
        {
            Service target = CommonTest.CreateAMockedService();

            var presentationControlMock = new Mock<IPowerPointControl>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.PresentationControl = presentationControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            ClassService.PresentationAction action = new ClassService.PresentationAction(ClassService.PresentationAction.NEXT);
            target.PresentationCommand(action);

            presentationControlMock.Verify(x => x.GoToNextSlide(), Times.Exactly(1));
        }

        [TestMethod()]
        public void PresentationPreviousCommand()
        {
            Service target = CommonTest.CreateAMockedService();

            var presentationControlMock = new Mock<IPowerPointControl>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();

            Service.PresentationControl = presentationControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;

            ClassService.PresentationAction action = new ClassService.PresentationAction(ClassService.PresentationAction.PREVIOUS);
            target.PresentationCommand(action);

            presentationControlMock.Verify(x => x.GoToPreviousSlide(), Times.Exactly(1));
        }

        #endregion
    }
}
