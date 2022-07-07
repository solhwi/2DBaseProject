using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

namespace Solhwi
{
    public class EnterScene : GameSceneBase
    { 
        [System.Serializable]
        public enum SingletonID // 게임 진입 시 초기화되는 싱글톤, 클래스 이름과 동일하게 enum을 작성해야 합니다.
        {
            ResourceMgr = 0,
            ScriptTable = 1,
            InputMgr = 2,
            Max
        }

        // 화면에 관리되는 매니저를 보여주는 용도의 컬렉션 
        [System.Serializable]
        public class SingletonDictionary : SerializableDictionary<SingletonID, Singleton> { }
        [SerializeField] SingletonDictionary singletonDictionary = new SingletonDictionary();

        public bool IsSceneEnded
        {
            get
            {
                return ScriptTable.Instance.IsLoadDone &&
                        ResourceMgr.Instance.IsLoadDone;
            }
        }

        protected override void OnEnable()
        {
            singletonDictionary = new SingletonDictionary()
            {
                { SingletonID.ResourceMgr, ResourceMgr.Instance},
                { SingletonID.ScriptTable, ScriptTable.Instance},
            };
        }

        protected override IEnumerator Start()
        {
            while (!IsSceneEnded)
            {
                yield return null;
            }
        }

        protected override void OnDisable()
        {
            singletonDictionary = null;
        }
    }

   
}
