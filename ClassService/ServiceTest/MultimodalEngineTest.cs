using ServiceCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ServiceCore.Engine;
using ServiceCore.Engine.Triggers;
using ServiceCore.Utils;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for MultimodalEngineTest and is intended
    ///to contain all MultimodalEngineTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultimodalEngineTest
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
        public void AddNewEvent()
        {
            MultimodalEngine target = new MultimodalEngine();
            var action = new Mock<ModalityEvent>();

            target.NewInputModalityEvent(action.Object);

            Assert.IsTrue(target.Actions.Contains(action.Object));            
        }

        [TestMethod()]
        public void AddOneEffectTrigger()
        {
            MultimodalEngine target = new MultimodalEngine();

            var modalityEvent = new Mock<ModalityEvent>();
            var effect = new Mock<IEffect>();

            EffectTrigger effectTrigger = new EffectTrigger();

            target.addNewTrigger(effectTrigger);

            Assert.IsTrue(target.Triggers.Contains(effectTrigger));
        }

        [TestMethod()]
        public void AddOneEffectTriggerAndTriggerIt()
        {
            MultimodalEngine target = new MultimodalEngine();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            var effect = new Mock<IEffect>();

            //Define trigger with one input modality event that triggers one effect
            EffectTrigger effectTrigger = new EffectTrigger();
            effectTrigger.Effects.Add(effect.Object);
            effectTrigger.Triggers.Add(modalityEvent);

            target.addNewTrigger(effectTrigger);

            //Prepare future event
            ModalityEvent action = new ModalityEvent();
            action.Type = ActionType.HAND_SWIPE_LEFT;
            
            //trigger it
            target.NewInputModalityEvent(action);

            //Verify triggering
            effect.Verify(foo => foo.execute());
        }

        [TestMethod()]
        public void AddOneEffectTriggerAndDontTriggerIt()
        {
            MultimodalEngine target = new MultimodalEngine();

            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            var effect = new Mock<IEffect>();

            //Define trigger with one input modality event that triggers one effect
            EffectTrigger effectTrigger = new EffectTrigger();
            effectTrigger.Effects.Add(effect.Object);
            effectTrigger.Triggers.Add(modalityEvent);

            target.addNewTrigger(effectTrigger);

            //Prepare future event
            ModalityEvent action = new ModalityEvent();
            action.Type = ActionType.HAND_SWIPE_RIGHT;

            //add event
            target.NewInputModalityEvent(action);

            //Verify that wasnt triggered
            effect.Verify(foo => foo.execute(), Times.Never());
        }

        [TestMethod()]
        public void AddComposedTriggerEffectAndTriggerIt()
        {
            MultimodalEngine target = new MultimodalEngine();

            ModalityEvent modalityEvent1 = new ModalityEvent();
            modalityEvent1.Type = ActionType.HAND_SWIPE_LEFT;

            ModalityEvent modalityEvent2 = new ModalityEvent();
            modalityEvent2.Type = ActionType.HAND_SWIPE_RIGHT;

            var effect = new Mock<IEffect>();

            //Define trigger with one input modality event that triggers one effect
            EffectTrigger effectTrigger = new EffectTrigger();
            effectTrigger.Effects.Add(effect.Object);
            effectTrigger.Triggers.Add(modalityEvent1);
            effectTrigger.Triggers.Add(modalityEvent2);

            target.addNewTrigger(effectTrigger);

            //Prepare future event1
            ModalityEvent action1 = new ModalityEvent();
            action1.Type = ActionType.HAND_SWIPE_LEFT;

            target.NewInputModalityEvent(action1);

            //Was not trigger yet
            effect.Verify(foo => foo.execute(), Times.Never());

            //Prepare future event2
            ModalityEvent action2 = new ModalityEvent();
            action2.Type = ActionType.HAND_SWIPE_RIGHT;

            target.NewInputModalityEvent(action2);

            effect.Verify(foo => foo.execute());


        }

        [TestMethod()]
        public void AddOneTriggerAndTriggersOnCorrectTimePassed()
        {
            //Create a mock for time, that returns 0,5 seconds more for each call
            var mock = new Mock<Time>();
            var timeInMilliseconds = 0;
            mock.Setup(foo => foo.CurrentTimeInMillis())
                .Returns(() => timeInMilliseconds)
                .Callback(() => timeInMilliseconds += 500);
            
            MultimodalEngine target = new MultimodalEngine();
            
            ModalityEvent modalityEvent = new ModalityEvent();
            modalityEvent.Type = ActionType.HAND_SWIPE_LEFT;

            var effect = new Mock<IEffect>();

            //Define trigger with one input modality event that triggers one effect
            EffectTrigger effectTrigger = new EffectTrigger();
            effectTrigger.Effects.Add(effect.Object);
            effectTrigger.Triggers.Add(modalityEvent);

            target.addNewTrigger(effectTrigger);

            //Prepare future event
            ModalityEvent action = new ModalityEvent();
            action.Type = ActionType.HAND_SWIPE_RIGHT;

            //add event
            target.NewInputModalityEvent(action);
        }
    }
}
