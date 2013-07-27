using ServiceCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Moq;
using ServiceTest;

namespace TestProject.Units
{
    
    
    /// <summary>
    ///This is a test class for AbstractCommunicatorTest and is intended
    ///to contain all AbstractCommunicatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DefaultCommunicatorTest
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
        ///A test for ReceiveMessage
        ///</summary>
        [TestMethod()]
        public void ReceiveMessageTest()
        {
            DefaultCommunicator speaker = new DefaultCommunicator();
            var listener = new Mock<DefaultCommunicator>();
            
            speaker.RegisterListener(listener.Object);

            string message = "a message";
            speaker.SendMessage(message);

            CommonTest.WaitForEventAsyncInvoke();

            listener.Verify(foo => foo.ReceiveMessage(message));
        }
    }
}
