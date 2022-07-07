using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : ActiveObject
{
    public override void Init(ENUM_LAYER_TYPE layerType, ENUM_TAG_TYPE tagType, ENUM_RIGIDBODY_TYPE rigidbodyType, bool isTrigger = true)
    {
        base.Init(layerType, tagType, rigidbodyType, isTrigger);
    }
}
