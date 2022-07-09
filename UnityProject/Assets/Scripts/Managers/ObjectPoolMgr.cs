using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

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

    protected override void Initialize()
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

    public UnityEngine.Object GetPoolComponent(PoolType type)
    {
        if (pools == null ||
            !pools.ContainsKey(type))
            return null;

        return pools[type].GetObject();
    }

    public void ReturnPoolComponent(PoolType type, UnityEngine.Object returnObj)
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
