using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    public List<int> modifiers = new List<int>();

    public event Action<int, int> OnValueChanged;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
        OnValueChanged?.Invoke(GetValue(), baseValue);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
        OnValueChanged?.Invoke(GetValue(), baseValue);
    }
}
