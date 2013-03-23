using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerPointPresentation;

namespace ServiceTest
{
    [TestClass]
    public class PowerPointPresentationTest
    {
        private static String correctFileName;

        [TestInitialize()]
        public void TestInitialize()
        {
            string fileName = "test.pptx";
            correctFileName = CommonTest.GetFileResourcePath(fileName);
        }

        /// <summary>
        ///A test for preparePresentation
        ///</summary>
        [TestMethod()]
        public void prepareCorrectPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); 
            string fileName = correctFileName; 

            try
            {
                target.PreparePresentation(fileName);
                Assert.IsTrue(true);
            }
            catch(Exception e)
            {
                Assert.Fail("Exeption" + e.Message);
            }

        }

        /// <summary>
        ///A test for preparePresentation
        ///</summary>
        [TestMethod()]
        public void prepareNotExistentPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); 
            string fileName = "wrondfile.dd"; 
            try
            {
                target.PreparePresentation(fileName);
                Assert.Fail("No exception was throw with a wrong name");
            }
            catch (Exception e)
            {
                Assert.IsTrue(true, "Exeption was throw " + e.Message );
            }
        }

        /// <summary>
        ///A test for closePresentation
        ///</summary>
        [TestMethod()]
        public void closePresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value

            target.PreparePresentation(correctFileName);
            target.StartPresentation();

            try
            {
                target.ClosePresentation();
                Assert.IsTrue(true, "Presentation was closed with no exception");
            }
            catch(Exception e)
            {
                Assert.Fail("Exception : " + e.Message); 
            }

        }

        /// <summary>
        ///A test for closePresentation
        ///</summary>
        [TestMethod()]
        public void closeNotStartedPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value

            try
            {
                target.ClosePresentation();
                Assert.Fail("Non existent presentation was closed with no exception");
            }
            catch (Exception e)
            {
                Assert.IsTrue(true, "Exception was correctly throw for a bad non existent presentation");
            }
        }
    }
}
