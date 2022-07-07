using System;
using System.Collections.Generic;


public partial class MapPathTable : ScriptParser
{
    public string GetMapPath(int idx)
    {
        if (MapPathPairDictionary.ContainsKey(idx))
            return MapPathPairDictionary[idx].MapPath;

        return string.Empty;
    }
}
