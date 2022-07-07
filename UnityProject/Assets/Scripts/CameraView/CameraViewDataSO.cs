using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomScriptParser("CameraViewData.asset")]
public class CameraViewDataSO : CustomScriptParser
{
    public CameraViewData cameraViewData;

    public override void CustomParser()
    {
        if (cameraViewData == null)
            return;

    }

    public CameraViewData GetCameraViewData()
    {
        return cameraViewData;
    }
}
