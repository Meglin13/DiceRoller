using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>
/// Класс, отвечающий за логику броска кубика
/// </summary>
public class RollerScript : MonoBehaviour
{
    #region [Roll Forces]
    [SerializeField]
    private float MaxForce = 5;

    [SerializeField]
    [Range(0, 5f)]
    private float DiceTorqueMod = 5; 
    #endregion

    /// <summary>
    /// Ссылка на объект кубика
    /// </summary>
    [SerializeField]
    private DiceScript Dice;

    [SerializeField]
    private int diceResult;

    #region TextMeshPro
    [SerializeField]
    private TextMeshProUGUI diceResultText;

    [SerializeField]
    private TextMeshProUGUI modsListText; 
    #endregion

    #region Modifiers
    [SerializeField]
    private List<DiceModifier> modifiers = new();
    [SerializeField]
    private int ModsCapacity = 10; 
    #endregion

    #region Events
    /// <summary>
    /// Событие, вызываемое при изменение модификаторов
    /// </summary>
    public event Action OnModsChanged = delegate { };

    /// <summary>
    /// Событие, вызываемое после подсчета результата
    /// </summary>
    public UnityEvent OnResultCalculated; 
    #endregion

    [SerializeField]
    private DiceModAnimator animator;

    private void Start()
    {
        Dice.OnDiceStop += () => StartCoroutine(CalculateResult());

        OnModsChanged += SetModsInfo;

        modifiers.Capacity = ModsCapacity;
    }

    private void OnDestroy() => OnModsChanged = null;

    /// <summary>
    /// Метод подсчета результата броска
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalculateResult()
    {
        diceResult = Dice.GetUpperSide();

        yield return StartCoroutine(animator.StartAnimation(diceResultText, modsListText.transform, modifiers, diceResult));

        OnResultCalculated?.Invoke();
    }

    /// <summary>
    /// Метод броска кубика. Выполняется только если кубик в покое
    /// </summary>
    public void RollDice()
    {
        if (Dice.rb.velocity == Vector3.zero)
        {
            ApplyForces();

            StartCoroutine(Dice.WaitTillStop());
        }
    }

    /// <summary>
    /// Метод для приложения случайных силы и вращения кубику
    /// </summary>
    private void ApplyForces()
    {
        Dice.transform.position = transform.position;

        float x = Random.Range(-MaxForce, MaxForce);
        float y = Random.Range(-MaxForce, MaxForce);
        float z = Random.Range(-MaxForce, MaxForce);

        Dice.rb.AddForce(x, y, z, ForceMode.Impulse);
        Dice.rb.AddTorque(x * DiceTorqueMod, y * DiceTorqueMod, z * DiceTorqueMod);
    }

    private void SetModsInfo() => modsListText.text = string.Join(", ", modifiers.Select(x => $"{x.Value:+#.##;-#.##;(0)}"));

    #region [Modifiers Methods]
    /// <summary>
    /// Метод для добавления случайного модификатора
    /// </summary>
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

    /// <summary>
    /// Метод удаления модификатора из списка
    /// </summary>
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
