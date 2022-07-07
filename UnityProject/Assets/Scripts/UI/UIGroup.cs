using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UILayer
{
    BOTTOM_MOST = 0,
    BOTTOM = 1,
    MIDDLE = 2,
    TOP = 3,
    TOPMOST = 4
}

public class UIGroup
{
    UICanvas groupCanvas;

    Stack<UIWindow> windowStack = new Stack<UIWindow>();
    private const int UIStackOffset = 10;

    private int currTopSortingOrder = -1;

    public UIGroup(UICanvas canvas, UILayer layer)
    {
        groupCanvas = canvas;

        if(groupCanvas != null)
        {
            groupCanvas.SortingOrder(layer);
        }

        currTopSortingOrder = groupCanvas.sortingOrder;
    }

    public void RegisterWindow(UIWindow window)
    {
        if(groupCanvas == null || window == null)
        {
            Debug.LogError("아직 캔버스가 초기화되지 않은 상태이거나, 윈도우가 null입니다.");
            return;
        }

        currTopSortingOrder += UIStackOffset;

        window.transform.SetParent(groupCanvas.transform, false);
        window.SetWindow(currTopSortingOrder);

        windowStack.Push(window);
    }

    public void UnRegisterWindow(UIWindow window)
    {
        currTopSortingOrder -= UIStackOffset;

        if (windowStack.Contains(window) && window == windowStack.Peek())
        {
            windowStack.Pop();
        }
    }
}
