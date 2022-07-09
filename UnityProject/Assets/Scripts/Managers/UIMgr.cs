using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIMgr : Singleton<UIMgr>, PlayerInputActions.IUIActions
{
    [SerializeField] private UICamera cam;

    private Dictionary<UILayer, UIGroup> UIGroupDictionary = new Dictionary<UILayer, UIGroup>();

    private CachingPool<UIWindow> pool = new CachingPool<UIWindow>();
    private Dictionary<Type, Action> loadingUIActions = new Dictionary<Type, Action>();

    #region Init

    protected override void Initialize()
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
        for (int i = (int)UILayer.BOTTOM_MOST; i <= (int)UILayer.TOPMOST; i++)
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

        var inputModule = g.AddComponent<InputSystemUIInputModule>();
        inputModule.moveRepeatDelay = 0.5f;
        inputModule.moveRepeatRate = 0.1f;
        inputModule.deselectOnBackgroundClick = true;
        inputModule.pointerBehavior = UIPointerBehavior.SingleMouseOrPenButMultiTouchAndTrack;

        var defaultInputActionAsset = InputMgr.Instance.Asset;
        var uiActionMap = defaultInputActionAsset.FindActionMap("UI");

        inputModule.actionsAsset = defaultInputActionAsset;
        inputModule.point = InputActionReference.Create(uiActionMap.FindAction("Point"));
        inputModule.leftClick = InputActionReference.Create(uiActionMap.FindAction("Click"));
        inputModule.middleClick = InputActionReference.Create(uiActionMap.FindAction("MiddleClick"));
        inputModule.rightClick = InputActionReference.Create(uiActionMap.FindAction("RightClick"));
        inputModule.scrollWheel = InputActionReference.Create(uiActionMap.FindAction("ScrollWheel"));
        inputModule.move = InputActionReference.Create(uiActionMap.FindAction("Navigate"));
        inputModule.submit = InputActionReference.Create(uiActionMap.FindAction("Submit"));
        inputModule.cancel = InputActionReference.Create(uiActionMap.FindAction("Cancel"));
        inputModule.trackedDevicePosition = InputActionReference.Create(uiActionMap.FindAction("TrackedDevicePosition"));
        inputModule.trackedDeviceOrientation = InputActionReference.Create(uiActionMap.FindAction("TrackedDeviceOrientation"));
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

        if (obj != null)
            return obj as T;

        return null;
    }

    public void OpenUI<T>(UIParam param = null) where T : UIWindow
    {
        T window = GetUI<T>();

        if (window != null)
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

    public void OnNavigate(InputAction.CallbackContext context)
    {

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {

    }

    public void OnCancel(InputAction.CallbackContext context)
    {

    }

    public void OnPoint(InputAction.CallbackContext context)
    {

    }

    public void OnClick(InputAction.CallbackContext context)
    {

    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {

    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

    }

    public void OnRightClick(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {

    }
}
