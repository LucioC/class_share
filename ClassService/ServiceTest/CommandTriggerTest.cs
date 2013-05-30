using ServiceCore.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ServiceCore;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for CommandTriggerTest and is intended
    ///to contain all CommandTriggerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommandTriggerTest
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

        [TestMethod()]
        public void CommandTriggerReady()
        {
            EffectTrigger target = new EffectTrigger();
            var action = new Mock<ModalityEvent>();

            //ActionDependency dependency = new ActionDependency(action.Object.Type);

            //target.AddActionDependency(action.Object);
        }
    }
}
