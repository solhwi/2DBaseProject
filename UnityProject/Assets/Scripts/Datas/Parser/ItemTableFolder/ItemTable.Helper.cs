using SDDefine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ItemTable
{
    public ItemInfo GetItemInfo(int ddid)
    {
        if(ItemInfoDictionary.ContainsKey(ddid))
        {
            return ItemInfoDictionary[ddid];
        }

        return null;
    }

    public IReadOnlyCollection<ItemInfo> GetItemInfos()
    {
        return ItemInfoList;
    }    

}





