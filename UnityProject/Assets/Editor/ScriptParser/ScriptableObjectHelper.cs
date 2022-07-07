using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectHelper
{
	public static TSO GenerateCustomScriptParser<TSO>()
		where TSO : CustomScriptParser // 읽어온 것으로부터 저장할 CustomScriptParser
	{
		var assetPath = AttributeUtil.GetResourcePath<TSO>();
		var assetType = typeof(TSO);

		var asset = GenerateSO(assetPath, assetType);
		return asset as TSO;
	}

	public static ScriptableObject GenerateSO(string assetPath, Type assetType)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(assetPath));

		var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType) as ScriptableObject;

		if (asset == null)
		{
			asset = ScriptableObject.CreateInstance(assetType.Name);
			AssetDatabase.CreateAsset((ScriptableObject)asset, assetPath);

			asset.hideFlags = HideFlags.NotEditable;
		}

		return asset;
	}
}
