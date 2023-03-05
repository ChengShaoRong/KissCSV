/*
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
    /// Support type in class as below ONLY:
    /// build-in type: string sbyte ushort uint ulong byte short int long bool float double DateTime
    /// List&lt;build-in type&gt; 
    /// Dictionary&lt;string,build-in type&gt;
    /// Dictionary&lt;int,build-in type&gt;
    /// </summary>
    public sealed class KissCSV
    {
        static Dictionary<string, Dictionary<string, object>> datas = new Dictionary<string, Dictionary<string, object>>();
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
        public static int Load(Type type, string fileName, string keyColumnName, string fileContext = null, string keyColumnName2 = null, string keyColumnName3 = null, string keyColumnName4 = null)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            datas[fileName] = data;
            object csv = type.Assembly.CreateInstance(type.FullName);
            if (string.IsNullOrEmpty(fileContext))
            {
                if (string.IsNullOrEmpty(keyColumnName4))
                {
                    if (string.IsNullOrEmpty(keyColumnName3))
                    {
                        if (string.IsNullOrEmpty(keyColumnName2))
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"");
                            SimpleKissCSV.Load(fileName, keyColumnName);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2, keyColumnName3);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4);
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
                            SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, fileContext);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, fileContext);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, fileContext);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4, fileContext);
                }
            }
            List<string> keys = SimpleKissCSV.GetStringListKeys(fileName);
            //Type type = csv.GetType();
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
                csv = type.Assembly.CreateInstance(type.FullName);
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
                    data[key] = csv;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Load \"{fileName}\" error {e.Message} in key = \"{key}\"! This line will be ignore.");
                }
            }
            SimpleKissCSV.Clear(fileName);
            Console.WriteLine($"Load \"{fileName}\" done, it have data count : {data.Count}.");
            return data.Count;
        }
#if _CSHARP_LIKE_
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
        public static int Load(SType type, string fileName, string keyColumnName, string fileContext = null, string keyColumnName2 = null, string keyColumnName3 = null, string keyColumnName4 = null)
        {
            UnityEngine.Debug.LogError("Load ");
            Dictionary<string, object> data = new Dictionary<string, object>();
            datas[fileName] = data;
            if (string.IsNullOrEmpty(fileContext))
            {
                if (string.IsNullOrEmpty(keyColumnName4))
                {
                    if (string.IsNullOrEmpty(keyColumnName3))
                    {
                        if (string.IsNullOrEmpty(keyColumnName2))
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"");
                            SimpleKissCSV.Load(fileName, keyColumnName);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2, keyColumnName3);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" (auto load) and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.Load(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4);
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
                            SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, fileContext);
                        }
                        else
                        {
                            Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"");
                            SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, fileContext);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"");
                        SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, fileContext);
                    }
                }
                else
                {
                    Console.WriteLine($"Load \"{fileName}\" and key column \"{keyColumnName}\"_\"{keyColumnName2}\"_\"{keyColumnName3}\"_\"{keyColumnName4}\"");
                    SimpleKissCSV.LoadWithFileContent(fileName, keyColumnName, keyColumnName2, keyColumnName3, keyColumnName4, fileContext);
                }
            }
            List<string> keys = SimpleKissCSV.GetStringListKeys(fileName);
            Dictionary<string, string> memberInfos = new Dictionary<string, string>();
            SInstance csv = type.New().value as SInstance;
            foreach (var member in csv.members)
            {
                string strShortName = GetShortName(member.Value.type.FullName);
                if (strShortName != null)
                    memberInfos.Add(member.Key, strShortName);
            }
            foreach (string key in keys)
            {
                csv = type.New().value as SInstance;
                try
                {
                    foreach (var one in memberInfos)
                    {
                        string strValue = SimpleKissCSV.GetString(fileName, key, one.Key);
                        csv.members[one.Key].value = GetValue(one.Value, strValue);
                    }
                    data[key] = csv;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Load \"{fileName}\" error {e.Message} in key = \"{key}\"! This line will be ignore.");
                }
            }
            SimpleKissCSV.Clear(fileName);
            Console.WriteLine($"Load \"{fileName}\" done, it have data count : {data.Count}.");
            return data.Count;
        }
#endif
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
                    case "List<System.String>": return new List<string>();
                    case "List<System.SByte>": return new List<sbyte>();
                    case "List<System.UInt16>": return new List<ushort>();
                    case "List<System.UInt32>": return new List<uint>();
                    case "List<System.UInt64>": return new List<long>();
                    case "List<System.Byte>": return new List<byte>();
                    case "List<System.Int16>": return new List<short>();
                    case "List<System.Int32>": return new List<int>();
                    case "List<System.Int64>": return new List<long>();
                    case "List<System.Boolean>": return new List<bool>();
                    case "List<System.Single>": return new List<float>();
                    case "List<System.Double>": return new List<double>();
                    case "List<System.DateTime>": return new List<DateTime>();
                    case "Dictionary<System.String,System.String>": return new Dictionary<string, string>();
                    case "Dictionary<System.String,System.SByte>": return new Dictionary<string, sbyte>();
                    case "Dictionary<System.String,System.UInt16>": return new Dictionary<string, ushort>();
                    case "Dictionary<System.String,System.UInt32>": return new Dictionary<string, uint>();
                    case "Dictionary<System.String,System.UInt64>": return new Dictionary<string, long>();
                    case "Dictionary<System.String,System.Byte>": return new Dictionary<string, byte>();
                    case "Dictionary<System.String,System.Int16>": return new Dictionary<string, short>();
                    case "Dictionary<System.String,System.Int32>": return new Dictionary<string, int>();
                    case "Dictionary<System.String,System.Int64>": return new Dictionary<string, long>();
                    case "Dictionary<System.String,System.Boolean>": return new Dictionary<string, bool>();
                    case "Dictionary<System.String,System.Single>": return new Dictionary<string, float>();
                    case "Dictionary<System.String,System.Double>": return new Dictionary<string, double>();
                    case "Dictionary<System.String,System.DateTime>": return new Dictionary<string, DateTime>();
                    case "Dictionary<System.Int32,System.String>": return new Dictionary<int, string>();
                    case "Dictionary<System.Int32,System.SByte>": return new Dictionary<int, sbyte>();
                    case "Dictionary<System.Int32,System.UInt16>": return new Dictionary<int, ushort>();
                    case "Dictionary<System.Int32,System.UInt32>": return new Dictionary<int, uint>();
                    case "Dictionary<System.Int32,System.UInt64>": return new Dictionary<int, long>();
                    case "Dictionary<System.Int32,System.Byte>": return new Dictionary<int, byte>();
                    case "Dictionary<System.Int32,System.Int16>": return new Dictionary<int, short>();
                    case "Dictionary<System.Int32,System.Int32>": return new Dictionary<int, int>();
                    case "Dictionary<System.Int32,System.Int64>": return new Dictionary<int, long>();
                    case "Dictionary<System.Int32,System.Boolean>": return new Dictionary<int, bool>();
                    case "Dictionary<System.Int32,System.Single>": return new Dictionary<int, float>();
                    case "Dictionary<System.Int32,System.Double>": return new Dictionary<int, double>();
                    case "Dictionary<System.Int32,System.DateTime>": return new Dictionary<int, DateTime>();
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
        public static int GetCount(string fileName)
        {
#if _CSHARP_LIKE_
            if (dataExs.ContainsKey(fileName))
                return dataExs[fileName].Count;
            return datas[fileName].Count;
#else
            return datas[fileName].Count;
#endif
        }
        /// <summary>
        /// The dictionary data in memory
        /// </summary>
        public static Dictionary<string, object> GetData(string fileName)
        {
            if (datas.TryGetValue(fileName, out Dictionary<string, object> ret))
            {
                return ret;
            }
            return null;
        }
#if _CSHARP_LIKE_
        /// <summary>
        /// The dictionary data in memory
        /// </summary>
        public static Dictionary<string, SInstance> GetDataEx(string fileName)
        {
            if (dataExs.TryGetValue(fileName, out Dictionary<string, SInstance> ret))
            {
                return ret;
            }
            return null;
        }
#endif
        /// <summary>
        /// Get the one row data by custom unique key
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="strUniqueKey">unique key string</param>
        /// <returns>one row data in a class</returns>
        public static object Get(string fileName, string strUniqueKey)
        {
#if _CSHARP_LIKE_
            if (dataExs.ContainsKey(fileName))
            {
                if (dataExs[fileName].TryGetValue(strUniqueKey, out SInstance value))
                    return value;
            }
            else
            {
                if (datas[fileName].TryGetValue(strUniqueKey, out object value))
                    return value;
            }
#else
            if (datas[fileName].TryGetValue(strUniqueKey, out object value))
                return value;
#endif
            return default;
        }
        /// <summary>
        /// Get the one row data by custom unique key
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="uniqueKey">unique key value in Int32</param>
        /// <returns>one row data in a class</returns>
        public static object Get(string fileName, int uniqueKey)
        {
            return Get(fileName, uniqueKey + "");
        }
        /// <summary>
        /// Get the one row data by custom unique key (two column as the unique key)
        /// </summary>
        /// <param name="strUniqueKey">column name 1</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <returns>one row data in a class</returns>
        public static object Get(string fileName, string strUniqueKey, string strUniqueKey2)
        {
#if _CSHARP_LIKE_
            if (dataExs.ContainsKey(fileName))
            {
                if (dataExs[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2, out SInstance value))
                    return value;
            }
            else
            {
                if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2, out object value))
                    return value;
            }
#else
            if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2, out object value))
                return value;
#endif
            return default;
        }
        /// <summary>
        /// Get the one row data by custom unique key (three column as the unique key)
        /// </summary>
        /// <param name="strUniqueKey">column name 1</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <returns>one row data in a class</returns>
        public static object Get(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3)
        {
#if _CSHARP_LIKE_
            if (dataExs.ContainsKey(fileName))
            {
                if (dataExs[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3, out SInstance value))
                    return value;
            }
            else
            {
                if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3, out object value))
                    return value;
            }
#else
            if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3, out object value))
                return value;
#endif
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
        public static object Get(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3, string strUniqueKey4)
        {
#if _CSHARP_LIKE_
            if (dataExs.ContainsKey(fileName))
            {
                if (dataExs[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4, out SInstance value))
                    return value;
            }
            else
            {
                if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4, out object value))
                    return value;
            }
#else
            if (datas[fileName].TryGetValue(strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4, out object value))
                return value;
#endif
            return default;
        }
        /// <summary>
        /// Add or replace a row data by your self
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string fileName, string strUniqueKey, object csv)
        {
            datas[fileName][strUniqueKey] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="strUniqueKey">custom key name</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, object csv)
        {
            datas[fileName][strUniqueKey + "_" + strUniqueKey2] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3, object csv)
        {
            datas[fileName][strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3] = csv;
        }
        /// <summary>
        /// Add or replace a row data by your self (two column as the unique key)
        /// Just modify the data in memory, it won't save into file.
        /// </summary>
        /// <param name="fileName">The file name of the CSV file</param>
        /// <param name="strUniqueKey">custom key</param>
        /// <param name="strUniqueKey2">column name 2</param>
        /// <param name="strUniqueKey3">column name 3</param>
        /// <param name="strUniqueKey4">column name 4</param>
        /// <param name="csv">your custom data</param>
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3, string strUniqueKey4, object csv)
        {
            datas[fileName][strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4] = csv;
        }

#if _CSHARP_LIKE_
        static Dictionary<string, Dictionary<string, SInstance>> dataExs = new Dictionary<string, Dictionary<string, SInstance>>();
        public static void Set(string fileName, string strUniqueKey, SInstance csv)
        {
            dataExs[fileName][strUniqueKey] = csv;
        }
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, SInstance csv)
        {
            dataExs[fileName][strUniqueKey + "_" + strUniqueKey2] = csv;
        }
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3, SInstance csv)
        {
            dataExs[fileName][strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3] = csv;
        }
        public static void Set(string fileName, string strUniqueKey, string strUniqueKey2, string strUniqueKey3, string strUniqueKey4, SInstance csv)
        {
            dataExs[fileName][strUniqueKey + "_" + strUniqueKey2 + "_" + strUniqueKey3 + "_" + strUniqueKey4] = csv;
        }
#endif
    }
}