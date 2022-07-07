using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    private Camera cam;

    private readonly Vector3 DefaultCameraPos = new Vector3(0, 0, -10); 

    public void MakeUICamera(Transform parent)
    {
        if(cam == null)
        {
            cam = gameObject.AddComponent<Camera>();
        }

        transform.SetParent(parent); // transform은 모든 MonoBehaviour의 컴포넌트임

        SetUICamera();
    }

    private void SetUICamera()
    {
        cam.clearFlags = CameraClearFlags.Depth;
        cam.transform.position = DefaultCameraPos;
        cam.depth = (int)SDDefine.CameraDepth.UI;
        cam.cullingMask = 1 << LayerMask.NameToLayer("UI");
        cam.orthographic = true;
    }

    public Camera GetCamera()
    {
        return cam;
    }
}
