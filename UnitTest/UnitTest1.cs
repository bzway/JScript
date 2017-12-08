using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JScript;
using System.Diagnostics;
using JScript.Lexers;
using JScript.Parsers;

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
            while (source.MoveNext())
            {
                var item = source.Current;
                Console.WriteLine(item);
            }
            Console.WriteLine("------------------");
            watch.Stop();

        }
        [TestMethod]
        public void TestMethod2()
        {
            Parser parser = new Parser(@"
var test='abc';
return test;");

            Console.WriteLine(parser.Parse().Value(new ScriptContext()).Value);
            Console.WriteLine("------------------");
        }
    }
}
