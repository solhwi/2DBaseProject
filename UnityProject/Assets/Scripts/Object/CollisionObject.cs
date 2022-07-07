using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using System;

/// <summary>
/// 물리 충돌이 가능한 충돌체에게 컴포넌트 그대로, 혹은 상속하여 적용합니다.
/// 콜라이더의 외형은 스스로 부착해야 합니다.
/// </summary>

[RequireComponent(typeof(Collider))]
public class CollisionObject : MonoBehaviour
{
    private ENUM_LAYER_TYPE layerType = ENUM_LAYER_TYPE.Default;
    public ENUM_LAYER_TYPE LayerType
    {
        get { return layerType; }
        protected set
        {
            layerType = value;
            CustomPhysics.SetLayer(this, layerType);
        }
    }

    public ENUM_TAG_TYPE tagType = ENUM_TAG_TYPE.Untagged;
    public ENUM_COLLIDER_TYPE colliderType;
    public ENUM_RIGIDBODY_TYPE rigidbodyType = ENUM_RIGIDBODY_TYPE.Static;

    public Transform Tr
    {
        get;
        private set;
    }

    public Collider2D Col
    {
        get;
        private set;
    }

    public Rigidbody2D Rigid
    {
        get;
        private set;
    }

    public event Action<Collision2D> onCollisionEnter = null;
    public event Action<Collision2D> onCollisionStay = null;
    public event Action<Collision2D> onCollisionExit = null;

    public event Action<Collider2D> onTriggerEnter = null;
    public event Action<Collider2D> onTriggerStay = null;
    public event Action<Collider2D> onTriggerExit = null;

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

    public virtual void Init(ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, ENUM_RIGIDBODY_TYPE rigidbodyType, bool isTrigger = true)
    {
        this.layerType = layerType;
        this.tagType = tagType;
        this.rigidbodyType = rigidbodyType;

        Init();

        Col.isTrigger = isTrigger;
    }

    private void Init()
    {
        Tr = transform;
        Col = GetCollider();
        Rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
    }

    public Collider2D GetCollider()
    {
        var col = GetComponent<Collider2D>();

        if (col is BoxCollider2D)
        {
            colliderType = ENUM_COLLIDER_TYPE.Box;
        }
        else if (col is CapsuleCollider2D)
        {
            colliderType = ENUM_COLLIDER_TYPE.Capsule;
        }
        else if (col is PolygonCollider2D)
        {
            colliderType = ENUM_COLLIDER_TYPE.Polygon;
        }
        else if (col is EdgeCollider2D)
        {
            colliderType = ENUM_COLLIDER_TYPE.Edge;
        }

        return col;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter?.Invoke(collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        onCollisionStay?.Invoke(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        onCollisionExit?.Invoke(collision);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter?.Invoke(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        onTriggerStay?.Invoke(other);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExit?.Invoke(other);
    }
}
