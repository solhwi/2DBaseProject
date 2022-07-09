using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public enum ResourceType
{
    Material = 1,
    Prefab = 2,
    Scene = 3,
    Animation = 4,
    UnityAsset = 5
}

[System.AttributeUsage(AttributeTargets.Class)]
public class ResourceAttribute : Attribute
{
    private string resourcePath;

    public ResourceAttribute(string resourcePath)
    {
        this.resourcePath = resourcePath;
    }

    public string GetPath()
    {
        return resourcePath;
    }
}

public class ResourceMgr : Singleton<ResourceMgr>
{
#if !UNITY_EDITOR
    private int loadedAssetCount = 0;
#endif
    public override bool IsLoadDone
    {
        get
        {
#if !UNITY_EDITOR
            return loadedAssetCount == Enum.GetValues(typeof(AssetType)).Length;
#else
            return true;
#endif
        }
    }

    protected override void Initialize()
    {
#if !UNITY_EDITOR
        foreach(var label in Enum.GetValues(typeof(AssetType)))
        {
            DownLoadAsset(label.ToString());
        }
#endif
    }

#if !UNITY_EDITOR
    private void DownLoadAsset(string label)
    {
        Addressables.DownloadDependenciesAsync(label).Completed += (op) =>
            {
                if(op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
                {
                    Addressables.Release(op);
                    loadedAssetCount++;
                }
            };
    }
#endif

    public AsyncOperationHandle<GameObject> InstantiateUI<T>(Action<T> PushObj = null) where T : UIWindow
    {
        string path = AttributeUtil.GetResourcePath<T>();

        if (path == null)
            return new AsyncOperationHandle<GameObject>();

        return Instantiate<T>(path, null, PushObj);
    }

    public AsyncOperationHandle<GameObject> Instantiate<T>(Action OnCompleted = null, Action<T> PushObj = null) where T : MonoBehaviour
    {
        string path = AttributeUtil.GetResourcePath<T>();

        if (path == null) 
            return new AsyncOperationHandle<GameObject>();

        return Instantiate<T>(path, OnCompleted, PushObj);
    }

    private AsyncOperationHandle<GameObject> Instantiate<T>(string path, Action OnCompleted = null, Action<T> ResultCallBack = null) where T : MonoBehaviour
    {
        var async = Addressables.InstantiateAsync(path);

        async.Completed += (op) =>
        {
            if (op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
            {
                T monoObj = op.Result.GetComponent<T>();

                ResultCallBack?.Invoke(monoObj);
                OnCompleted?.Invoke();
            }
        };

        return async;
    }

    public AsyncOperationHandle<GameObject> Instantiate(AssetReference gameObject, Transform parent, Action<GameObject> OnCompleted = null)
    {
        if (gameObject == null)
            return new AsyncOperationHandle<GameObject>();

        var async = gameObject.InstantiateAsync(parent);

        async.Completed += (op) =>
        {
            if (op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
            {
                OnCompleted?.Invoke(op.Result);
            }
        };

        return async;
    }

    public UnityEngine.Object InstantiateSync(UnityEngine.Object obj)
    {
        if (obj == null)
            return new UnityEngine.Object();

        return Instantiate(obj);
    }

    public void LoadMap(string mapPath, Action OnLoaded)
    {
        this.StartUpdateCoroutine(CoLoadMap(mapPath, OnLoaded));
    }
    
    private IEnumerator CoLoadMap(string mapPath, Action OnLoaded)
    {
        if (mapPath == null ||
            mapPath == string.Empty)
            yield break;

        var async = Addressables.LoadSceneAsync(mapPath, LoadSceneMode.Additive);
        while (!async.IsDone)
            yield return null;

        OnLoaded?.Invoke();
        Addressables.UnloadSceneAsync(async);
    }

    public AsyncOperationHandle<T> LoadByAttribute<T>(Action OnLoaded = null, Action<T> PushObj = null) where T : UnityEngine.Object
    {
        string path = AttributeUtil.GetResourcePath<T>();

        if (path == null) 
            return new AsyncOperationHandle<T>() { };

        return Load<T>(path, OnLoaded, PushObj);
    }

    public AsyncOperationHandle<T> LoadByType<T>(Type type, Action OnLoaded = null, Action<T> PushObj = null) where T : UnityEngine.Object
    {
        string path = AttributeUtil.GetResourcePath(type);

        if (path == null)
            return new AsyncOperationHandle<T>() { };

        return Load<T>(path, OnLoaded, PushObj);
    }

    public AsyncOperationHandle<T> LoadByPath<T>(string path, Action OnLoaded = null, Action<T> PushObj = null) where T : UnityEngine.Object
    {
        if (path == null ||
            path == string.Empty) 
            return new AsyncOperationHandle<T>() { };

        return Load<T>(path, OnLoaded, PushObj);
    }

    private AsyncOperationHandle<T> Load<T>(string path, Action OnCompleted = null, Action<T> ResultCallBack = null) where T : UnityEngine.Object
    {
        var async = Addressables.LoadAssetAsync<T>(path);

        async.Completed += (op) =>
        {
            if (op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
            {
                OnCompleted?.Invoke();
                ResultCallBack?.Invoke(op.Result);
            }
        };

        return async;
    }
}