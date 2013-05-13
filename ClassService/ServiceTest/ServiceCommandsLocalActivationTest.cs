using KinectPowerPointControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ServiceCore;
using Moq;
using ServiceTest;

namespace TestProject1
{
    /// <summary>
    ///This is a test class for ServiceCommandsLocalActivationTest and is intended
    ///to contain all ServiceCommandsLocalActivationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceCommandsLocalActivationTest
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
        ///A test for ProcessCloseImage
        ///</summary>
        [TestMethod()]
        public void ProcessCloseImageTest()
        {
            var listener = new Mock<DefaultCommunicator>();
            MessageEvent messagesListeners = null;
            messagesListeners += listener.Object.ReceiveMessage;

            ServiceCommandsLocalActivation target = new ServiceCommandsLocalActivation(messagesListeners);
            target.ProcessCloseImage();

            CommonTest.WaitForEventAsyncInvoke();

            listener.Verify(foo => foo.ReceiveMessage(ServiceCommands.CLOSE_IMAGE));
        }

        /// <summary>
        ///A test for ProcessClosePresentation
        ///</summary>
        [TestMethod()]
        public void ProcessClosePresentationTest()
        {
            var listener = new Mock<DefaultCommunicator>();
            MessageEvent messagesListeners = null;
            messagesListeners += listener.Object.ReceiveMessage;

            ServiceCommandsLocalActivation target = new ServiceCommandsLocalActivation(messagesListeners); // TODO: Initialize to an appropriate value
            target.ProcessClosePresentation();

            CommonTest.WaitForEventAsyncInvoke();

            listener.Verify(foo => foo.ReceiveMessage(ServiceCommands.CLOSE_PRESENTATION));
        }
    }
}
