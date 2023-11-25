using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    private int diceResult;

    [SerializeField]
    private TextMeshProUGUI diceResultText;

    [SerializeField]
    private TextMeshProUGUI modsListText;

    [SerializeField]
    private List<DiceModifier> modifiers = new();
    [SerializeField]
    private int ModsCapacity = 10;

    public event Action OnModsChanged = delegate { };
    public UnityEvent OnResultCalculated;

    [SerializeField]
    private DiceModAnimator animator;

    private void Start()
    {
        Dice.OnDiceStop += () => StartCoroutine(CalculateResult());

        OnModsChanged += SetModsInfo;

        modifiers.Capacity = ModsCapacity;
    }

    private void OnDestroy()
    {
        OnModsChanged = null;
    }

    private IEnumerator CalculateResult()
    {
        diceResult = Dice.GetUpperSide();

        yield return StartCoroutine(animator.StartAnimation(diceResultText, modsListText.transform, modifiers, diceResult));

        OnResultCalculated?.Invoke();
    }

    private void SetModsInfo()
    {
        modsListText.text = string.Join(", ", modifiers.Select(x => $"{x.Value:+#.##;-#.##;(0)}"));
    }

    public void RollDice()
    {
        if (Dice.rb.velocity == Vector3.zero)
        {
            ApplyForces();

            StartCoroutine(Dice.WaitTillStop());
        }
    }

    private void ApplyForces()
    {
        Dice.transform.position = transform.position;

        float x = Random.Range(-MaxForce, MaxForce);
        float y = Random.Range(-MaxForce, MaxForce);
        float z = Random.Range(-MaxForce, MaxForce);

        Dice.rb.AddForce(x, y, z, ForceMode.Impulse);
        Dice.rb.AddTorque(x * DiceTorqueMod, y * DiceTorqueMod, z * DiceTorqueMod);
    }

    #region [Modifiers Methods]
    public void AddRandomMod()
    {
        if (modifiers.Capacity > modifiers.Count + 1)
        {
            int value = Random.Range(-5, 6);
            if (value == 0)
            {
                value++;
            }

            modifiers.Add(new(value));

            OnModsChanged?.Invoke();
        }
    }

    public void DeleteMod()
    {
        if (modifiers.Count > 0)
        {
            modifiers.RemoveAt(modifiers.Count - 1);

            OnModsChanged?.Invoke();
        }
    }
    #endregion
}
