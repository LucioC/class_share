using ServiceCore.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ActionDependencyTest and is intended
    ///to contain all ActionDependencyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActionDependencyTest
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
        ///A test for ActionDependency Constructor
        ///</summary>
        [TestMethod()]
        public void ActionDependencyConstructorDefineTypeTest()
        {
            ActionType type = ActionType.LOCK_SPEECH; // TODO: Initialize to an appropriate value
            ActionDependency target = new ActionDependency(type);

            Assert.AreEqual(type, target.Type);
        }

        /// <summary>
        ///A test for ActionDependency Constructor
        ///</summary>
        [TestMethod()]
        public void ActionDependencyConstructorInitializeWithWaitingStateTest()
        {
            ActionType type = ActionType.LOCK_SPEECH; // TODO: Initialize to an appropriate value
            ActionDependency target = new ActionDependency(type);

            Assert.AreEqual(ActionState.WAITING, target.State);
        }
    }
}
