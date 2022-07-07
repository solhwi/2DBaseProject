using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : UIWindow
{
    GameObject backgroundObj = null;
    Button btn = null;

    public override void Open(UIParam param = null)
    {
        base.Open();

        if(backgroundObj == null)
            backgroundObj = new GameObject("Background Panel");

        backgroundObj.transform.SetParent(transform);
        backgroundObj.transform.SetAsFirstSibling();

        RectTransform rect = backgroundObj.GetOrAddComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);

        rect.anchoredPosition3D = new Vector3(0, 0, 0);
        rect.localScale = new Vector3(1, 1, 1);

        Image image = backgroundObj.GetOrAddComponent<Image>();
        image.color = Color.clear;
        image.raycastTarget = true;

        btn = backgroundObj.GetOrAddComponent<Button>();
        btn.onClick.AddListener(Close);
    }

    public override void Close()
    {
        btn.onClick.RemoveAllListeners();
        base.Close();
    }
}
