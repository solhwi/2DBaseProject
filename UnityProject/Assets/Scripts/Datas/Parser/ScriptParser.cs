using System;
using UnityEngine;

[System.AttributeUsage(AttributeTargets.Class)]
public class ScriptParserAttribute : ResourceAttribute
{
    public ScriptParserAttribute(string path) : base("Assets/Bundles/Datas/Parser/" + path)
    {
        
    }
}

[System.AttributeUsage(AttributeTargets.Class)]
public class CustomScriptParserAttribute : ResourceAttribute
{
    public CustomScriptParserAttribute(string path) : base("Assets/Bundles/Datas/CustomParser/" + path)
    {

    }
}

public class ScriptAssetInfo
{
    public Type AssetType { get; set; }
    public ScriptParserAttribute Attribute { get; set; }
}

public abstract class ScriptParser : ScriptableObject
{
    // 기본 Parser, List, Dictionary를 만들어 줌
	public abstract void Parser();
    public virtual void CustomParser() { }
}

public abstract class CustomScriptParser : ScriptableObject
{
    public abstract void CustomParser();
}