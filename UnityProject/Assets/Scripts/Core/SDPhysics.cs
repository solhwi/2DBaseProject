using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDDefine;
using System.Linq;

public static class SDPhysics
{
    public static void SetLayer(CollisionObject collider, ENUM_LAYER_TYPE layerType)
    {
        collider.gameObject.layer = (int)layerType;
    }

    public static int GetLayerMaskWithoutLayerType(ENUM_LAYER_TYPE layerType)
    {
        return (-1) - (1 << (int)layerType);
    }

    public static int GetLayerMask(ENUM_LAYER_TYPE[] layerTypes)
    {
        if (layerTypes == null)
            return 0;

        string[] layerNames = layerTypes.Select(layer => layer.ToString()).ToArray();
        return LayerMask.GetMask(layerNames);
    }

    public static int GetLayerMask(ENUM_LAYER_TYPE layerType)
    {
        return LayerMask.GetMask(layerType.ToString());
    }

    private static void DrawRay(Vector3 origin, Vector3 dir, Color color)
    {
#if UNITY_EDITOR
        Debug.DrawRay(origin, dir, color, 0.5f);
#endif
    }

    public static bool RaycastWithoutLayerType(Ray ray, out RaycastHit hit, float maxDistance, ENUM_LAYER_TYPE layerType, bool isDrawRay = true)
    {
        if(isDrawRay)
            DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

        int layerMask = GetLayerMaskWithoutLayerType(layerType);
        bool layerCheck = Physics.Raycast(ray, out hit, maxDistance, layerMask);

        return layerCheck;
    }

    public static bool RaycastWithoutLayerType(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, ENUM_LAYER_TYPE layerType, bool isDrawRay = true)
    {
        if (isDrawRay)
            DrawRay(origin, direction * maxDistance, Color.green);

        int layerMask = GetLayerMaskWithoutLayerType(layerType);
        bool layerCheck = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        return layerCheck;
    }

    public static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance, ENUM_LAYER_TYPE layerType, bool isDrawRay = true)
    {
        if (isDrawRay)
            DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

        int layerMask = GetLayerMask(layerType);
        bool layerCheck = Physics.Raycast(ray, out hit, maxDistance, layerMask);

        return layerCheck;
    }

    public static bool Raycast(Ray ray, out CollisionObject obj, float maxDistance, ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, bool isDrawRay = true)
    {
        obj = null;
        RaycastHit hit;

        if (isDrawRay)
            DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

        int layerMask = GetLayerMask(layerType);
        bool layerCheck = Physics.Raycast(ray, out hit, maxDistance, layerMask);

        if (!layerCheck)
            return false;

        obj = hit.collider.GetComponent<CollisionObject>();

        if (obj != null)
            return obj.tagType == tagType;

        return false;
    }

    public static bool Raycast(Vector3 origin, Vector3 direction, out CollisionObject obj, float maxDistance, ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, bool isDrawRay = true)
    {
        obj = null;
        RaycastHit hit;

        if (isDrawRay)
            DrawRay(origin, direction * maxDistance, Color.blue);

        int layerMask = GetLayerMask(layerType);
        bool layerCheck = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        if (!layerCheck)
            return false;

        obj = hit.collider.GetComponent<CollisionObject>();

        if(obj != null)
            return obj.tagType == tagType;

        return false;
    }
}
