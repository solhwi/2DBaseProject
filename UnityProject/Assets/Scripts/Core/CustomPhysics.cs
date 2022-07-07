using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using System.Linq;

public static class CustomPhysics
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

    public static bool Raycast(Vector2 origin, Vector2 direction, out CollisionObject obj, float maxDistance, ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, bool isDrawRay = true)
    {
        obj = null;

        if (isDrawRay)
            DrawRay(origin, direction * maxDistance, Color.blue);

        int layerMask = GetLayerMask(layerType);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

        if (hit.collider == null)
            return false;

        obj = hit.collider.GetComponent<CollisionObject>();

        if(obj != null)
            return obj.tagType == tagType;

        return false;
    }
}
