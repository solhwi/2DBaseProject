using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMgr : Singleton<UIMgr>
{
    [SerializeField] private UICamera cam;

    private Dictionary<UILayer, UIGroup> UIGroupDictionary = new Dictionary<UILayer, UIGroup>();

    private CachingPool<UIWindow> pool = new CachingPool<UIWindow>();
    private Dictionary<Type, Action> loadingUIActions = new Dictionary<Type, Action>();

    #region Init

    protected override void Init()
    {
        InitCamera();
        InitUIGroup();
        InitEventSystem();
    }

    private void InitCamera()
    {
        GameObject g = new GameObject();
        cam = g.AddComponent<UICamera>();
        cam.MakeUICamera(transform);

#if UNITY_EDITOR
        g.name = "UICamera";
#endif
    }

    private void InitUIGroup()
    {
        for(int i = (int)UILayer.BOTTOM_MOST; i <= (int)UILayer.TOPMOST; i++)
        {
            UILayer currLayer = (UILayer)i;

            GameObject g = new GameObject();

            UICanvas uiCanvas = g.AddComponent<UICanvas>();
            uiCanvas.MakeUICanvas(cam);

            UIGroup uiGroup = new UIGroup(uiCanvas, currLayer);

            UIGroupDictionary.Add(currLayer, uiGroup);

#if UNITY_EDITOR
            g.name = $"{currLayer.ToString()}";
#endif
        }
    }

    private void InitEventSystem()
    {
        GameObject g = new GameObject("EventSystem");
        g.transform.SetParent(transform);

        var eventSystemComponent = g.AddComponent<EventSystem>();
        eventSystemComponent.firstSelectedGameObject = null;
        eventSystemComponent.sendNavigationEvents = true;
        eventSystemComponent.pixelDragThreshold = 10;

        var eventInputModule = g.AddComponent<StandaloneInputModule>();
        eventInputModule.horizontalAxis = "Horizontal";
        eventInputModule.verticalAxis = "Vertical";
        eventInputModule.submitButton = "Submit";
        eventInputModule.cancelButton = "Cancel";
        eventInputModule.inputActionsPerSecond = 10;
        eventInputModule.repeatDelay = 0.5f;
    }

    #endregion

    #region Load, Open, Close, Get

    private void LoadUI<T>(Action<UIWindow> OnCompleted = null) where T : UIWindow
    {
        var windowType = typeof(T);

        if (!loadingUIActions.ContainsKey(windowType)) // 현재 생성 중이 아니라면
        {
            loadingUIActions.Add(windowType, null);

            ResourceMgr.Instance.InstantiateUI<T>((window) =>
            {
                pool.PushObject(window);

                window.OnInit();
                OnCompleted?.Invoke(window);

                loadingUIActions[windowType]?.Invoke();
                loadingUIActions[windowType] = null;

                loadingUIActions.Remove(windowType);
            });
        }
        else
        {
            loadingUIActions[windowType] += () =>
            {
                var window = pool.GetObject(windowType);
                OnCompleted?.Invoke(window);
            };
        }
    }

    public T GetUI<T>() where T : UIWindow
    {
        Type windowType = typeof(T);
        var obj = pool.GetObject(windowType);

        if(obj != null)
            return obj as T;

        return null;
    }

    public void OpenUI<T>(UIParam param = null) where T : UIWindow
    {
        T window = GetUI<T>();

        if(window != null)
        {
            if (window.IsOpen)
                return;

            window.Open(param);
            window.OnOpen();
        }
        else
        {
            LoadUI<T>((window) =>
            {
                window.Open(param);
                window.OnOpen();
            });
        }
    }

    public void CloseUI<T>() where T : UIWindow
    {
        T window = GetUI<T>();

        if (window != null)
        {
            if (!window.IsOpen)
                return;

            window.Close();
            window.OnClose();
        }
        else
        {
            LoadUI<T>((window) =>
            {
                window.Close();
                window.OnClose();
            });
        }
    }

    #endregion

    public UIGroup FindUIGroup(UILayer layer)
    {
        return UIGroupDictionary?[layer];
    }
}
