using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    /// <summary>
    /// 레이어 타입, 에디터 상 레이어 넘버와 ENUM이 동일해야 함
    /// </summary>
    /// 
    [Serializable]
    public enum ENUM_LAYER_TYPE
    {
        /// <summary>
        /// 여기는 Built-In 레이어
        /// </summary>
        /// 
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2, // 레이캐스트 무시
        Environment = 3, // 맵, 오브젝트 등 환경
        Water = 4, // 물 등 특이 장애물
        UI = 5, 

        /// <summary>
        /// 여기부터 User 커스텀 레이어
        /// </summary>
        ///
        Player = 6, // 유저
    }

    /// <summary>
    /// 태그 타입, Unity의 태그는 사용하지 않을 예정
    /// </summary>
    /// 
    [Serializable]
    public enum ENUM_TAG_TYPE
    {
        Untagged = 0,
        PlayerCharacter = 1,
        NPCCharacter = 2,
    }


    /// <summary>
    /// 콜라이더 종류
    /// </summary>

    [Serializable]
    public enum ENUM_COLLIDER_TYPE
    {
        None = -1,
        Box = 0,
        Capsule = 1,
        Polygon = 2,
        Edge = 3,
        Tile = 4,
    }

    [System.Serializable]
    public enum CameraDepth
    {
        Player = 0,
        UI = 1,
    }

    public enum ENUM_RIGIDBODY_TYPE
    {
        None = -1,
        Static = 0,
        Kinematic = 1,
        Dynamic = 2,
    }
}
