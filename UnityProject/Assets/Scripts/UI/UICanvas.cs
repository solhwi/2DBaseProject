using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    private Canvas canvas = null;
    private CanvasScaler scaler = null;
    private GraphicRaycaster raycaster = null;

    public UILayer layer { get; private set; }


    public int sortingOrder
    {
        private set; get;
    } = -30000;

    public void MakeUICanvas(UICamera parent)
    {
        if (parent == null)
        {
            Debug.LogError("아직 카메라가 생성되지 않았습니다.");
            return;
        }

        transform.SetParent(parent.transform);

        MakeCanvas(parent.GetCamera());
        MakeCanvasScaler();
        MakeGraphicRayCaster();
    }

    private void MakeCanvas(Camera cam)
    {
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
    }

    private void MakeCanvasScaler()
    {
        if(scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    private void MakeGraphicRayCaster()
    {
        if(raycaster == null)
        {
            raycaster = gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    public void SortingOrder(UILayer layer)
    {
        this.layer = layer;

        switch(layer)
        {
            case UILayer.BOTTOM_MOST:
                sortingOrder = -30000;
                break;
            case UILayer.BOTTOM:
                sortingOrder = -25000;
                break;
            case UILayer.MIDDLE:
                sortingOrder = -20000;
                break;
            case UILayer.TOP:
                sortingOrder = -15000;
                break;
            case UILayer.TOPMOST:
                sortingOrder = -10000;
                break;
        }

        canvas.sortingOrder = sortingOrder;
    }
}
