using System;
using System.Collections.Generic;


/// <summary>
/// !주의! 수동으로 조작하지 마시오. .Helper.cs에 편의성 함수를 추가하시오.
/// </summary>
[Serializable]
[ScriptParserAttribute("CharacterAnimationTable.asset")]
public partial class CharacterAnimationTable : ScriptParser
{
	public override void Parser()
	{
		CharacterAnimationPathDictionary.Clear();
		foreach(var value in CharacterAnimationPathList)
		{
			CharacterAnimationPathDictionary.Add(value.key, value);
		}
	}

	[Serializable]
	public class CharacterAnimationPath
	{
		public int key;
		public string animatorController;
		public string idle;
		public string idleFire;
		public string idleReload;
		public string idleMenu;
		public string runB;
		public string runF;
		public string runL;
		public string runR;
		public string walkB;
		public string walkF;
		public string walkL;
		public string walkR;
		public string runFire;
		public string sprint;
	}

	public List<CharacterAnimationPath> CharacterAnimationPathList = new List<CharacterAnimationPath>();
	[System.Serializable]
	public class SCharacterAnimationPathDictionary : SerializableDictionary<int, CharacterAnimationPath> {}
	public SCharacterAnimationPathDictionary CharacterAnimationPathDictionary = new SCharacterAnimationPathDictionary();


}
