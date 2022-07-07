using SDDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum ENUM_KEYBOARD_INPUT
{
    MoveSet,
    Shift,
    Space,
    R,
    G,
    Tab,

    MAX
}

public enum ENUM_MOUSE_INPUT
{
    MouseLeft,
    MouseMiddle,
    MouseRight,
    MousePosition,
    MouseDelta,

    MAX
}

public sealed class InputMgr : Singleton<InputMgr>
{
    private bool isLoadDone = false;
    public override bool IsLoadDone
    {
        get
        {
            return isLoadDone;
        }
    }

    public enum InputType
    {
        Keyboard = 0,
        Mouse = 1,

        MAX
    }
    private PlayerInputActions playerInputActions;

    public InputActionAsset Asset
    {
        get
        {
            if (playerInputActions == null)
                playerInputActions = new PlayerInputActions();

            return playerInputActions.asset;
        }
    }

    private InputActionMap[] actionMaps = new InputActionMap[(int)InputType.MAX];

    public static Vector2 MouseScreenPos
    {
        get
        {
            float x = Mouse.current.position.x.ReadValue();
            float y = Mouse.current.position.y.ReadValue();

            return new Vector2(x, y);
        }
    }

    public static Vector2 MouseScreenDelta
    {
        get
        {
            float x = Mouse.current.delta.x.ReadValue();
            float y = Mouse.current.delta.y.ReadValue();

            return new Vector2(x, y);
        }
    }

    public static Vector2 MoveSetVector2D
    {
        get
        {
            return Instance.actionMaps[(int)InputType.Keyboard].FindAction(ENUM_KEYBOARD_INPUT.MoveSet.ToString()).ReadValue<Vector2>();
        }
    }

    protected override void OnAwakeInstance() 
    {
        if(playerInputActions == null)
            playerInputActions = new PlayerInputActions();

        playerInputActions.Enable();

        actionMaps[(int)InputType.Keyboard] = playerInputActions.KeyBoard;
        actionMaps[(int)InputType.Mouse] = playerInputActions.Mouse;

        isLoadDone = true;
    }

    public void RegisterKeyboardAction(ENUM_KEYBOARD_INPUT keyboardType, Action<InputAction.CallbackContext> OnStarted = null, Action<InputAction.CallbackContext> OnPerformed = null, Action<InputAction.CallbackContext> OnCanceled = null)
    {
        if (keyboardType == ENUM_KEYBOARD_INPUT.MAX) 
            return;

        string actionKey = keyboardType.ToString();
        int type = (int)InputType.Keyboard;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnStarted != null)
            action.started += OnStarted;

        if (OnPerformed != null)
            action.performed += OnPerformed;

        if (OnCanceled != null)
            action.canceled += OnCanceled;

        action.Enable();
    }

    public void RegisterMouseAction(ENUM_MOUSE_INPUT mouseType, Action<InputAction.CallbackContext> OnStarted = null, Action<InputAction.CallbackContext> OnPerformed = null, Action<InputAction.CallbackContext> OnCanceled = null)
    {
        if (mouseType == ENUM_MOUSE_INPUT.MAX)
            return;

        string actionKey = mouseType.ToString();
        int type = (int)InputType.Mouse;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnStarted != null)
            action.started += OnStarted;

        if (OnPerformed != null)
            action.performed += OnPerformed;

        if (OnCanceled != null)
            action.canceled += OnCanceled;

        action.Enable();
    }

    public void RegisterMouseAction(ENUM_MOUSE_INPUT mouseType, Action<InputAction.CallbackContext> OnCompleted)
    {
        if (mouseType == ENUM_MOUSE_INPUT.MAX)
            return;

        string actionKey = mouseType.ToString();
        int type = (int)InputType.Mouse;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnCompleted != null)
        {
            action.started += OnCompleted;
            action.performed += OnCompleted;
            action.canceled += OnCompleted;
        }

        action.Enable();
    }

    public void RegisterKeyboardAction(ENUM_KEYBOARD_INPUT keyboardType, Action<InputAction.CallbackContext> OnCompleted)
    {
        if (keyboardType == ENUM_KEYBOARD_INPUT.MAX)
            return;

        string actionKey = keyboardType.ToString();
        int type = (int)InputType.Keyboard;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnCompleted != null)
        {
            action.started += OnCompleted;
            action.performed += OnCompleted;
            action.canceled += OnCompleted;
        }

        action.Enable();
    }

    public void UnRegisterKeyboardAction(ENUM_KEYBOARD_INPUT keyboardType, Action<InputAction.CallbackContext> OnStarted = null, Action<InputAction.CallbackContext> OnPerformed = null, Action<InputAction.CallbackContext> OnCanceled = null)
    {
        if (keyboardType == ENUM_KEYBOARD_INPUT.MAX)
            return;

        string actionKey = keyboardType.ToString();
        int type = (int)InputType.Keyboard;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnStarted != null)
            action.started -= OnStarted;

        if (OnPerformed != null)
            action.performed -= OnPerformed;

        if (OnCanceled != null)
            action.canceled -= OnCanceled;
    }

    public void UnRegisterMouseAction(ENUM_MOUSE_INPUT mouseType, Action<InputAction.CallbackContext> OnStarted = null, Action<InputAction.CallbackContext> OnPerformed = null, Action<InputAction.CallbackContext> OnCanceled = null)
    {
        if (mouseType == ENUM_MOUSE_INPUT.MAX)
            return;

        string actionKey = mouseType.ToString();
        int type = (int)InputType.Mouse;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnStarted != null)
            action.started -= OnStarted;

        if (OnPerformed != null)
            action.performed -= OnPerformed;

        if (OnCanceled != null)
            action.canceled -= OnCanceled;
    }

    public void UnRegisterKeyboardAction(ENUM_KEYBOARD_INPUT keyboardType, Action<InputAction.CallbackContext> OnCompleted)
    {
        if (keyboardType == ENUM_KEYBOARD_INPUT.MAX)
            return;

        string actionKey = keyboardType.ToString();
        int type = (int)InputType.Keyboard;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnCompleted != null)
        {
            action.started -= OnCompleted;
            action.performed -= OnCompleted;
            action.canceled -= OnCompleted;
        }
    }

    public void UnRegisterMouseAction(ENUM_MOUSE_INPUT mouseType, Action<InputAction.CallbackContext> OnCompleted)
    {
        if (mouseType == ENUM_MOUSE_INPUT.MAX)
            return;

        string actionKey = mouseType.ToString();
        int type = (int)InputType.Mouse;

        var action = actionMaps[type].FindAction(actionKey);

        if (OnCompleted != null)
        {
            action.started -= OnCompleted;
            action.performed -= OnCompleted;
            action.canceled -= OnCompleted;
        }
    }
}
