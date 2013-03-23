using ClassService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.IO;
using ImageZoom;
using KinectPowerPointControl;

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

            var imageControlMock = new Moq.Mock<ImageFormControl>();
            var kinectControlMock = new Moq.Mock<KinectWindowControl>();

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            
            target.CloseCurrentImage();

            imageControlMock.Verify(foo => foo.StopThread());
            kinectControlMock.Verify(foo => foo.StopThread());
        }

        /// <summary>
        ///A test for ClosePresentation
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void ClosePresentationTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.ClosePresentation();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFile
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void GetFileTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Stream expected = null; // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.GetFile(fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GoToSlideNumber
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void GoToSlideNumberTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            string number = string.Empty; // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.GoToSlideNumber(number);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for NextSlide
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void NextSlideTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.NextSlide();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OpenImage
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void OpenImageTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.OpenImage(fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PresentationCommand
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void PresentationCommandTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            ClassService.Action action = null; // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.PresentationCommand(action);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PreviousSlide
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void PreviousSlideTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.PreviousSlide();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for StartPresentation
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void StartPresentationTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            ClassService.File file = null; // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.StartPresentation(file);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadFile
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void UploadFileTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Stream fileContents = null; // TODO: Initialize to an appropriate value
            Result expected = null; // TODO: Initialize to an appropriate value
            Result actual;
            actual = target.UploadFile(fileName, fileContents);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
