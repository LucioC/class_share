using ClassService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Net;
using System.IO;
using System.ServiceModel;
using Autofac;
using PowerPointPresentation;
using Moq;

namespace TestProject1
{

    public class ServiceClient : ClientBase<IService>, IService
    {
        public String Add()
        {
            return base.Channel.Add();
        }

        public MyContract AddParameter(MyContract name)
        {
            return base.Channel.AddParameter(name);
        }

        public Result StartPresentation(string fileName)
        {
            throw new NotImplementedException();
        }
        
        public Result NextSlide()
        {
            throw new NotImplementedException();
        }
        
        public Result PreviousSlide()
        {
            throw new NotImplementedException();
        }


        public Result GoToSlideNumber(string number)
        {
            throw new NotImplementedException();
        }


        public Result ClosePresentation()
        {
            throw new NotImplementedException();
        }
    }

    
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

        public void get(Uri address)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output  
                Console.WriteLine(reader.ReadToEnd());
            }
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
        //[UrlToTest("http://localhost:2475/Service.svc/")]
        public void AddTest()
        {
            
            Service target = new Service(); // TODO: Initialize to an appropriate value
            string expected = "Lucio"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Add();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [UrlToTest("http://localhost:2475/Service.svc/")]
        public void testUnit()
        {
            Service target = new Service();
            string expected = "Lucio";
            string actual;
            actual = target.Add();
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for AddParameter
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\lucioc\\Desktop\\class_share\\ClassService\\WcfService", "/")]
       // [UrlToTest("http://localhost:2475/Service.svc/")]
        public void AddParameterTest()
        {
            Service target = new Service(); // TODO: Initialize to an appropriate value
            MyContract name = new MyContract(); // TODO: Initialize to an appropriate value "
            name.first = "Lucio";
            name.last = "L";
            MyContract expected = null; // TODO: Initialize to an appropriate value
            MyContract actual;
            actual = target.AddParameter(name);
            //Assert.AreEqual(expected, actual);

            String resultString;
            ServiceClient client = new ServiceClient();
            resultString = client.Add();
            Assert.AreEqual("Lucio", resultString);

        }

        [TestMethod()]
        public void verifyCorrectStartMessage()
        {
            //TODO finish example with mock and maybe di
            var mock = new Mock<PowerPointControl>();
            mock.Setup(foo => foo.PreparePresentation(It.IsAny<string>())).Returns(true);

            Service target = new Service();

            Result actual;
            actual = target.StartPresentation("correct");

            Assert.AreEqual("Presentation has been started.", actual.Message);
        }
    }
}
