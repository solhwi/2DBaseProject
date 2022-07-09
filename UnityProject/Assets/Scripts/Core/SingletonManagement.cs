using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SingletonManagement
{
    [SerializeField] private static GameMgr rootMgr = null;
    [SerializeField] private static Dictionary<Type, Singleton> AliveSingletonsTypeDictionary = new Dictionary<Type, Singleton>();

    private static void InitManagement()
    {
        rootMgr = GameMgr.Instance;
        MonoBehaviour.DontDestroyOnLoad(rootMgr.gameObject);
    }

    public static void RegisterManager(Singleton obj)
    {
        if(rootMgr == null) InitManagement();
        
        if(obj != null)
        {
            obj.transform.SetParent(rootMgr.transform);
            AliveSingletonsTypeDictionary.Add(obj.GetType(), obj);
        }
    }

    public static void UnregisterManager<T>()
    {
        var type = typeof(T);

        AliveSingletonsTypeDictionary[type] = null;
        AliveSingletonsTypeDictionary.Remove(type);
    }


#if UNITY_EDITOR
    [MenuItem("Solhwi/살아있는 싱글톤 갯수 확인하기")]
    public static void FindAliveSingletonsType()
    {
        foreach(var pair in AliveSingletonsTypeDictionary)
        {
            DebugUtil.Log(pair.Key.ToString());
        }
    }
#endif

}
