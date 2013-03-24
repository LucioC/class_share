using ClassService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.IO;
using ImageZoom;
using KinectPowerPointControl;
using Moq;
using ServiceCore;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ServiceTest and is intended
    ///to contain all ServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceTest
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

        /// <summary>
        ///A test for CloseCurrentImage
        ///</summary>
        [TestMethod()]
        public void CloseCurrentImageWithoutInitializeTest()
        {
            Service target = new Service();

            var imageControlMock = new Mock<IWindowThreadControl>();
            var kinectControlMock = new Mock<IWindowThreadControl>();

            imageControlMock.Setup(foo => foo.StopThread());
            kinectControlMock.Setup(foo => foo.StopThread());

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            
            target.CloseCurrentImage();

            imageControlMock.Verify(x => x.StopThread(), Times.Exactly(1));
            kinectControlMock.Verify(x => x.StopThread(), Times.Exactly(1));
        }

       
    }
}
