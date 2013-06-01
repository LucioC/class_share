using ServiceCore.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ServiceCore.Engine.Triggers;
using ServiceCore.Utils;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for EffectTriggerTest and is intended
    ///to contain all EffectTriggerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EffectTriggerTest
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
        ///A test for HasEvent
        ///</summary>
        [TestMethod()]
        public void HasEventTest()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            target.Triggers.Add(modalityEvent);
            
            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent));
        }

        [TestMethod()]
        public void NotHaveEventTest()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            target.Triggers.Add(modalityEvent);
            
            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_RIGHT;

            Assert.AreEqual(false, target.HasEvent(newModalityEvent));
        }

        [TestMethod()]
        public void SetOneEventTriggerAndIsAbleToTrigger()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            target.Triggers.Add(modalityEvent);

            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent));
            target.SetNewEvent(newModalityEvent);

            Assert.AreEqual(true, target.IsReadyToTrigger());
        }

        [TestMethod()]
        public void SetOneEventTriggerAndIsNotAbleToTrigger()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            target.Triggers.Add(modalityEvent);

            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_RIGHT;

            Assert.AreEqual(false, target.HasEvent(newModalityEvent));
            target.SetNewEvent(newModalityEvent);

            Assert.AreEqual(false, target.IsReadyToTrigger());
        }

        [TestMethod()]
        public void SetTwoEventsTriggerAndIsAbleToTrigger()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            target.Triggers.Add(modalityEvent1);
            target.Triggers.Add(modalityEvent2);

            ModalityEvent newModalityEvent1 = new ModalityEvent();
            newModalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent newModalityEvent2 = new ModalityEvent();
            newModalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent1));
            Assert.AreEqual(true, target.HasEvent(newModalityEvent2));

            target.SetNewEvent(newModalityEvent1);
            Assert.AreEqual(false, target.IsReadyToTrigger());

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(true, target.IsReadyToTrigger());
        }

        [TestMethod()]
        public void SetTwoEventsTriggerAndIsNotAbleToTrigger()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            target.Triggers.Add(modalityEvent1);
            target.Triggers.Add(modalityEvent2);

            ModalityEvent newModalityEvent1 = new ModalityEvent();
            newModalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent newModalityEvent2 = new ModalityEvent();
            newModalityEvent2.Type = ActionType.SPEECH_NEXT;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent1));
            Assert.AreEqual(false, target.HasEvent(newModalityEvent2));

            target.SetNewEvent(newModalityEvent1);
            Assert.AreEqual(false, target.IsReadyToTrigger());

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(false, target.IsReadyToTrigger());
        }

        [TestMethod()]
        public void SetOneEventTriggerAndIsAbleToTriggerOnCorrectTimeWindow()
        {
            //Create a mock for time, that returns 0,5 seconds more for each call
            var mock = new Mock<Time>();
            var timeInMilliseconds = 0;
            mock.Setup(foo => foo.CurrentTimeInMillis())
                .Returns(() => timeInMilliseconds );

            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;
            target.Triggers.Add(modalityEvent);
            //Set the time windows constraint
            target.TimeWindow = 2000;

            target.Clock = mock.Object;

            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent));
            target.SetNewEvent(newModalityEvent);

            Assert.AreEqual(true, target.IsReadyToTrigger());
        }
    }
}
