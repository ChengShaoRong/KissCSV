/*
 *           KissCSV
 * Copyright © 2023 RongRong. All right reserved.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CSharpLike
{
    /// <summary>
    /// A most simple and stupid way get data from CSV(Comma-Separated Values) file with 'RFC 4180'.
    /// Direct read data from CSV file with no class.
    /// We recommend that using KissCSV instead.
    /// </summary>
    public sealed class SimpleKissCSV
    {
        /// <summary>
        /// Initialize CSV file with one key.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// KissCSV.Init("Item.csv", "id");//Use "id" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\"</param>
        /// <param name="columnName">The column name of unique id in this CSV file</param>
        public static void Load(string fileName, string columnName)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, null);
        }
        /// <summary>
        /// Initialize CSV file with two keys, 'columnName_columnName2' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type
        /// 100,"shop name",1
        /// 100,shop2,2
        /// "
        /// Useage:
        /// KissCSV.Init("Shop.csv", "id", "type");//Use both "id" and "type" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\"</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2' will as the unique id in this CSV file</param>
        public static void Load(string fileName, string columnName, string columnName2)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, null);
        }
        /// <summary>
        /// Initialize CSV file with three keys, 'columnName_columnName2_columnName3' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type,subType
        /// 100,"shop name",1,3
        /// 100,shop2,2,2
        /// "
        /// Useage:
        /// KissCSV.Init("Shop.csv", "id", "type", "subType");//Use "id" and "type" and "subType" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\"</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        /// <param name="columnName3">The third column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        public static void Load(string fileName, string columnName, string columnName2, string columnName3)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, columnName3, null);
        }
        /// <summary>
        /// Initialize CSV file with four keys, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type,subType,exType
        /// 100,"shop name",1,3,0
        /// 100,shop2,2,2,0
        /// "
        /// Useage:
        /// KissCSV.Init("Shop.csv", "id", "type", "subType", "exType");//Use "id" and "type" and "subType" and "exType" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\"</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName3">The third column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName4">The fourth column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        public static void Load(string fileName, string columnName, string columnName2, string columnName3, string columnName4)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, columnName3, columnName4, null);
        }
        /// <summary>
        /// Initialize CSV file with one key.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// KissCSV.InitWithFileContent("Item.csv", "id", File.ReadAllText(".\\CSV\\Item.csv"));//Use "id" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\" if fileContent is empty</param>
        /// <param name="columnName">The column name of unique id in this CSV file</param>
        /// <param name="fileContent">The string of the file content, you can load the file by yourself</param>
        public static void LoadWithFileContent(string fileName, string columnName, string fileContent)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, fileContent);
        }
        /// <summary>
        /// Initialize CSV file with two keys, 'columnName_columnName2' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type
        /// 100,"shop name",1
        /// 100,shop2,2
        /// "
        /// Useage:
        /// KissCSV.InitWithFileContent("Shop.csv", "id", "type", File.ReadAllText(".\\CSV\\Shop.csv"));//Use both "id" and "type" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\" if fileContent is empty</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2' will as the unique id in this CSV file</param>
        public static void LoadWithFileContent(string fileName, string columnName, string columnName2, string fileContent)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, fileContent);
        }
        /// <summary>
        /// Initialize CSV file with three keys, 'columnName_columnName2_columnName3' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type,subType
        /// 100,"shop name",1,3
        /// 100,shop2,2,2
        /// "
        /// Useage:
        /// KissCSV.InitWithFileContent("Shop.csv", "id", "type", "subType", File.ReadAllText(".\\CSV\\Shop.csv"));//Use "id" and "type" and "subType" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\" if fileContent is empty</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        /// <param name="columnName3">The third column name in this CSV file, 'columnName_columnName2_columnName3' will as the unique id in this CSV file</param>
        public static void LoadWithFileContent(string fileName, string columnName, string columnName2, string columnName3, string fileContent)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, columnName3, fileContent);
        }
        /// <summary>
        /// Initialize CSV file with four keys, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file.
        /// E.G. "Shop.csv" file content is below:
        /// "
        /// id,name,type,subType,exType
        /// 100,"shop name",1,3,0
        /// 100,shop2,2,2,0
        /// "
        /// Useage:
        /// KissCSV.InitWithFileContent("Shop.csv", "id", "type", "subType", "exType", File.ReadAllText(".\\CSV\\Shop.csv"));//Use "id" and "type" and "subType" and "exType" as the unique column.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from ".\\CSV\\" or ".\\" if fileContent is empty</param>
        /// <param name="columnName">The first column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName2">The second column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName3">The third column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="columnName4">The fourth column name in this CSV file, 'columnName_columnName2_columnName3_columnName4' will as the unique id in this CSV file</param>
        /// <param name="fileContent">The string of the file content, you can load the file by yourself</param>
        public static void LoadWithFileContent(string fileName, string columnName, string columnName2, string columnName3, string columnName4, string fileContent)
        {
            mDatas[fileName] = new KissCSVImpl(fileName, columnName, columnName2, columnName3, columnName4, fileContent);
        }
        /// <summary>
        /// Get the all keys of the whole CSV file
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// List<int> keys = KissCSV.GetIntListKeys("Item.csv"");//keys content is {100,101}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        public static List<int> GetIntListKeys(string fileName)
        {
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                return csv.GetIntListKeys();
            return new List<int>();
        }
        /// <summary>
        /// Get the all keys of the whole CSV file
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// List<string> keys = KissCSV.GetStringListKeys("Item.csv"");//keys content is {"100","101"}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        public static List<string> GetStringListKeys(string fileName)
        {
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                return csv.GetStringListKeys();
            return new List<string>();
        }
        /// <summary>
        /// Get the string value from CSV file.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// string name = KissCSV.GetString("Item.csv", "101", "name");//name equal to "item2"
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        /// <param name="strDefault">default value if not exist in CSV file</param>
        public static string GetString(string fileName, string strUniqueKey, string strColumnName, string strDefault = "")
        {
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                return csv.GetDataString(strUniqueKey, strColumnName, strDefault);
            return strDefault;
        }
        /// <summary>
        /// Get the float value from CSV file.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// float stack = KissCSV.GetSingle("Item.csv", "101", "maxStack");//stack equal to 8888
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        /// <param name="fDefault">default value if not exist in CSV file</param>
        public static float GetSingle(string fileName, string strUniqueKey, string strColumnName, float fDefault = 0.0f)
        {
            return Convert.ToSingle(GetString(fileName, strUniqueKey, strColumnName, fDefault.ToString()), CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Get the int value from CSV file.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,enable
        /// 100,"item name",9999,false
        /// 101,item2,8888,true
        /// "
        /// Useage:
        /// int stack = KissCSV.GetInt("Item.csv", "101", "maxStack");//stack equal to 8888
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        /// <param name="iDefault">default value if not exist in CSV file</param>
        public static int GetInt(string fileName, string strUniqueKey, string strColumnName, int iDefault = 0)
        {
            return Convert.ToInt32(GetString(fileName, strUniqueKey, strColumnName, iDefault.ToString()));
        }
        /// <summary>
        /// Get the bool value from CSV file. Value '0/false/False' mean false, value '1/true/True' mean true.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,enable
        /// 100,"item name",9999,false
        /// 101,item2,8888,true
        /// "
        /// Useage:
        /// bool enable = KissCSV.GetBoolean("Item.csv", "101", "enable");//enable equal to true
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        /// <param name="bDefault">default value if not exist in CSV file</param>
        public static bool GetBoolean(string fileName, string strUniqueKey, string strColumnName, bool bDefault = false)
        {
            string str = GetString(fileName, strUniqueKey, strColumnName, bDefault.ToString());
            if (str.Length == 0)
                return bDefault;
            else if (str.Length == 1)
                return str != "0";
            return Convert.ToBoolean(str);
        }
        /// <summary>
        /// Get the double value from CSV file.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack
        /// 100,"item name",9999
        /// 101,item2,8888
        /// "
        /// Useage:
        /// double stack = KissCSV.GetDouble("Item.csv", "101", "maxStack");//stack equal to 8888
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        /// <param name="dDefault">default value if not exist in CSV file</param>
        public static double GetDouble(string fileName, string strUniqueKey, string strColumnName, double dDefault = 0)
        {
            return Convert.ToDouble(GetString(fileName, strUniqueKey, strColumnName, dDefault.ToString()), CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Get the StringList value from CSV file, and split by '|', but the string can't contain '|' itself.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testList
        /// 100,"item name",9999,ab|3
        /// 101,item2,8888,0|1
        /// "
        /// Useage:
        /// KissCSV.GetStringList("Item.csv", "100", "testList");//return values {"ab","3"}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static List<string> GetStringList(string fileName, string strUniqueKey, string strColumnName)
        {
            List<string> ret = new List<string>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
                ret.Add(s);
            return ret;
        }
        /// <summary>
        /// Get the SingleList value from CSV file, and split by '|'.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testList
        /// 100,"item name",9999,0.2|3.3
        /// 101,item2,8888,0|1
        /// "
        /// Useage:
        /// KissCSV.GetSingleList("Item.csv", "100", "testList");//return values {0.2,3.3}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static List<float> GetSingleList(string fileName, string strUniqueKey, string strColumnName)
        {
            List<float> ret = new List<float>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
                ret.Add(Convert.ToSingle(s, CultureInfo.InvariantCulture));
            return ret;
        }
        /// <summary>
        /// Get the IntList value from CSV file, and split by '|'.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testList
        /// 100,"item name",9999,2|3
        /// 101,item2,8888,0|1
        /// "
        /// Useage:
        /// KissCSV.GetIntList("Item.csv", "100", "testList");//return values {2,3}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static List<int> GetIntList(string fileName, string strUniqueKey, string strColumnName)
        {
            List<int> ret = new List<int>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
                ret.Add(Convert.ToInt32(s));
            return ret;
        }
        /// <summary>
        /// Get the BooleanList value from CSV file, and split by '|'. Value '0/false/False' mean false, value '1/true/True' mean true.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testList
        /// 100,"item name",9999,true|false
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetBooleanList("Item.csv", "100", "testList");//return values {true,false}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static List<bool> GetBooleanList(string fileName, string strUniqueKey, string strColumnName)
        {
            List<bool> ret = new List<bool>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                if (s.Length == 0)
                    ret.Add(false);
                else if (s.Length == 1)
                    ret.Add(s != "0");
                else
                    ret.Add(Convert.ToBoolean(s));
            }
            return ret;
        }
        /// <summary>
        /// Get the StringStringDictionary value from CSV file, and split by '|' and '_', but the string can't contain '|' and '_' itself.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,aa_b|bb_c
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetStringStringDictionary("Item.csv", "100", "testDictionary");//return values {{"aa","b"},{"bb","c"}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<string, string> GetStringStringDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[strss[0]] = strss[1];
            }
            return ret;
        }
        /// <summary>
        /// Get the StringStringDictionary value from CSV file, and split by '|' and '_', but the string can't contain '|' and '_' itself.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,aa_2|bb_3
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetStringIntDictionary("Item.csv", "100", "testDictionary");//return values {{"aa",2},{"bb",3}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<string, int> GetStringIntDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[strss[0]] = Convert.ToInt32(strss[1]);
            }
            return ret;
        }
        /// <summary>
        /// Get the StringSingleDictionary value from CSV file, and split by '|' and '_', but the string can't contain '|' and '_' itself.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,aa_2.1|bb_3
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetStringSingleDictionary("Item.csv", "100", "testDictionary");//return values {{"aa",2.1},{"bb",3}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<string, float> GetStringSingleDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<string, float> ret = new Dictionary<string, float>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[strss[0]] = Convert.ToSingle(strss[1], CultureInfo.InvariantCulture);
            }
            return ret;
        }
        /// <summary>
        /// Get the StringBooleanDictionary value from CSV file, and split by '|' and '_', but the string can't contain '|' and '_' itself. Value '0/false/False' mean false, value '1/true/True' mean true.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,aa_true|bb_false
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetStringBooleanDictionary("Item.csv", "100", "testDictionary");//return values {{"aa",true},{"bb",false}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<string, bool> GetStringBooleanDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<string, bool> ret = new Dictionary<string, bool>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                {
                    string ss = strss[1];
                    if (ss.Length == 0)
                        ret[strss[0]] = false;
                    else if (ss.Length == 1)
                        ret[strss[0]] = ss != "0";
                    else
                        ret[strss[0]] = Convert.ToBoolean(strss[1]);
                }
            }
            return ret;
        }
        /// <summary>
        /// Get the IntIntDictionary value from CSV file, and split by '|' and '_'.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,1_2|3_4
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetIntIntDictionary("Item.csv", "100", "testDictionary");//return values {{1,2},{3,4}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<int, int> GetIntIntDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[Convert.ToInt32(strss[0])] = Convert.ToInt32(strss[1]);
            }
            return ret;
        }
        /// <summary>
        /// Get the IntSingleDictionary value from CSV file, and split by '|' and '_'.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,1_2.1|3_4
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetIntSingleDictionary("Item.csv", "100", "testDictionary");//return values {{1,2.1},{3,4}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<int, float> GetIntSingleDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<int, float> ret = new Dictionary<int, float>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[Convert.ToInt32(strss[0])] = Convert.ToSingle(strss[1], CultureInfo.InvariantCulture);
            }
            return ret;
        }
        /// <summary>
        /// Get the IntBooleanDictionary value from CSV file, and split by '|' and '_'. Value '0/false/False' mean false, value '1/true/True' mean true.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,1_true|3_false
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetIntBooleanDictionary("Item.csv", "100", "testDictionary");//return values {{1,true},{3,false}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<int, bool> GetIntBooleanDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<int, bool> ret = new Dictionary<int, bool>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                {
                    string ss = strss[1];
                    if (ss.Length == 0)
                        ret[Convert.ToInt32(strss[0])] = false;
                    else if (ss.Length == 1)
                        ret[Convert.ToInt32(strss[0])] = ss != "0";
                    else
                        ret[Convert.ToInt32(strss[0])] = Convert.ToBoolean(ss);
                }
            }
            return ret;
        }
        /// <summary>
        /// Get the IntStringDictionary value from CSV file, and split by '|' and '_', but the string can't contain '|' and '_' itself.
        /// E.G. "Item.csv" file content is below:
        /// "
        /// id,name,maxStack,testDictionary
        /// 100,"item name",9999,1_aa|3_cc
        /// 101,item2,8888,,
        /// "
        /// Useage:
        /// KissCSV.GetIntStringDictionary("Item.csv", "100", "testDictionary");//return values {{1,"aa"},{3,"cc"}}
        /// </summary>
        /// <param name="fileName">File name of the CSV file</param>
        /// <param name="strUniqueKey">The unique key that that your want to get value</param>
        /// <param name="strColumnName">The column name that your want to get value</param>
        public static Dictionary<int, string> GetIntStringDictionary(string fileName, string strUniqueKey, string strColumnName)
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            string str;
            if (mDatas.TryGetValue(fileName, out KissCSVImpl csv))
                str = csv.GetDataString(strUniqueKey, strColumnName, "");
            else
                return ret;
            if (string.IsNullOrEmpty(str))
                return ret;
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                string[] strss = s.Split('_');
                if (strss.Length == 2)
                    ret[Convert.ToInt32(strss[0])] = strss[1];
            }
            return ret;
        }
        public static bool Clear(string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                if (mDatas.Count > 0)
                {
                    mDatas.Clear();
                    return true;
                }
            }
            else
            {
                return mDatas.Remove(key);
            }
            return false;
        }


        public static bool printLogWhenDuplicateCSV = true;
        #region InternalImpl
        static Dictionary<string, KissCSVImpl> mDatas = new Dictionary<string, KissCSVImpl>();
        private class KissCSVImpl
        {
            private Dictionary<string, string[]> mData = new Dictionary<string, string[]>();

            public KissCSVImpl(string fileName, string columnName, string fileContent)
            {
                Read(fileName, columnName, null, null, null, fileContent);
            }
            public KissCSVImpl(string fileName, string columnName, string columnName2, string fileContent)
            {
                Read(fileName, columnName, columnName2, null, null, fileContent);
            }
            public KissCSVImpl(string fileName, string columnName, string columnName2, string columnName3, string fileContent)
            {
                Read(fileName, columnName, columnName2, columnName3, null, fileContent);
            }

            public KissCSVImpl(string fileName, string columnName, string columnName2, string columnName3, string columnName4, string fileContent)
            {
                Read(fileName, columnName, columnName2, columnName3, columnName4, fileContent);
            }

            private Dictionary<string, int> mFirstLineData = new Dictionary<string, int>();
            private void Read(string fileName, string columnName, string columnName2, string columnName3, string columnName4, string strs)
            {
                if (string.IsNullOrEmpty(strs))
                {
                    try
                    {
                        string strRealPath = fileName;
                        if (File.Exists(Environment.CurrentDirectory + "/CSV/" + fileName))
                            strs = File.ReadAllText(Environment.CurrentDirectory + "/CSV/" + fileName);
                        else if (File.Exists(Environment.CurrentDirectory + "/" + fileName))
                            strs = File.ReadAllText(Environment.CurrentDirectory + "/" + fileName);
                        else if (File.Exists(fileName))
                            strs = File.ReadAllText(fileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("load " + fileName + " error " + e.ToString());
                        return;
                    }
                }
                if (string.IsNullOrEmpty(strs))
                    return;
                int keyIndex = -1;
                int keyIndex2 = -1;
                int keyIndex3 = -1;
                int keyIndex4 = -1;
                int keyCount = 0;
                int lenght = strs.Length;
                StringBuilder sb = new StringBuilder();
                List<string> lines = new List<string>();
                bool bQuotes = false;
                bool bFirstLine = true;
                for (int i = 0; i < lenght; i++)
                {
                    char c = strs[i];
                    switch (c)
                    {
                        case '"':
                            if (bQuotes)
                            {
                                if (i + 1 < lenght && strs[i + 1] == '"')
                                {
                                    i++;
                                    sb.Append(c);
                                }
                                else
                                    bQuotes = false;
                            }
                            else
                            {
                                bQuotes = true;
                            }
                            break;
                        case ',':
                            if (bQuotes)
                            {
                                sb.Append(c);
                            }
                            else
                            {
                                lines.Add(sb.ToString());
                                sb.Clear();
                            }
                            break;
                        case '\n':
                            if (bQuotes)
                            {
                                sb.Append(c);
                            }
                            else
                            {
                                lines.Add(sb.ToString());
                                sb.Clear();
                                if (bFirstLine)
                                {
                                    bFirstLine = false;
                                    for (int j = 0; j < lines.Count; j++)
                                    {
                                        string str = lines[j];
                                        mFirstLineData.Add(str, j);
                                        if (str.CompareTo(columnName) == 0)
                                        {
                                            keyIndex = j;
                                            keyCount++;
                                        }
                                        else if (str.CompareTo(columnName2) == 0)
                                        {
                                            keyIndex2 = j;
                                            keyCount++;
                                        }
                                        else if (str.CompareTo(columnName3) == 0)
                                        {
                                            keyIndex3 = j;
                                            keyCount++;
                                        }
                                        else if (str.CompareTo(columnName4) == 0)
                                        {
                                            keyIndex4 = j;
                                            keyCount++;
                                        }
                                    }

                                }
                                else
                                {
                                    string strColumnName;
                                    switch (keyCount)
                                    {
                                        case 1: strColumnName = lines[keyIndex]; break;
                                        case 2: strColumnName = lines[keyIndex] + "_" + lines[keyIndex2]; break;
                                        case 3: strColumnName = lines[keyIndex] + "_" + lines[keyIndex2] + "_" + lines[keyIndex3]; break;
                                        case 4: strColumnName = lines[keyIndex] + "_" + lines[keyIndex2] + "_" + lines[keyIndex3] + "_" + lines[keyIndex4]; break;
                                        default:
                                            Console.WriteLine("invalid key count.");
                                            return;
                                    }
                                    if (!mData.ContainsKey(strColumnName))
                                        mData.Add(strColumnName, lines.ToArray());
                                    else if (printLogWhenDuplicateCSV)
                                        Console.WriteLine($"Duplicate key : {strColumnName}, and will use the old one.");
                                }
                                lines.Clear();
                            }
                            break;
                        case '\r':
                            if (bQuotes)
                            {
                                sb.Append(c);
                            }
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            public List<int> GetIntListKeys()
            {
                List<int> results = new List<int>();
                foreach (var one in mData.Keys)
                    results.Add(Convert.ToInt32(one));
                return results;
            }
            public List<string> GetStringListKeys()
            {
                List<string> results = new List<string>();
                foreach (var one in mData.Keys)
                    results.Add(one);
                return results;
            }
            public string GetDataString(string key, string strName, string strDefault)
            {
                if (mData.ContainsKey(key) && mFirstLineData.ContainsKey(strName))
                {
                    string data = mData[key][mFirstLineData[strName]];
                    if (!string.IsNullOrEmpty(data))
                        return data;
                }
                return strDefault;
            }
        }
        #endregion
    }
}
