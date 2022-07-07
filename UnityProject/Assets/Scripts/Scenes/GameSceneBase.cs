using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 씬의 Awake는 입장할 때 반드시 최초로 호출되어야한다.
/// </summary>

public abstract class GameSceneBase : MonoBehaviour
{
    public GameScene currScene
    {
        get
        {
            Type t = this.GetType();
            return ReflectionUtil.ConvertToEnum<GameScene>(t);
        }
    }

    public virtual bool IsSceneLoaded
    {
        get
        {
            return false;
        }
    }

    public virtual bool IsSceneEnd
    {
        get
        {
            return false;
        }
    }

    protected abstract void OnEnable();

    protected abstract IEnumerator Start();

    protected abstract void OnDisable();
}
