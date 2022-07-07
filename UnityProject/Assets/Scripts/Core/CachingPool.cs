using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간이 지나면 메모리에서 해제시키는 풀
/// 씬 위에 생존하는 MonoBehaviour만 캐싱하고, 해제할 수 있음
/// </summary>
/// <typeparam name="T">Pool의 내용으로 사용되는 인스턴스</typeparam>

[System.Serializable]
public class CachingPool<T> where T : MonoBehaviour
{
    public class InstanceLifeTimePair
    {
        public T instance;
        public float lifeTime;

        public InstanceLifeTimePair(T instance, float lifeTime = 0.0f) => this.instance = instance;
        public void Reset() => lifeTime = 0.0f;
    }

    Dictionary<Type, InstanceLifeTimePair> pool = new Dictionary<Type, InstanceLifeTimePair>();
    private const float MAX_LIFE_TIME = 300.0f;

    protected void Update()
    {
        foreach(var p in pool)
        {
            var pair = p.Value;
            if (pair == null ||
                pair.instance == null) 
                return;

            if (pair.instance.isActiveAndEnabled)
            {
                pair.Reset();
            }
            else
            {
                pair.lifeTime += Time.deltaTime;

                if (pair.lifeTime > MAX_LIFE_TIME)
                {
                    PopObject(p.Key);
                }
            }      
        }
    }

    public T GetObject(Type type)
    {
        InstanceLifeTimePair obj;

        if(pool.TryGetValue(type, out obj))
        {
            obj.Reset();
            return obj.instance;
        }

        return null;
    }

    public void PushObject(T obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var pair = new InstanceLifeTimePair(obj);

        if (pool.ContainsKey(type))
        {
            pool[type] = pair;
        }
        else
        {
            pool.Add(type, pair);
        }
    }

    public void PopObject<TInstance>() where TInstance : T 
    {
        Type type = typeof(TInstance);

        if(pool.ContainsKey(type))
        {
            MonoBehaviour.Destroy(pool[type].instance);

            pool[type].instance = null;
            pool.Remove(type);
        }
    }

    private void PopObject(Type type)
    {
        if (pool.ContainsKey(type))
        {
            MonoBehaviour.Destroy(pool[type].instance);

            pool[type].instance = null;
            pool.Remove(type);
        }
    }

    public void Clear()
    {
        pool.Clear();
    }
}
