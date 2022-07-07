using System;
using System.Collections.Generic;


/// <summary>
/// !주의! 수동으로 조작하지 마시오. .Helper.cs에 편의성 함수를 추가하시오.
/// </summary>
[Serializable]
[ScriptParserAttribute("MapPathTable.asset")]
public partial class MapPathTable : ScriptParser
{
	public override void Parser()
	{
		MapPathPairDictionary.Clear();
		foreach(var value in MapPathPairList)
		{
			MapPathPairDictionary.Add(value.key, value);
		}
	}

	[Serializable]
	public class MapPathPair
	{
		public int key;
		public string MapPath;
	}

	public List<MapPathPair> MapPathPairList = new List<MapPathPair>();
	[System.Serializable]
	public class SMapPathPairDictionary : SerializableDictionary<int, MapPathPair> {}
	public SMapPathPairDictionary MapPathPairDictionary = new SMapPathPairDictionary();


}
