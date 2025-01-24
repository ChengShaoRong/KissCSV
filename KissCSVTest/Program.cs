/*
 *           KissCSV
 * Copyright © 2023 RongRong. All right reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpLike;

namespace KissCSVTest
{
    /// <summary>
    /// Define a class for the CSV table.
    /// </summary>
    public class TestCsv
    {
        public int id;
        public int number;
        public string name;
        public List<int> testInts;
        public List<string> testStrings;
        public List<float> testFloats;
        public Dictionary<string, int> testStringIntDicts;
        public Dictionary<int, bool> testIntBooleanDicts;
        [KissCSVDontLoad]
        public int testDontLoad;//This attribute won't be loaded whether exist column 'testDontLoad' in CSV file, due to mask as KissCSVDontLoad.
        public int[] testNotSupportType;//This attribute won't be loaded due to it is not support type
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("We recommend read data from CSV file with class!");
            //We recommend read data from CSV file with class(NOT struct!).
            //First step: define a class
            // e.g. we define class TestCsv
            //Second step: Load it into memory
            KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id");
            //final step: Get the data in class by unique key
            TestCsv csv = KissCSV.Get("TestCsv.csv", "1") as TestCsv;
            if (csv != null)//If not exist "1" in columnName "id" will return null.
            {
                Console.WriteLine($"id={csv.id}");//output id=1
                Console.WriteLine($"name={csv.name}");//output name=test name
                Console.WriteLine($"testStrings={List2String(csv.testStrings)}");//testStrings=aa,bb
                Console.WriteLine($"testStringIntDicts={Dictionary2String(csv.testStringIntDicts)}");//testStringIntDicts={ab,2},{cd,4}
            }

            Console.WriteLine("\nRead data from CSV file with no class. it't not recommend.");
            //We read data from CSV file with no class.
            //First step:
            // Load the CSV file into memory. You need call this just one time before get any value from it.
            // If you want to reload the CSV file you can call it again.
            // We will try load './Item.csv' if './CSV/Item.csv' not exist.
            SimpleKissCSV.Load("Item.csv", "id");
            //KissCSV.InitWithFileContent("Item.csv", "id", File.ReadAllText("./Item.csv"));//You can load the file by yourself if you need to.

            //Below are file content of 'Item.csv'
/*
id,name,maxStack,testSingle,testDouble,testBoolean,testStringList,testIntList,testStringStringDictionary,testIntIntDictionary
100,"test"" 
name",8888,0.5,888888888888,true,"Hello world|Hi,RongRong",2|3,aa_b|cc_d,1_2|3_4|5_6
101,name2,9999,,,,,,
*/


            //Final step; get kinds of values from the CSV file.
            Console.WriteLine($"keys={List2String(SimpleKissCSV.GetIntListKeys("Item.csv"))}");//output keys=100,101
            Console.WriteLine($"name={SimpleKissCSV.GetString("Item.csv", "100", "name")}");//output name=test" \nname
            Console.WriteLine($"maxStack={SimpleKissCSV.GetInt("Item.csv", "100", "maxStack")}");//output maxStack=8888
            Console.WriteLine($"testSingle={SimpleKissCSV.GetSingle("Item.csv", "100", "testSingle")}");//output testSingle=0.5
            Console.WriteLine($"testDouble={SimpleKissCSV.GetDouble("Item.csv", "100", "testDouble")}");//output testDouble=888888888888
            Console.WriteLine($"testBoolean={SimpleKissCSV.GetBoolean("Item.csv", "100", "testBoolean")}");//output testBoolean=True
            Console.WriteLine($"testStringList={List2String(SimpleKissCSV.GetStringList("Item.csv", "100", "testStringList"))}");//output testStringList=Hello world,Hi,RongRong
            Console.WriteLine($"testIntList={List2String(SimpleKissCSV.GetIntList("Item.csv", "100", "testIntList"))}");//output testIntList=2,3
            Console.WriteLine($"testStringStringDictionary={Dictionary2String(SimpleKissCSV.GetStringStringDictionary("Item.csv", "100", "testStringStringDictionary"))}");//output testStringStringDictionary={aa,b},{cc,d}
            Console.WriteLine($"testIntIntDictionary={Dictionary2String(SimpleKissCSV.GetIntIntDictionary("Item.csv", "100", "testIntIntDictionary"))}");//output testIntIntDictionary={1,2},{3,4},{5,6}

            Console.ReadKey();
        }

        static string List2String<T>(List<T> lists)
        {
            StringBuilder sb = new StringBuilder();
            if (lists.Count > 0)
            {
                foreach (T data in lists)
                    sb.Append(data+",");
                sb.Remove(sb.Length-1, 1);
            }
            return sb.ToString();
        }

        static string Dictionary2String<T,V>(Dictionary<T,V> lists)
        {
            StringBuilder sb = new StringBuilder();
            if (lists.Count > 0)
            {
                foreach (var data in lists)
                    sb.Append($"{{{data.Key},{data.Value}}},");
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}
