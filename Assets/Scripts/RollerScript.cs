using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollerScript : MonoBehaviour
{
    [SerializeField]
    private float MaxForce = 5;

    [SerializeField]
    [Range(0, 5f)]
    private float DiceTorqueMod = 5;

    [SerializeField]
    private DiceScript Dice;

    [SerializeField]
    private TextMeshProUGUI diceResult;

    [SerializeField]
    private TextMeshProUGUI modsList;

    [SerializeField]
    private List<DiceModifier> Mods = new();
    [SerializeField]
    private int ModsCapacity = 10;

    public event Action OnModsChanged = delegate { };

    private void Start()
    {
        Dice.OnDiceStop += SetResultText;

        OnModsChanged += SetModsInfo;

        Mods.Capacity = ModsCapacity;
    }

    private void SetResultText()
    {
        int result = Dice.GetUpperSide();
        result += GetModifiers();

        diceResult.text = result.ToString();
    }

    private void SetModsInfo()
    {
        modsList.text = string.Empty;
        Mods.ForEach(x => modsList.text += $"{x.Value:+#.##;-#.##;(0)}, ");
    }

    public int GetModifiers()
    {
        int result = 0;
        Mods.ForEach(x => result += x.Value);

        return result;
    }

    public void RollDice()
    {
        if (Dice.rb.velocity == Vector3.zero)
        {
            Dice.transform.position = transform.position;

            float x = Random.Range(-MaxForce, MaxForce) ;
            float y = Random.Range(-MaxForce, MaxForce) ;
            float z = Random.Range(-MaxForce, MaxForce) ;

            Dice.rb.AddForce(x, y, z, ForceMode.Impulse);
            Dice.rb.AddTorque(x * DiceTorqueMod, y * DiceTorqueMod, z * DiceTorqueMod);

            StartCoroutine(Dice.WaitTillStop());
        }
    }

    public void AddRandomMod()
    {
        if (Mods.Capacity > Mods.Count + 1)
        {
            int value = Random.Range(-5, 5);
            if (value == 0)
            {
                value++;
            }

            Mods.Add(new(value));

            OnModsChanged?.Invoke();
        }
    }

    public void DeleteMod()
    {
        if (Mods.Count > 0)
        {
            Mods.RemoveAt(Mods.Count - 1);

            OnModsChanged?.Invoke();
        }
    }
}
