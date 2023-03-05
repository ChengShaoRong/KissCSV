# Introduction
This is a most simple and stupid way get data from CSV(Comma-Separated Values) file with [RFC 4180](https://datatracker.ietf.org/doc/rfc4180/). It part of  [C#Like](https://assetstore.unity.com/packages/tools/integration/c-likefree-hot-update-framework-222880) and [C#LikeFree](https://assetstore.unity.com/packages/tools/integration/c-like-hot-update-framework-222256) that I uploaded to the Unity Asset Store. You can get the [C#LikeFree](https://github.com/ChengShaoRong/CSharpLikeFree) from GitHub too.  

***
这个是最简单易用读取符合[RFC 4180](https://datatracker.ietf.org/doc/rfc4180/)规范的CSV(Comma-Separated Values)文档. 它是我上传到Unity资源商店里的[C#Like](https://assetstore.unity.com/packages/tools/integration/c-likefree-hot-update-framework-222880) 和 [C#Like免费版](https://assetstore.unity.com/packages/tools/integration/c-like-hot-update-framework-222256) 的一部分. 你也可以在GitHub里下载到[C#Like免费版](https://github.com/ChengShaoRong/CSharpLikeFree).  


# Install
Package had been uploaded to Nuget, dependent library: **.NET Standard 2.0**. You can install by command ``Install-Package KissCSV``
***
包已上传至Nuget,依赖库为: **.NET Standard 2.0**. 你可以通过``Install-Package KissCSV``来安装.

# Usage
e.g. we want to read the 'TestCsv.csv' file which take column 'id' as the unique index, and the file contents is below:
```
id,number,name,testInts,testStrings,testFloats,testStringIntDicts,testIntBooleanDicts
1,5,"test name",1|2,"aa|bb",88.8,"ab_2|cd_4",5_true|6_false
101,6,name2,,,,,
```
* Step 1: Define a class for the row data. The class attribute name should be exactly the same as the column name, and it's **case sensitive**.
```
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
    }
```
* Step 2: Load it into memory. **Just call it once**. Recall it if you need to reload the CSV file.
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id");
```
* Step 3: Get the a row data into a class object.
```
	TestCsv csv = KissCSV.Get("TestCsv.csv", "1") as TestCsv;
	if (csv != null)//If not exist "1" in column "id" will return null.
	{
	    Console.WriteLine($"id={csv.id}");//output id=1
	    Console.WriteLine($"name={csv.name}");//output name=test name
	    string strPrint = "testStrings=";
	    foreach(var one in csv.testInts)
		    strPrint += one + ",";
	    Console.WriteLine(strPrint);//output testStrings=aa,bb
	    strPrint = "testStringIntDicts=";
	    foreach(var one in csv.testStringIntDicts)
		    strPrint += "{" + one.Key + "," + one.Value + "},";
	    Console.WriteLine(strPrint);//output testStringIntDicts={ab,2},{cd,4}
	}
```

***
例如我们要读取TestCsv.csv这个文件,该文件以id这列为唯一索引,文件里面内容为
```
id,number,name,testInts,testStrings,testFloats,testStringIntDicts,testIntBooleanDicts
1,5,"test name",1|2,"aa|bb",88.8,"ab_2|cd_4",5_true|6_false
101,6,name2,,,,,
```
* 步骤1: 为每行数据定义一个类. 注意属性名字要和表头的名字完全一致(**区分大小写**)
```
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
    }
```
* 步骤2: 加载如内存中. **只需要调用1次**, 如果你需要重新加载文件, 请再次调用.
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id");
```
* 步骤3: 根据索引获取整行数据到一个类对象.
```
	TestCsv csv = KissCSV.Get("TestCsv.csv", "1") as TestCsv;
	if (csv != null)//如果"id"这列不存在"1"这个数据会返回null
	{
	    Console.WriteLine($"id={csv.id}");//输出 id=1
	    Console.WriteLine($"name={csv.name}");//输出 name=test name
	    string strPrint = "testStrings=";
	    foreach(var one in csv.testInts)
		    strPrint += one + ",";
	    Console.WriteLine(strPrint);//输出 testStrings=aa,bb
	    strPrint = "testStringIntDicts=";
	    foreach(var one in csv.testStringIntDicts)
		    strPrint += "{" + one.Key + "," + one.Value + "},";
	    Console.WriteLine(strPrint);//输出 testStringIntDicts={ab,2},{cd,4}
	}
```

# Q&A
* What if there is not only one unique index of my file? e.g. the 'id' and 'number' in TestCsv above constitute a unique index
>We support a unique index composed of up to 4 columns
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id", null, "number");
	TestCsv csv = KissCSV.Get("TestCsv.csv", "1", "5") as TestCsv; 
```

* Why not use style like  ``KissCSV<TestCsv>.Load("TestCsv.csv", "id");``? It seem to be more tidy, and don't need to convert while get it.
>That style not support in hot update script of C#Like. For compatibility we have to using ``typeof(TestCsv)``. 

* I don't want to define a new class. Can I read the data in CSV directly?
>>You can read by SimpleKissCSV.
```
	SimpleKissCSV.Load("TestCsv.csv", "id");
	SimpleKissCSV.GetInt("TestCsv.csv", "1", "number")); 
	SimpleKissCSV.GetString("TestCsv.csv", "1", "name"));
