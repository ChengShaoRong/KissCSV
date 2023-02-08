/*
 *           KissCSV
 * Copyright © 2023 RongRong. All right reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KissFramework;

namespace KissCSVTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //First step:
            // Load the CSV file into memory. You need call this just one time before get any value from it.
            // If you want to reload the CSV file you can call it again.
            // We will try load './Item.csv' if './CSV/Item.csv' not exist.
            KissCSV.Init("Item.csv", "id");
            //KissCSV.InitWithFileContent("Item.csv", "id", File.ReadAllText("./Item.csv"));//You can load the file by yourself if you need to.

            //Final step; get kinds of values from the CSV file.
            Console.WriteLine($"keys={List2String(KissCSV.GetIntListKeys("Item.csv"))}");//output keys=100,101
            Console.WriteLine($"name={KissCSV.GetString("Item.csv", "100", "name")}");//output name=test" \nname
            Console.WriteLine($"maxStack={KissCSV.GetInt("Item.csv", "100", "maxStack")}");//output maxStack=8888
            Console.WriteLine($"testSingle={KissCSV.GetSingle("Item.csv", "100", "testSingle")}");//output testSingle=0.5
            Console.WriteLine($"testDouble={KissCSV.GetDouble("Item.csv", "100", "testDouble")}");//output testDouble=888888888888
            Console.WriteLine($"testBoolean={KissCSV.GetBoolean("Item.csv", "100", "testBoolean")}");//output testBoolean=True
            Console.WriteLine($"testStringList={List2String(KissCSV.GetStringList("Item.csv", "100", "testStringList"))}");//output testStringList=Hello world,Hi,RongRong
            Console.WriteLine($"testIntList={List2String(KissCSV.GetIntList("Item.csv", "100", "testIntList"))}");//output testIntList=2,3
            Console.WriteLine($"testStringStringDictionary={Dictionary2String(KissCSV.GetStringStringDictionary("Item.csv", "100", "testStringStringDictionary"))}");//output testStringStringDictionary={aa,b},{cc,d}
            Console.WriteLine($"testIntIntDictionary={Dictionary2String(KissCSV.GetIntIntDictionary("Item.csv", "100", "testIntIntDictionary"))}");//output testIntIntDictionary={1,2},{3,4},{5,6}

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
