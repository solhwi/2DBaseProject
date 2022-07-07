using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MicroCoroutineExtensition
{
    public static IMicroCoroutine StartUpdateCoroutine(this MonoBehaviour owner, IEnumerator routine)
    {
        return CoroutineMgr.StartCoroutine(owner, CoroutineType.Update, routine);
    }

    public static IMicroCoroutine StartFixedUpdateCoroutine(this MonoBehaviour owner, IEnumerator routine)
    {
        return CoroutineMgr.StartCoroutine(owner, CoroutineType.FixedUpdate, routine);
    }

    public static IMicroCoroutine StartEndOfFrameCoroutine(this MonoBehaviour owner, IEnumerator routine)
    {
        return CoroutineMgr.StartCoroutine(owner, CoroutineType.EndOfFrame, routine);
    }

    public static void StopCoroutine(this MonoBehaviour owner, IMicroCoroutine coroutine, CoroutineType type = CoroutineType.Update)
    {
        CoroutineMgr.StopCoroutine(type, coroutine);
    }
}

public interface IMicroCoroutine
{

}

public class MicroCoroutine : IMicroCoroutine, IEqualityComparer<MicroCoroutine>
{
    private MonoBehaviour owner;
    private List<IEnumerator> coroutines = new List<IEnumerator>();

    public MicroCoroutine(MonoBehaviour owner)
    {
        this.owner = owner;
    }

    public MicroCoroutine Addroutine(IEnumerator enumerator)
    {
        coroutines.Add(enumerator);

        return this;
    }

    public bool Equals(MicroCoroutine x, MicroCoroutine y)
    {
        return x.owner == y.owner;
    }

    public int GetHashCode(MicroCoroutine obj)
    {
        return owner.GetHashCode();
    }

    public void Run()
    {
        int iter = 0;

        while (coroutines.Count > iter)
        {
            if (!coroutines[iter].MoveNext())
            {
                coroutines.RemoveAt(iter);
                continue;
            }

            iter++;
        }
    }
}

/// <summary>
/// 코루틴 객체의 리턴이 필요없을 때 사용하도록 하는 코루틴 매니저
/// </summary>
/// 

public enum CoroutineType
{
    Update = 0,
    FixedUpdate = 1,
    EndOfFrame = 2
}

public class CoroutineMgr : Singleton<CoroutineMgr>
{
    private Dictionary<CoroutineType, List<MicroCoroutine>> coroutineDictionary = null;

    protected override void OnAwakeInstance()
    {
        coroutineDictionary = new Dictionary<CoroutineType, List<MicroCoroutine>>()
        {
            { CoroutineType.Update, new List<MicroCoroutine>() },
            { CoroutineType.FixedUpdate, new List<MicroCoroutine>() },
            { CoroutineType.EndOfFrame, new List<MicroCoroutine>() },
        };
    }

    protected override void OnStartInstance()
    {
        StartCoroutine(RunUpdateMicroCoroutine());
        StartCoroutine(RunFixedUpdateMicroCoroutine());
        StartCoroutine(RunEndOfFrameMicroCoroutine());
    }

    public static IMicroCoroutine StartCoroutine(MonoBehaviour owner, CoroutineType type, IEnumerator routine)
    {
        if (Instance == null)
            return null;

        MicroCoroutine coroutine = null;

        if (Instance.coroutineDictionary.ContainsKey(type))
        {
            List<MicroCoroutine> list = Instance.coroutineDictionary[type];

            int index = list.FindIndex(co => co.Equals(co));

            if (index > -1)
            {
                coroutine = list[index];
                coroutine.Addroutine(routine);
            }
            else
            {
                coroutine = new MicroCoroutine(owner);
                coroutine.Addroutine(routine);

                list.Add(coroutine);
            }
        }

        return coroutine;
    }

    public static void StopCoroutine(CoroutineType type, IMicroCoroutine coroutine)
    {
        List<MicroCoroutine> coroutineList = null;

        if(Instance.coroutineDictionary.TryGetValue(type, out coroutineList))
        {
            coroutineList.Remove(coroutine as MicroCoroutine);
        }
    }

    IEnumerator RunUpdateMicroCoroutine()
    {
        while (true)
        {
            yield return null;

            Instance.coroutineDictionary[CoroutineType.Update]
                .ForEach(co => co.Run());
        }
    }

    IEnumerator RunFixedUpdateMicroCoroutine()
    {
        var fu = new WaitForFixedUpdate();

        while (true)
        {
            yield return fu;

            Instance.coroutineDictionary[CoroutineType.FixedUpdate]
                 .ForEach(co => co.Run());
        }
    }

    IEnumerator RunEndOfFrameMicroCoroutine()
    {
        var eof = new WaitForEndOfFrame();

        while (true)
        {
            yield return eof;

            Instance.coroutineDictionary[CoroutineType.EndOfFrame]
                 .ForEach(co => co.Run());
        }
    }
}
