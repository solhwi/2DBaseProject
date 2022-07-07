using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Singleton : MonoBehaviour
{
    protected virtual void Awake()
    {
        OnAwakeInstance();
    }

    protected virtual void Start()
    {
        OnStartInstance();
    }

    protected virtual void OnDestroy()
    {
        OnReleaseInstance();
    }

    public virtual bool IsLoadDone
    {
        get
        {
            return false;
        }
    }

    protected abstract void OnAwakeInstance();
    protected abstract void OnStartInstance();
    protected abstract void OnReleaseInstance();
    public abstract Singleton GetInstance();
}

public class Singleton<T> : Singleton where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // 디폴트는 외형이 없는 오브젝트로 초기화
            return instance ?? Init();
        }
    }

    private static T Init()
    {
        GameObject g = new GameObject();
        instance = g.AddComponent<T>();

        SingletonManagement.RegisterManager(instance);

#if UNITY_EDITOR_WIN
        g.name = typeof(T).ToString();
#endif
        return instance;
    }

    protected override void OnReleaseInstance()
    {
        SingletonManagement.UnregisterManager<T>();
    }

    /// <summary>
    /// 주로 내부에 의한 초기화
    /// </summary>
    protected override void OnAwakeInstance() { }

    /// <summary>
    /// 주로 외부 싱글톤에 의한 초기화
    /// </summary>
    protected override void OnStartInstance() { }
    public override Singleton GetInstance() { return Instance; }

}
