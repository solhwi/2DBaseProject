using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class JsonParserHelper
{
    public const string JsonRootPath = "../Common/JsonDatas/";

	public static void GenerateCustomScriptParser<TData, TSO>(TData data)
		where TData : class // 읽어올 Json 데이터 클래스 형식, 반드시 Serializable한 클래스여야 한다.
		where TSO : CustomScriptParser // 읽어온 것으로부터 저장할 CustomScriptParser, TData를 포함한 클래스여야 한다.
	{
		var dataType = typeof(TData);
		if (!dataType.IsSerializable)
			return;

		var asset = ScriptableObjectHelper.GenerateCustomScriptParser<TSO>();

		var soType = typeof(TSO);
		var fields = soType.GetFields();

		foreach (var field in fields)
		{
			if (field.FieldType == typeof(TData))
			{
				field.SetValue(asset, data);
				break;
			}
		}

		asset.CustomParser();

		EditorUtility.SetDirty(asset);
	}

	public static void GenerateCustomScriptParser<TData, TSO>(string jsonFilePath)
		where TData : class
		where TSO : CustomScriptParser
	{
		TData data = ReadJson<TData>(jsonFilePath);

		var dataType = typeof(TData);
		if (!dataType.IsSerializable)
			return;

		var asset = ScriptableObjectHelper.GenerateCustomScriptParser<TSO>();

		var soType = typeof(TSO);
		var fields = soType.GetFields();

		foreach(var field in fields)
        {
			if(field.ReflectedType == typeof(TData))
            {
				field.SetValue(asset, data);
				break;
			}
		}

		asset.CustomParser();

		EditorUtility.SetDirty(asset);
	}

	public static void WriteJson<T>(string filePath, T mapData) where T : class
    {
		string jsonData = JsonUtility.ToJson(mapData, true);

		using (FileStream fs = new FileStream(JsonRootPath + filePath, FileMode.Create, FileAccess.Write))
		{
			byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

			fs.Write(byteData, 0, byteData.Length);
			fs.Close();
		}
	}

	public static T ReadJson<T>(string filePath) where T : class
    {
		string path = JsonRootPath + filePath;

		using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			byte[] byteData = new byte[fs.Length];

			fs.Read(byteData, 0, byteData.Length);
			fs.Close();

			string JsonData = Encoding.UTF8.GetString(byteData);
			return JsonUtility.FromJson<T>(JsonData);
		}
	}
}
