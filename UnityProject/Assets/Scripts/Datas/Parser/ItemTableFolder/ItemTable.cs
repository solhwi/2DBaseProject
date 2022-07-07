using System;
using System.Collections.Generic;


/// <summary>
/// !주의! 수동으로 조작하지 마시오. .Helper.cs에 편의성 함수를 추가하시오.
/// </summary>
[Serializable]
[ScriptParserAttribute("ItemTable.asset")]
public partial class ItemTable : ScriptParser
{
	public override void Parser()
	{
		ItemInfoDictionary.Clear();
		foreach(var value in ItemInfoList)
		{
			ItemInfoDictionary.Add(value.key, value);
		}
		ItemInfoTestDictionary.Clear();
		foreach(var value in ItemInfoTestList)
		{
			ItemInfoTestDictionary.Add(value.key, value);
		}
	}

	[Serializable]
	public class ItemInfo
	{
		public int key;
		public string itemType;
		public string itemSubType;
	}

	public List<ItemInfo> ItemInfoList = new List<ItemInfo>();
	[System.Serializable]
	public class SItemInfoDictionary : SerializableDictionary<int, ItemInfo> {}
	public SItemInfoDictionary ItemInfoDictionary = new SItemInfoDictionary();

	[Serializable]
	public class ItemInfoTest
	{
		public int key;
		public int Index;
		public int Type;
		public int SubType;
		public string Name;
	}

	public List<ItemInfoTest> ItemInfoTestList = new List<ItemInfoTest>();
	[System.Serializable]
	public class SItemInfoTestDictionary : SerializableDictionary<int, ItemInfoTest> {}
	public SItemInfoTestDictionary ItemInfoTestDictionary = new SItemInfoTestDictionary();


}
