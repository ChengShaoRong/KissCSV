﻿/*
 *           KissCSV
 * Copyright © 2023 RongRong. All right reserved.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
#if _CSHARP_LIKE_
using CSharpLike.Internal;
#endif

namespace CSharpLike
{
    /// <summary>
    /// A most simple and stupid way get data from CSV(Comma-Separated Values) file with 'RFC 4180'.
    /// Read data from CSV file with class (NOT struct!).
    /// Support type in class or struct as below ONLY:
    /// build-in type: string sbyte ushort uint ulong byte short int long bool float double DateTime
    /// List&lt;build-in type&gt; 
    /// Dictionary&lt;string,build-in type&gt;
    /// Dictionary&lt;int,build-in type&gt;
    /// </summary>
    public sealed class KissCSV<T> where T : new()
    {
        static Dictionary<string, T> datas = new Dictionary<string, T>();
        /// <summary>
        /// Initalize the CSV file into memory, just need call one time. Recall it if you reed reload it.
        /// </summary>
        /// <param name="fileName">File name of the CSV file, will load it from '.\\CSV\\' or '.\\' by 'File.ReadAllText', MUST be unique.</param>
        /// <param name="keyColumnName">The column name of unique id in this CSV file</param>
        /// <param name="fileContext">The CSV file conten, if not null, will direct use it, and won't load by fileName, because you may load the CSV file from AssetBundle or Addressables. If null will load from fileName in ".\\CSV\\" or ".\\". Default is null.</param>
        /// <param name="keyColumnName2">The second column name in this CSV file, default is null. If this CSV have 2 more columns as unique key, set it not null. </param>
        /// <param name="keyColumnName3">The third column name in this CSV file, default is null. If this CSV have 3 more columns as unique key, set it not null.</param>
        /// <param name="keyColumnName4">The fourth column name in this CSV file, default is null. If this CSV have 4 more columns as unique key, set it not null.</param>
        /// <returns></returns>
        public static int Load(string fileName, string keyColumnName, string fileContext = null, string keyColumnName2 = null, string keyColumnName3 = null, string keyColumnName4 = null)
        {
            datas.Clear();
            T csv = new T();
            if (string.IsNullOrEmpty(fileContext))
            {
                if (string.IsNullOrEmpty(keyColumnName4))
                {
                    if (string.IsNullOrEmpty(keyColumnName3))
                    {
                        if (string.IsNullOrEmpty(keyColumnName2))
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"");
                            SimpleKissCSV.Init(fileName, keyColumnName);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.Init(fileName, keyColumnName, keyColumnName2);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.Init(fileName, keyColumnName, keyColumnName2, keyColumnName3);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.Init(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(keyColumnName4))
                {
                    if (string.IsNullOrEmpty(keyColumnName3))
                    {
                        if (string.IsNullOrEmpty(keyColumnName2))
                        {
                            Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"");
                            SimpleKissCSV.InitWithFileContent(fileName, keyColumnName, fileContext);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.InitWithFileContent(fileName, keyColumnName, keyColumnName2, fileContext);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.InitWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, fileContext);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.InitWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4, fileContext);
                }
            }
            List<string> keys = SimpleKissCSV.GetStringListKeys(fileName);
            Type type = csv.GetType();
            FieldInfo[] fs = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            Dictionary<FieldInfo, string> fieldInfos = new Dictionary<FieldInfo, string>();
            foreach (var f in fs)
            {
                if (f.FieldType.IsEnum)
                {
                    fieldInfos.Add(f, "IsEnum");
                    continue;
                }
                string strShortName = GetShortName(f.FieldType.FullName);
                if (strShortName != null)
                    fieldInfos.Add(f, strShortName);
            }
            PropertyInfo[] ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
            Dictionary<PropertyInfo, string> propertys = new Dictionary<PropertyInfo, string>();
            foreach (var f in ps)
            {
                if (f.PropertyType.IsEnum)
                {
                    propertys.Add(f, "IsEnum");
                    continue;
                }
                string strShortName = GetShortName(f.PropertyType.FullName);
                if (strShortName != null)
                    propertys.Add(f, strShortName);
            }

            foreach (string key in keys)
            {
                csv = new T();
                try
                {
                    foreach(var one in fieldInfos)
                    {
                        string strValue = SimpleKissCSV.GetString(fileName, key, one.Key.Name);
                        if (one.Key.FieldType.IsEnum)
                            one.Key.SetValue(csv, Enum.Parse(one.Key.FieldType, strValue));
                        else
                            one.Key.SetValue(csv, GetValue(one.Value, strValue));
                    }
                    foreach (var one in propertys)
                    {
                        string strValue = SimpleKissCSV.GetString(fileName, key, one.Key.Name);
                        if (one.Key.PropertyType.IsEnum)
                            one.Key.SetValue(csv, Enum.Parse(one.Key.PropertyType, strValue));
                        else
                            one.Key.SetValue(csv, GetValue(one.Value, strValue));
                    }
                    datas[key] = csv;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Load \"{fileName}\" error {e.Message} in key = \"{key}\"! This line will be ignore.");
                }
            }
            SimpleKissCSV.Clear(fileName);
            Console.WriteLine($"Load \"{fileName}\" done, it have data count : {datas.Count}.");
            return datas.Count;
        }
        private static object GetValue(string strFullName, string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                switch (strFullName)
                {
                    case "System.String": return strValue;
                    case "System.SByte": return default(sbyte);
                    case "System.UInt16": return default(ushort);
                    case "System.UInt32": return default(uint);
                    case "System.UInt64": return default(ulong);
                    case "System.Byte": return default(byte);
                    case "System.Int16": return default(short);
                    case "System.Int32": return default(int);
                    case "System.Int64": return default(long);
                    case "System.Boolean": return default(bool);
                    case "System.Single": return default(float);
                    case "System.Double": return default(double);
                    case "System.DateTime": return default(DateTime);
                    default: return null;
                }
            }
            switch (strFullName)
            {
                case "System.String": return strValue;
                case "System.SByte": return Convert.ToSByte(strValue);
                case "System.UInt16": return Convert.ToUInt16(strValue);
                case "System.UInt32": return Convert.ToUInt32(strValue);
                case "System.UInt64": return Convert.ToUInt64(strValue);
                case "System.Byte": return Convert.ToByte(strValue);
                case "System.Int16": return Convert.ToInt16(strValue);
                case "System.Int32": return Convert.ToInt32(strValue);
                case "System.Int64": return Convert.ToInt64(strValue);
                case "System.Boolean":
                    if (strValue.Length == 1) return strValue != "0";
                    else return Convert.ToBoolean(strValue);
                case "System.Single": return Convert.ToSingle(strValue, CultureInfo.InvariantCulture);
                case "System.Double": return Convert.ToDouble(strValue, CultureInfo.InvariantCulture);
                case "System.DateTime": return Convert.ToDateTime(strValue);
                case "List<System.String>":
                    {
                        List<string> ret = new List<string>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add(s);
                        return ret;
                    }
                case "List<System.SByte>":
                    {
                        List<sbyte> ret = new List<sbyte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToSByte(s));
                        return ret;
                    }
                case "List<System.UInt16>":
                    {
                        List<ushort> ret = new List<ushort>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToUInt16(s));
                        return ret;
                    }
                case "List<System.UInt32>":
                    {
                        List<uint> ret = new List<uint>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToUInt32(s));
                        return ret;
                    }
                case "List<System.UInt64>":
                    {
                        List<ulong> ret = new List<ulong>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToUInt64(s));
                        return ret;
                    }
                case "List<System.Byte>":
                    {
                        List<byte> ret = new List<byte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToByte(s));
                        return ret;
                    }
                case "List<System.Int16>":
                    {
                        List<short> ret = new List<short>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToInt16(s));
                        return ret;
                    }
                case "List<System.Int32>":
                    {
                        List<int> ret = new List<int>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToInt32(s));
                        return ret;
                    }
                case "List<System.Int64>":
                    {
                        List<long> ret = new List<long>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToInt64(s));
                        return ret;
                    }
                case "List<System.Single>":
                    {
                        List<float> ret = new List<float>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToSingle(s, CultureInfo.InvariantCulture));
                        return ret;
                    }
                case "List<System.Double>":
                    {
                        List<double> ret = new List<double>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToDouble(s, CultureInfo.InvariantCulture));
                        return ret;
                    }
                case "List<System.DateTime>":
                    {
                        List<DateTime> ret = new List<DateTime>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : Convert.ToDateTime(s));
                        return ret;
                    }
                case "List<System.Boolean>":
                    {
                        List<bool> ret = new List<bool>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                            ret.Add((s.Length == 0) ? default : ((s.Length == 1) ? s != "0" : Convert.ToBoolean(s)));
                        return ret;
                    }
                case "Dictionary<System.String,System.String>":
                    {
                        Dictionary<string, string> ret = new Dictionary<string, string>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = strss[1];
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.SByte>":
                    {
                        Dictionary<string, sbyte> ret = new Dictionary<string, sbyte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToSByte(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.UInt16>":
                    {
                        Dictionary<string, ushort> ret = new Dictionary<string, ushort>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToUInt16(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.UInt32>":
                    {
                        Dictionary<string, uint> ret = new Dictionary<string, uint>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToUInt32(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.UInt64>":
                    {
                        Dictionary<string, ulong> ret = new Dictionary<string, ulong>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToUInt64(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Byte>":
                    {
                        Dictionary<string, byte> ret = new Dictionary<string, byte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToByte(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Int16>":
                    {
                        Dictionary<string, short> ret = new Dictionary<string, short>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToInt16(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Int32>":
                    {
                        Dictionary<string, int> ret = new Dictionary<string, int>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToInt32(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Int64>":
                    {
                        Dictionary<string, long> ret = new Dictionary<string, long>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToInt64(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Boolean>":
                    {
                        Dictionary<string, bool> ret = new Dictionary<string, bool>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = ((strss[1].Length == 0) ? default : ((strss[1].Length == 1) ? strss[1] != "0" : Convert.ToBoolean(strss[1])));
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Single>":
                    {
                        Dictionary<string, float> ret = new Dictionary<string, float>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToSingle(strss[1], CultureInfo.InvariantCulture);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.Double>":
                    {
                        Dictionary<string, double> ret = new Dictionary<string, double>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToDouble(strss[1], CultureInfo.InvariantCulture);
                        }
                        return ret;
                    }
                case "Dictionary<System.String,System.DateTime>":
                    {
                        Dictionary<string, DateTime> ret = new Dictionary<string, DateTime>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[strss[0]] = Convert.ToDateTime(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.String>":
                    {
                        Dictionary<int, string> ret = new Dictionary<int, string>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = strss[1];
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.SByte>":
                    {
                        Dictionary<int, sbyte> ret = new Dictionary<int, sbyte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToSByte(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.UInt16>":
                    {
                        Dictionary<int, ushort> ret = new Dictionary<int, ushort>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToUInt16(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.UInt32>":
                    {
                        Dictionary<int, uint> ret = new Dictionary<int, uint>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToUInt32(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.UInt64>":
                    {
                        Dictionary<int, ulong> ret = new Dictionary<int, ulong>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToUInt64(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Byte>":
                    {
                        Dictionary<int, byte> ret = new Dictionary<int, byte>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToByte(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Int16>":
                    {
                        Dictionary<int, short> ret = new Dictionary<int, short>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToInt16(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Int32>":
                    {
                        Dictionary<int, int> ret = new Dictionary<int, int>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToInt32(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Int64>":
                    {
                        Dictionary<int, long> ret = new Dictionary<int, long>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToInt64(strss[1]);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Boolean>":
                    {
                        Dictionary<int, bool> ret = new Dictionary<int, bool>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = ((strss[1].Length == 0) ? default : ((strss[1].Length == 1) ? strss[1] != "0" : Convert.ToBoolean(strss[1])));
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Single>":
                    {
                        Dictionary<int, float> ret = new Dictionary<int, float>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToSingle(strss[1], CultureInfo.InvariantCulture);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.Double>":
                    {
                        Dictionary<int, double> ret = new Dictionary<int, double>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToDouble(strss[1], CultureInfo.InvariantCulture);
                        }
                        return ret;
                    }
                case "Dictionary<System.Int32,System.DateTime>":
                    {
                        Dictionary<int, DateTime> ret = new Dictionary<int, DateTime>();
                        string[] strs = strValue.Split('|');
                        foreach (string s in strs)
                        {
                            string[] strss = s.Split('_');
                            if (strss.Length == 2)
                                ret[Convert.ToInt32(strss[0])] = Convert.ToDateTime(strss[1]);
                        }
                        return ret;
                    }
                default:
                    return null;
            }
        }
        private static string GetShortName(string strFullName)
        {
            switch (strFullName)
            {
                case "System.String":
                case "System.SByte":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Boolean":
                case "System.Single":
                case "System.Double":
                case "System.DateTime":
                    return strFullName;
                default:
                    if (strFullName.StartsWith("System.Collections.Generic.List`1["))//List<
                    {
                        return $"List<{strFullName.Substring(35, strFullName.IndexOf(',', 35) - 35)}>";
                    }
                    else if (strFullName.StartsWith("System.Collections.Generic.Dictionary`2[["))//Dictionary<
                    {
                        int iEnd = strFullName.IndexOf(',', 41);
                        string strFirst = strFullName.Substring(41, iEnd - 41);
                        int iStart = strFullName.IndexOf("],[", iEnd);
                        iEnd = strFullName.IndexOf(',', iStart + 3);
                        string strSecond = strFullName.Substring(iStart + 3, iEnd - iStart - 3);
                        return $"Dictionary<{strFirst},{strSecond}>";
                    }
                    return null;
            }
        }
        /// <summary>
        /// Row count of this CSV file
        /// </summary>
        public static int Count
        {
            get
            {
                return datas.Count;
            }
        }
        /// <summary>
        /// The dictionary data in memory
        /// </summary>
        public static Dictionary<string, T> Data
        {
            get
            {
                return datas;
            }
        }
        /// <summary>
        /// Get the one row data by custom unique key
        /// </summary>
        /// <param name="strUniqueKey">unique key string</param>
        /// <returns>one row data in a class</returns>
        public static T Get(string strUniqueKey)
        {
            if (datas.TryGetValue(strUniqueKey, out T value))
                return value;
            return default;
        }
        /// <summary>
        /// Get the one row data by custom unique key
        /// </summary>
        /// <param name="strUniqueKey">unique key value in Int32</param>
        /// <returns>one row data in a class</returns>
        public static T Get(int uniqueKey)
        {
            return Get(uniqueKey + "");
        }
        /// <summary>
        /// Get the one row data by custom unique key (two column as the unique key)
        /// </summary>
        /// <param name="strUniqueKey">column name 1</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <returns>one row data in a class</returns>
        public static T Get(string strUniqueKey, string strUniqueKey2)
        {
            if (datas.TryGetValue(strUniqueKey + "_" + strUniqueKey2, out T value))
                return value;
            return default;
        }
        /// <summary>
        /// Get the one row data by custom unique key (three column as the unique key)
        /// </summary>
        /// <param name="strUniqueKey">column name 1</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <returns>one row data in a class</returns>
        public static T Get(string strUniqueKey, string strUniqueKey2, string strUniqueKey3)
        {
            if (datas.TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3, out T value))
                return value;
            return default;
        }
        /// <summary>
        /// Get the one row data by custom unique key (four column as the unique key)
        /// </summary>
        /// <param name="strUniqueKey">column name 1</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <param name="strUniqueKey4">column name 4</param>
        /// <returns>one row data in a class</returns>
        public static T Get(string strUniqueKey, string strUniqueKey2, string strUniqueKey3, string strUniqueKey4)
        {
            if (datas.TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4, out T value))
                return value;
            return default;
        }
        /// <summary>
        /// Add or replace a row data by your self
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string strUniqueKey, T csv)
        {
            datas[strUniqueKey] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="strUniqueKey">custom key name</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string strUniqueKey, string strUniqueKey2, T csv)
        {
            datas[strUniqueKey + "_" + strUniqueKey2] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string strUniqueKey, string strUniqueKey2, string strUniqueKey3, T csv)
        {
            datas[strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <param name="strUniqueKey4">column name 4</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string strUniqueKey, string strUniqueKey2, string strUniqueKey3, string strUniqueKey4, T csv)
        {
            datas[strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4] = csv;
        }
    }
}