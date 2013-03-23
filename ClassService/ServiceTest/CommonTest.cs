using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceTest
{
    public class CommonTest
    {
        public static string GetFileResourcePath(string fileName)
        {
            String testLocalPath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\" + fileName;
            return testLocalPath;
        }
    }
}
