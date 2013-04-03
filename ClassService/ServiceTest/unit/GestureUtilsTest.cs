using KinectPowerPointControl.Gesture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for GestureUtilsTest and is intended
    ///to contain all GestureUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GestureUtilsTest
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
        ///A test for normalizeDistance
        ///</summary>
        [TestMethod()]
        public void normalizePositiveDistanceTest()
        {
            float deltaDistance = 2.0F;
            float errorExpected = 1.0F;
            float expected = 1.0F;
            float actual;
            actual = GestureUtils.normalizeDistance(deltaDistance, errorExpected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void normalizeNegativeDistanceTest()
        {
            float deltaDistance = -2.0F;
            float errorExpected = 1.0F;
            float expected = -1.0F;
            float actual;
            actual = GestureUtils.normalizeDistance(deltaDistance, errorExpected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void normalizeToZeroWhenErrorIsGreaterThanPositiveDistanceTest()
        {
            float deltaDistance = 2.0F;
            float errorExpected = 3.0F;
            float expected = 0.0F;
            float actual;
            actual = GestureUtils.normalizeDistance(deltaDistance, errorExpected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void normalizeToZeroWhenErrorIsGreaterThanNegativeDistanceTest()
        {
            float deltaDistance = -2.0F;
            float errorExpected = 3.0F;
            float expected = 0.0F;
            float actual;
            actual = GestureUtils.normalizeDistance(deltaDistance, errorExpected);
            Assert.AreEqual(expected, actual);
        }
    }
}
