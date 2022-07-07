using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDDefine;

public enum PoolType
{
    TestItem = 0,
    Max
}


public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
{
    private Dictionary<PoolType, ObjectPool<ObjectPoolMgr>> pools = new Dictionary<PoolType, ObjectPool<ObjectPoolMgr>>();
    
    public int LoadingPoolCount = int.MaxValue;
    public override bool IsLoadDone
    {
        get
        {
            return LoadingPoolCount == 0;
        }
    }

    public void InitPool()
    {
        for (int i = (int)PoolType.TestItem; i < (int)PoolType.Max; i++)
        {
            pools.Add((PoolType)i, new ObjectPool<ObjectPoolMgr>());
        }

        LoadingPoolCount = (int)PoolType.Max;

        ResourceMgr.Instance.LoadByAttribute<PoolComponent>(PushObj: PushPool);
    }

    private void PushPool(PoolComponent poolComponent)
    {
        if (poolComponent == null) 
            return;

        pools[PoolType.TestItem].Initialize(this, poolComponent.testItem, 3);
    }

    public GameObject GetPoolComponent(PoolType type, bool isActive = true)
    {
        if (pools == null ||
            !pools.ContainsKey(type))
            return null;

        return pools[type].GetObject(isActive);
    }

    public GameObject GetPoolComponent(PoolType type, Vector3 pos ,bool isActive = true)
    {
        if (pools == null ||
            !pools.ContainsKey(type))
            return null;

        var obj = pools[type].GetObject(isActive);
        obj.transform.position = pos;

        return obj;
    }

    public void ReturnPoolComponent(PoolType type, GameObject returnObj)
    {
        if (pools == null ||
           !pools.ContainsKey(type))
        {
            DestroyImmediate(returnObj);
            return;
        }

        pools[type].ReturnObject(returnObj);
    }
}
