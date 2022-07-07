using SDDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IEnumerable 형태로 구현을 성공해야 쓸 수가 있음
/// 일단 시간이 걸릴 거 같아서 보류
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TKey2"></typeparam>
/// <typeparam name="TValue"></typeparam>

public class DoubleDictionary<TKey, TKey2, TValue>
{
    Dictionary<TKey, Dictionary<TKey2, TValue>> dictionary
        = new Dictionary<TKey, Dictionary<TKey2, TValue>>();

    public void Add(TKey key, TKey2 key2, TValue value)
    {
        if(dictionary.ContainsKey(key))
        {
            if(dictionary[key].ContainsKey(key2))
            {
                dictionary[key][key2] = value;
            }
            else
            {
                dictionary[key].Add(key2, value);
            }
        }
        else
        {
            dictionary.Add(key, new Dictionary<TKey2, TValue>()
            {
                { key2, value }
            });
        }
    }

    public bool ContainsKey(TKey key, TKey2 key2)
    {
        if(dictionary.ContainsKey(key))
        {
            if(dictionary[key].ContainsKey(key2))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetValue(TKey key, TKey2 key2, out TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            if (dictionary[key].ContainsKey(key2))
            {
                value = dictionary[key][key2];
                return true;
            }
        }

        value = default;

        return false;
    }
}