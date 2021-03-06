using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 미리 생성해두고 생성과 해제 시기를 조절할 수 있는 풀
/// 씬 위에 생존하는 MonoBehaviour만 해당 풀을 소유할 수 있음
/// </summary>
/// <typeparam name="T"></typeparam>

public class ObjectPool<T> where T : MonoBehaviour
{
    private T owner;

    private AssetReference poolingObj = null;
    private UnityEngine.Object dummyPoolingObj = null; // 시스템적 한계를 보강하기 위한 더미 게임 오브젝트

    private Queue<UnityEngine.Object> poolingQueue = new Queue<UnityEngine.Object>();

    public bool IsLoadDone = false;

    public void Initialize(T owner, AssetReference prefab, int initCount)
    {
        this.owner = owner;
        poolingObj = prefab;

        owner.StartCoroutine(CreateCoroutine(initCount, PushObject));
    }

    private IEnumerator CreateCoroutine(int initCount, Action<UnityEngine.Object> enqueue)
    {
        for (int i = 0; i < initCount; i++)
        {
            var asyncData = ResourceMgr.Instance.Instantiate(poolingObj, owner.transform, enqueue);

            while(!asyncData.IsDone)
                yield return null;

            if (dummyPoolingObj == null)
                dummyPoolingObj = asyncData.Result;
        }

        if (owner is ObjectPoolMgr)
        {
            ObjectPoolMgr.Instance.LoadingPoolCount--;
        }
    }

    private void PushObject(UnityEngine.Object obj)
    {
        poolingQueue.Enqueue(obj);
    }

    public UnityEngine.Object GetObject(bool isActive = true)
    {
        UnityEngine.Object obj = null;

        if (poolingQueue.Count > 0)
        {
            obj = poolingQueue.Dequeue();
        }    
        else
        {
            // 이 경우 어쩔 수 없이 동기 생성해야 함, 상당한 오버헤드가 예상되므로 initCount를 초기에 잘 설정할 것
            obj = ResourceMgr.Instance.InstantiateSync(dummyPoolingObj);
        }

        return obj;
    }

    public void ReturnObject(UnityEngine.Object obj)
    {
        poolingQueue.Enqueue(obj);
    }
}