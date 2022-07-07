using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : CollisionObject
{
    Animator anim;

    public override void Init(ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, ENUM_RIGIDBODY_TYPE rigidbodyType)
    {
        base.Init(layerType, tagType, rigidbodyType);
    }
}
