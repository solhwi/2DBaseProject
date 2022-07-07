using System;
using System.Collections.Generic;


/// <summary>
/// !주의! 수동으로 조작하지 마시오. .Helper.cs에 편의성 함수를 추가하시오.
/// </summary>
[Serializable]
[ScriptParserAttribute("ItemSequenceTable.asset")]
public partial class ItemSequenceTable : ScriptParser
{
	public override void Parser()
	{
		ItemSequenceInfoDictionary.Clear();
		foreach(var value in ItemSequenceInfoList)
		{
			ItemSequenceInfoDictionary.Add(value.key, value);
		}
	}

	[Serializable]
	public class ItemSequenceInfo
	{
		public int key;
		public int objId;
	}

	public List<ItemSequenceInfo> ItemSequenceInfoList = new List<ItemSequenceInfo>();
	[System.Serializable]
	public class SItemSequenceInfoDictionary : SerializableDictionary<int, ItemSequenceInfo> {}
	public SItemSequenceInfoDictionary ItemSequenceInfoDictionary = new SItemSequenceInfoDictionary();


}
