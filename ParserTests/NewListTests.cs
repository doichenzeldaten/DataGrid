﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser.Tests
{
    [TestClass()]
    public class NewListTests
    {
        [TestMethod()]
        public void IndexatorTest()
        {
            MyList<int> a = new MyList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            a.Add(15);                       
           Assert.AreEqual(15, a[4]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => a[10]);
            a[1] = 123;
            Assert.AreEqual(123, a[1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => a[10]=123);
        }
        [TestMethod()]
        public void RemoveTest()
        {
            MyList<int> test = new MyList<int>();
            test.Add(1);
            test.Add(2);
            test.Add(3);
            test.Add(4);
            test.Add(5);
            test.Remove(3);
            test.Add(56);
            Assert.AreEqual(4, test[2]);
            Assert.AreEqual(56, test[4]);
        }

        [TestMethod()]
        public void AddTest()
        {
            MyList<int> a = new MyList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            Assert.AreEqual(12, a[3]);
        }
         
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            MyList<JSONObject> a = new MyList<JSONObject>();
            JSONObject Obj1 = new JSONObject();
            Obj1.Fields.Add("FirstName", "FistName1");
            Obj1.Fields.Add("LastName", "LastName1");
            JSONObject Obj2 = new JSONObject();
            Obj2.Fields.Add("FirstName", "FistName2");
            Obj2.Fields.Add("LastName", "LastName2");
            a.Add(Obj1);
            a.Add(Obj2);
            var index = 0;
            string TestResult="";
            foreach (var item in a)
            {                            
                if (index == 1)
                {
                   TestResult = item.Fields["LastName"];
                }
                index++;
            }
            Assert.AreEqual("LastName2",TestResult);
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            MyList<int> a = new MyList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            Assert.AreEqual(2, a.IndexOf(21));
        }

        [TestMethod()]
        public void InsertTest()
        {
            MyList<int> a = new MyList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            a.Insert(2, 123);
            Assert.AreEqual(123, a[2]);
        }
        [TestMethod()]
        public void AddToListWithPresetCapacityTest()
        {
            MyList<int> a = new MyList<int>(10);
            a.Add(5);
            a.Add(6);
            a.Add(7);
            a.Add(8);
            a.Add(9);
            a.Add(10);
            a.Add(11);
            a.Add(12);
            a.Add(13);
            a.Add(14);
            a.Add(15);
            a.Add(16);
            a.Insert(2, 123);
            Assert.AreEqual(8, a[4]);
        }

       

        
       
    }
}