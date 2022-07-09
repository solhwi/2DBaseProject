using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Singleton : MonoBehaviour
{
    private void Awake()
    {
        OnAwakeInstance();
    }

    private void Start()
    {
        OnStartInstance();
    }

    private void OnDestroy()
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

    protected abstract void Initialize();
    protected virtual void OnAwakeInstance() { }
    protected virtual void OnStartInstance() { }
    protected virtual void OnReleaseInstance() { }
    public abstract Singleton GetInstance();
}

public abstract class Singleton<T> : Singleton where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            GameObject g = new GameObject();
            instance = g.AddComponent<T>();

            instance.Initialize();

            SingletonManagement.RegisterManager(instance);

#if UNITY_EDITOR_WIN
            g.name = typeof(T).ToString();
#endif
            return instance;
        }
    }

    protected override void OnReleaseInstance()
    {
        SingletonManagement.UnregisterManager<T>();
    }

    public override Singleton GetInstance() { return Instance; }
}
