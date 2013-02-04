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
            correctFileName = @"C:\Users\lucioc\Desktop\class_share\Samples\WCF\WcfService1\TestProject1\resources\PEP_posM.pptx";
        }

        /// <summary>
        ///A test for preparePresentation
        ///</summary>
        [TestMethod()]
        public void prepareCorrectPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value
            string fileName = correctFileName; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.preparePresentation(fileName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for preparePresentation
        ///</summary>
        [TestMethod()]
        public void prepareNotExistentPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value
            string fileName = "wrondfile.dd"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.preparePresentation(fileName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for closePresentation
        ///</summary>
        [TestMethod()]
        public void closePresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value

            target.preparePresentation(correctFileName);
            target.startPresentation();
            
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.closePresentation();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for closePresentation
        ///</summary>
        [TestMethod()]
        public void closeNotStartedPresentationTest()
        {
            PowerPointControl target = new PowerPointControl(); // TODO: Initialize to an appropriate value
                        
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.closePresentation();
            Assert.AreEqual(expected, actual);
        }
    }
}
