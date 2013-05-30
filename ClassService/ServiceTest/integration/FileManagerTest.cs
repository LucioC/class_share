using CommonUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace TestProject.Integration
{    
    
    /// <summary>
    ///This is a test class for FileManagerTest and is intended
    ///to contain all FileManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileManagerTest
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


        private void createFile(String fileName, byte[] content)
        {
            using (System.IO.FileStream fs = System.IO.File.Create(fileName))
            {
                fs.Write(content, 0, content.Length);
                fs.Close();
            }         
        }

        /// <summary>
        ///A test for FileExists
        ///</summary>
        [TestMethod()]
        public void FileExistsTest()
        {
            FileManager target = new FileManager(); // TODO: Initialize to an appropriate value
            string fileName = "MyFile.txt"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            String content = "Content";
            bool actual;
            
            actual = target.FileExists(fileName);
            Assert.AreEqual(false, actual);

            createFile(fileName, Encoding.ASCII.GetBytes(content));
            actual = target.FileExists(fileName);
            Assert.AreEqual(expected, actual);
            System.IO.File.Delete(fileName);
        }

        [TestMethod()]
        public void CreateFileTest()
        {
            FileManager target = new FileManager(); // TODO: Initialize to an appropriate value

            String fileName = "MyFile.txt";
            String content = "Content of my file";

            target.CreateFile(fileName, content);

            Assert.IsTrue(target.FileExists(fileName));

            System.IO.File.Delete(fileName);
        }

        [TestMethod()]
        public void ReadFileTest()
        {
            FileManager target = new FileManager(); // TODO: Initialize to an appropriate value

            String fileName = "MyFile.txt";
            String content = "Content of my file";

            createFile(fileName, Encoding.ASCII.GetBytes(content));
            
            Assert.IsTrue(target.FileExists(fileName));
            Assert.AreEqual(content, target.ReadFile(fileName).ToString());
            System.IO.File.Delete(fileName);
        }

        [TestMethod()]
        public void DeleteFileTest()
        {
            FileManager target = new FileManager(); // TODO: Initialize to an appropriate value

            String fileName = "MyFile1.txt";
            String content = "Content of my file";

            createFile(fileName, Encoding.ASCII.GetBytes(content));

            target.DeleteFile(fileName);

            Assert.IsFalse(target.FileExists(fileName));
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch (Exception e)
            {
            }
        }


    }
}
