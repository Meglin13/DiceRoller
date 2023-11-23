using System;
using UnityEngine;

[Serializable]
public class DiceModifier
{
    private int maxValue = 10;

    [SerializeField]
    private int value;
    public int Value
    {
        get => value;
        set => this.value = value > maxValue ? 0 : value;
    }

    public DiceModifier(int value)
    {
        this.value = value;
    }
}