using WcfService1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for Service1Test and is intended
    ///to contain all Service1Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Service1Test
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
        ///A test for Add
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Lucio\\Desktop\\class_share\\Samples\\WCF\\WcfService1\\WcfService1", "/")]
        [UrlToTest("http://localhost:2475/Service1.svc/")]
        public void AddTest()
        {
            Service1 target = new Service1(); // TODO: Initialize to an appropriate value
            string expected = "Lucio"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Add();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddParameter
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Lucio\\Desktop\\class_share\\Samples\\WCF\\WcfService1\\WcfService1", "/")]
        [UrlToTest("http://localhost:2475/")]
        public void AddParameterTest()
        {
            Service1 target = new Service1(); // TODO: Initialize to an appropriate value
            MyContract name = null; // TODO: Initialize to an appropriate value
            MyContract expected = null; // TODO: Initialize to an appropriate value
            MyContract actual;
            actual = target.AddParameter(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
