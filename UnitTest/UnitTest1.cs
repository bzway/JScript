using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JScript;
using System.Diagnostics;
using JScript.Lexers;
using JScript.Parsers;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace UnitTest
{

    public class MyClass
    {
        public string Test { get; set; }
        public int C { get; set; }
    }



    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodRef()
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                var class1 = new MyClass() { Test = "original string" + i.ToString(), C = 100, };
                var class2 = class1;


                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Console.WriteLine(class2.Test);
                    Console.WriteLine(class2.C);
                }));
                class1.Test = "has been changed";
                class1.C = i;

            }
            Task.WaitAll(tasks.ToArray());
            Thread.Sleep(5000);
        }

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

            Console.WriteLine(parser.Parse().Execute(new ScriptContext()).Value);
            Console.WriteLine("------------------");
        }
    }
}
