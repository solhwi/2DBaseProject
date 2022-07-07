using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ScriptTable : Singleton<ScriptTable>
{
    Dictionary<Type, ScriptParser> tableDictionary = new Dictionary<Type, ScriptParser>();
    Dictionary<Type, CustomScriptParser> customTableDictionary = new Dictionary<Type, CustomScriptParser>();
    
    private int notLoadedTableCount = int.MaxValue;

    public override bool IsLoadDone
    {
        get { return notLoadedTableCount == 0; }
    }

    protected override void Init()
    {
        SetTable();
    }

    public void SetTable()
    {
        tableDictionary.Clear();
        customTableDictionary.Clear();

        var csharp = Assembly.GetAssembly(typeof(ScriptParser));

        var parsers = csharp.GetTypes().
            Where(type => type.IsSubclassOf(typeof(ScriptParser)))
            .ToArray();

        var customParsers = csharp.GetTypes().
            Where(type => type.IsSubclassOf(typeof(CustomScriptParser)))
            .ToArray();

        notLoadedTableCount = parsers.Length + customParsers.Length;

        foreach (var parser in parsers)
        {
            ResourceMgr.Instance.LoadByType<ScriptParser>(parser, () => { notLoadedTableCount--; }, PushTable);
        }

        foreach (var parser in customParsers)
        {
            ResourceMgr.Instance.LoadByType<CustomScriptParser>(parser, () => { notLoadedTableCount--; }, PushCustomTable);
        }
    }

    private void PushTable<T>(T table) where T : ScriptParser
    {
        Type tableType = table.GetType();

        if (tableDictionary.ContainsKey(tableType))
        {
            tableDictionary[tableType] = table;
        }
        else
        {
            tableDictionary.Add(tableType, table);
        }
    }

    private void PushCustomTable<T>(T table) where T : CustomScriptParser
    {
        Type tableType = table.GetType();

        if (customTableDictionary.ContainsKey(tableType))
        {
            customTableDictionary[tableType] = table;
        }
        else
        {
            customTableDictionary.Add(tableType, table);
        }
    }

    public T GetTable<T>() where T : ScriptParser
    {
        var type = typeof(T);

        if(tableDictionary.ContainsKey(type))
        {
            return tableDictionary[typeof(T)] as T;
        }

        return null;
    }

    public T GetCustomTable<T>() where T : CustomScriptParser
    {
        var type = typeof(T);

        if (customTableDictionary.ContainsKey(type))
        {
            return customTableDictionary[typeof(T)] as T;
        }

        return null;
    }

}