```

* What types do the class for the row data support?
>We had supported types as below:
```
	Build-in type:string sbyte ushort uint ulong byte short int long bool float double DateTime
	List<Build-in type>
	Dictionary<string, Build-in type>
	Dictionary<int, Build-in type>  
```


* How are the List and Dictionary in the class split?
>List split by '|'. e.g. "1|2" split into {1,2}  
>Dictionary split by '|', and then split by '_'. e.g. "ab_2|cd_4" split into {{"ab",2},{"cd",4}}

* Can I add custom type in class?
>You can modify function  ``KissCSV.GetValue``.


* Where are the 'TestCsv.csv' put into?
>In folder ``.\CSV\`` or ``.\``

* Can I read 'TestCsv.csv' by myself??
>You can read that file and then pass parameter. e.g. 
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id", File.ReadAllText("./CSV/TestCsv.csv"));
```

***

* 如果我的文件唯一索引不是只有1个,怎么办?例如上面的TestCsv里的id和number组成唯一索引.
>我们支持最多4列组成的唯一索引
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id", null, "number");
	TestCsv csv = KissCSV.Get("TestCsv.csv", "1", "5") as TestCsv;
```


* 为什么不采用``KissCSV<TestCsv>.Load("TestCsv.csv", "id");``这种看起来更简洁的方式?而且获取的时候还得强转一次.
>C#Like的热更脚本不支持,为了兼容,统一使用传入``typeof(TestCsv)``的方式  

* 我不想新定义一个类,能否直接读取CSV里的数据?
>可以使用SimpleKissCSV来读取
```
	SimpleKissCSV.Load("TestCsv.csv", "id");
	SimpleKissCSV.GetInt("TestCsv.csv", "1", "number"));
	SimpleKissCSV.GetString("TestCsv.csv", "1", "name")); 
```


* 每列数据的类支持哪些类型的数据?
>已支持的类型如下:
```
	内置类型:string sbyte ushort uint ulong byte short int long bool float double DateTime
	List<内置类型>
	Dictionary<string, 内置类型>
	Dictionary<int, 内置类型>  
```

* 每列数据的类里属性为List和Dictionary的怎么分割的?
>List以'|'分割, 例如1|2分割成{1,2}  
>Dictionary以'|'分割,然后再以'_'分割, 例如"ab_2|cd_4"分割成{{"ab",2},{"cd",4}}

* 每列数据的类,我是否能够新增自定义类型
>请自行修改``KissCSV.GetValue``这个函数

* "TestCsv.csv"这个文件放在哪里文件夹内?
>在``.\CSV\``或``.\``目录内

* 我可否自行读取"TestCsv.csv"这个文件?
>你可以自行读取文件,然后传入参数,例如
```
	KissCSV.Load(typeof(TestCsv), "TestCsv.csv", "id", File.ReadAllText("./CSV/TestCsv.csv"));
```