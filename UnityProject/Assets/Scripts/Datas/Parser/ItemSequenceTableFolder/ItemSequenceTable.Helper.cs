using SDDefine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ItemSequenceTable
{
    public ItemSequenceInfo GetItemInfo(int ddid)
    {
        if(ItemSequenceInfoDictionary.ContainsKey(ddid))
        {
            return ItemSequenceInfoDictionary[ddid];
        }

        return null;
    }
}