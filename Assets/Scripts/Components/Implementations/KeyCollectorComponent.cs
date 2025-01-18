using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectorComponent : MonoBehaviour, IKeyCollector
{
    private Dictionary<string, Key> keyDictionary = new();
    public void Collect(Key key)
    {
        keyDictionary.Add(key.keyColor, key);
    }

    public bool HasKey(string keyColor)
    {
        return keyDictionary.ContainsKey(keyColor);
    }

    public void UseKey(string keyColor)
    {
        if (keyDictionary.ContainsKey(keyColor))
        {
            keyDictionary[keyColor].UseKey();
            keyDictionary.Remove(keyColor);
        }
    }
}
