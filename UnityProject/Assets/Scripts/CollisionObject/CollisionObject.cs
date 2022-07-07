using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDDefine;

/// <summary>
/// 물리 충돌이 가능한 충돌체에게 컴포넌트 그대로, 혹은 상속하여 적용
/// </summary>

[RequireComponent(typeof(Collider))]
public class CollisionObject : MonoBehaviour
{
    [SerializeField] private ENUM_LAYER_TYPE layerType = ENUM_LAYER_TYPE.Default;
    public ENUM_LAYER_TYPE LayerType
    {
        get { return layerType; }
        protected set
        {
            layerType = value;
            SDPhysics.SetLayer(this, layerType);
        }
    }

    public ENUM_TAG_TYPE tagType = ENUM_TAG_TYPE.Untagged;
    public ENUM_COLLIDER_TYPE colliderType;

    public int objGuid;

    public Transform Tr
    {
        get;
        protected set;
    }

    public Vector3 Position
    {
        get
        {
            return Tr.position;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return Tr.rotation;
        }
    }
    
    public Vector3 RotationAngle
    {
        get
        {
            return Tr.rotation.eulerAngles;
        }
    }

    public Collider Col
    {
        get;
        protected set;
    }

    protected virtual void Reset()
    {
        
    }

    public void ReplaceObject(Vector3 pos, Vector3 rot)
    {
        ReplaceObject(pos, rot, Vector3.one);
    }

    public void ReplaceObject(Vector3 pos, Vector3 rot, Vector3 scale)
    {
        ReplaceObject(pos, new Quaternion(rot.x, rot.y, rot.z, 1), scale);
    }

    public void ReplaceObject(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (Tr == null)
            Tr = transform;

        Tr.SetPositionAndRotation(pos, rot);
        Tr.localScale = scale;
    }

    public virtual void Init()
    {
        Tr = transform;
        Col = GetCollider();
    }

    public Collider GetCollider()
    {
        var col = GetComponent<Collider>();

        if (col is BoxCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Box;
        }
        else if (col is CapsuleCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Capsule;
        }
        else if (col is MeshCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Mesh;
        }
        else if (col is SphereCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Sphere;
        }
        else if (col is TerrainCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Terrain;
        }
        else if (col is WheelCollider)
        {
            colliderType = ENUM_COLLIDER_TYPE.Wheel;
        }

        return col;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

    }

    protected virtual void OnCollisionStay(Collision collision)
    {

    }

    protected virtual void OnCollisionExit(Collision collision)
    {

    }

    protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }
}
