using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 문제점 1: Prefab 위치를 옮길 경우 Path를 다시 지정해줘야한다는 점
/// 우선 별다른 방안이 나올 때까지 사용, 프리팹 위치를 옮긴 후 Addressable을 off/on 하면 경로를 얻을 수 있다.
public class WindowAttribute : ResourceAttribute
{
    public WindowAttribute(string path) : base("Assets/Bundles/Prefabs/UI/" + path)
    {

    }
}

public class UIWindow : UIElement
{
    public UILayer TargetUILayer;

    protected virtual void OnEnable()
    {
        UIGroup group = UIMgr.Instance.FindUIGroup(TargetUILayer);
        group.RegisterWindow(this);
    }

    protected virtual void OnDisable()
    {
        UIGroup group = UIMgr.Instance.FindUIGroup(TargetUILayer);
        group.UnRegisterWindow(this);
    }

    public virtual void Open(UIParam param = null)
    {
        gameObject.SetActive(true);
        IsOpen = true;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }

    public virtual void OnOpen() { } // 후 처리용 함수들
    public virtual void OnClose() { }
    public virtual void OnInit() { } // 첫 로드 시 호출
}
