using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(Canvas))]
public class UIElement : MonoBehaviour
{
    private Canvas canvas;
    protected int sortingOrder;
    public bool IsOpen { get; protected set; }

    public void SetWindow(int sortingOrder)
    {
        canvas = GetComponent<Canvas>();
        this.sortingOrder = sortingOrder;
        canvas.sortingOrder = sortingOrder;
    }
}
