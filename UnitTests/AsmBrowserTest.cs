using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class AsmBrowserTest
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    public class Example
    {
        public string Length { get; set; }
    }

    static class ExtensionMethods1
    {
        public static void extMethod1(this string obj)
        {
            Console.WriteLine(obj.Length);
        }
    }

    static class ExtensionMethods2
    {
        public static void extMethod2(this Example obj)
        {
            Console.WriteLine(obj.Length);
        }
    }

    /*static class ExtensionMethods3
    {
        public static void extMethod3(this ExtensionExampleClass obj)
        {
            Console.WriteLine(obj.StringObject);
        }
    }*/
}
