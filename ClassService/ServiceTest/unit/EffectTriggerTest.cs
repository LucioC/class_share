using ServiceCore.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ServiceCore.Engine.Triggers;
using ServiceCore.Utils;

namespace TestProject.Units
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

            Assert.AreEqual(true, target.IsReadyToTrigger(0));
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

            Assert.AreEqual(false, target.IsReadyToTrigger(0));
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
            Assert.AreEqual(false, target.IsReadyToTrigger(0));

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(true, target.IsReadyToTrigger(0));
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
            Assert.AreEqual(false, target.IsReadyToTrigger(0));

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(false, target.IsReadyToTrigger(0));
        }

        [TestMethod()]
        public void SetTwoEventTriggerWithTimeWindowAndIsAbleToTriggerOnCorrectTimeTest()
        {
            EffectTrigger target = new EffectTrigger();
            
            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            target.Triggers.Add(modalityEvent1);
            target.Triggers.Add(modalityEvent2);
            //Set the time windows constraint
            target.TimeWindow = 2000;

            ModalityEvent newModalityEvent1 = new ModalityEvent();
            newModalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            newModalityEvent1.EventTime = 1000;
            ModalityEvent newModalityEvent2 = new ModalityEvent();
            newModalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            newModalityEvent2.EventTime = 2000;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent1));
            Assert.AreEqual(true, target.HasEvent(newModalityEvent2));

            target.SetNewEvent(newModalityEvent1);
            Assert.AreEqual(false, target.IsReadyToTrigger(1001));

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(true, target.IsReadyToTrigger(2001));
        }

        [TestMethod()]
        public void SetTwoEventTriggerWithTimeWindowAndIsNotAbleToTriggerOnIncorrectTimeTest()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            target.Triggers.Add(modalityEvent1);
            target.Triggers.Add(modalityEvent2);
            //Set the time windows constraint
            target.TimeWindow = 2000;

            ModalityEvent newModalityEvent1 = new ModalityEvent();
            newModalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            newModalityEvent1.EventTime = 1000;
            ModalityEvent newModalityEvent2 = new ModalityEvent();
            newModalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            newModalityEvent2.EventTime = 3001;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent1));
            Assert.AreEqual(true, target.HasEvent(newModalityEvent2));

            target.SetNewEvent(newModalityEvent1);
            Assert.AreEqual(false, target.IsReadyToTrigger(1001));

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(false, target.IsReadyToTrigger(3002));
        }
        
        [TestMethod()]
        public void NewEventUpdateTimeAndStateTest()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;
            target.Triggers.Add(modalityEvent);

            ModalityEvent newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_LEFT;
            newModalityEvent.EventTime = 1000;
            target.SetNewEvent(newModalityEvent);

            Assert.AreEqual(target.Triggers[0].EventTime, 1000);

            newModalityEvent = new ModalityEvent();
            newModalityEvent.Type = ActionType.HAND_SWIPE_LEFT;
            newModalityEvent.EventTime = 2500;
            target.SetNewEvent(newModalityEvent);

            Assert.AreEqual(target.Triggers[0].EventTime, 2500);
        }


        [TestMethod()]
        public void SetThreeEventsTriggerWithTimeWindowAndIsAbleToTriggerIncorrectTimeTest()
        {
            EffectTrigger target = new EffectTrigger();

            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            ModalityEvent modalityEvent3 = new ModalityEvent();
            modalityEvent3.Type = ActionType.POSTURE_DIRECTION_KINECT;
            target.Triggers.Add(modalityEvent1);
            target.Triggers.Add(modalityEvent2);
            target.Triggers.Add(modalityEvent3);
            //Set the time windows constraint
            target.TimeWindow = 1500;

            ModalityEvent newModalityEvent1 = new ModalityEvent();
            newModalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;
            newModalityEvent1.EventTime = 1000;
            ModalityEvent newModalityEvent2 = new ModalityEvent();
            newModalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;
            newModalityEvent2.EventTime = 1500;
            ModalityEvent newModalityEvent3 = new ModalityEvent();
            newModalityEvent3.Type = ActionType.POSTURE_DIRECTION_KINECT;
            newModalityEvent3.EventTime = 3000;

            Assert.AreEqual(true, target.HasEvent(newModalityEvent1));
            Assert.AreEqual(true, target.HasEvent(newModalityEvent2));
            Assert.AreEqual(true, target.HasEvent(newModalityEvent3));

            target.SetNewEvent(newModalityEvent1);
            Assert.AreEqual(false, target.IsReadyToTrigger(1001));

            target.SetNewEvent(newModalityEvent2);
            Assert.AreEqual(false, target.IsReadyToTrigger(1550));

            target.SetNewEvent(newModalityEvent3);
            Assert.AreEqual(false, target.IsReadyToTrigger(3001));
        }
    }
}
