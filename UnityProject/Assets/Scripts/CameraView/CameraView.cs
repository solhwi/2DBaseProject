using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class CameraViewData
{
    public float lookingPos_x;
    public float lookingPos_y;
    public float lookingPos_z;

    public float cameraPos_y;
    public float cameraPos_z;
}

public class CameraView : MonoBehaviour
{
    private Camera cam;
    private Transform camArmTr;

    public Transform target;

    Vector3 lookingPos;
    [Range(-15.0f, 15.0f)] public float lookingPos_x;
    [Range(-15.0f, 15.0f)] public float lookingPos_y;
    [Range(-15.0f, 15.0f)] public float lookingPos_z;

    Vector3 cameraPos;
    [Range(-15.0f, 15.0f)] public float cameraPos_y;
    [Range(-15.0f, 15.0f)] public float cameraPos_z;

    private string filePath = "../Common/JsonDatas/CameraViewData/CameraViewData.json";

    public CameraViewDataSO cameraViewDataSO;

    private void Awake()
    {
        camArmTr = transform.GetChild(0).GetComponent<Transform>();
        cam = camArmTr.GetChild(0).GetComponent<Camera>();

        CameraViewData cameraViewData = new CameraViewData();

        string jsonData = File.ReadAllText(filePath);
        cameraViewData = JsonUtility.FromJson<CameraViewData>(jsonData);

        SetPosition(cameraViewData);
        
        lookingPos = new Vector3(lookingPos_x, lookingPos_y, lookingPos_z);
        cameraPos = new Vector3(0, cameraPos_y, cameraPos_z);

        camArmTr.transform.position += target.transform.position + lookingPos;
        cam.transform.position += target.transform.position + cameraPos;
    }

    private void Update()
    {
        lookingPos = new Vector3(lookingPos_x, lookingPos_y, lookingPos_z);
        cameraPos = new Vector3(0, cameraPos_y, cameraPos_z);

        cam.transform.localPosition = cameraPos;

        camArmTr.transform.localPosition = lookingPos;

        cam.transform.LookAt(camArmTr);
    }

    private void SetPosition(CameraViewData _cameraViewData)
    {
        lookingPos_x = _cameraViewData.lookingPos_x;
        lookingPos_y = _cameraViewData.lookingPos_y;
        lookingPos_z = _cameraViewData.lookingPos_z;

        cameraPos_y = _cameraViewData.cameraPos_y;
        cameraPos_z = _cameraViewData.cameraPos_z;
    }
}
