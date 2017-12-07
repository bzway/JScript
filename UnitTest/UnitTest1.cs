using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JScript;
using System.Diagnostics;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            SourceReader source = new SourceReader(@"
var test='test';
test +=10.1;
alert(test);");
            foreach (var item in source)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("------------------");
            watch.Stop();

        }
        [TestMethod]
        public void TestMethod2()
        {
            Parser parser = new Parser(@"
var test='test';
return test;");

            Console.WriteLine(parser.Parse().Value());
            Console.WriteLine("------------------");
        }
    }
}
