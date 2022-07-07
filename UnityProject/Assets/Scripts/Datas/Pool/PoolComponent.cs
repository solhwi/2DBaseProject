using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[ResourceAttribute("Assets/Bundles/Datas/Pool/PoolComponent.asset")]
[CreateAssetMenu(fileName = "PoolComponent", menuName = "Datas/PoolComponent")]
public class PoolComponent : ScriptableObject
{
    [SerializeField] public AssetReferenceGameObject testItem;
}