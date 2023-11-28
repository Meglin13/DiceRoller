using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
/// Класс, отвечающий за анимацию модификаторов и подсчет результата броска
/// </summary>
public class DiceModAnimator : MonoBehaviour
{
    #region References
    private TextMeshProUGUI text;

    private TrailRenderer trail;
    #endregion

    #region [Tween Config]
    [SerializeField]
    private Ease ease;

    [SerializeField]
    [Range(0, 5f)]
    private float animationDuration = 0.5f;

    [SerializeField]
    [Range(0, 5f)]
    private float animationDelay = 1;

    [SerializeField]
    [Range(0, 5f)]
    private float animationDelayBetweenMods = 0.2f; 
    #endregion

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();

        text = GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
    }

    /// <summary>
    /// Метод, отвечающий за анимацию модификаторов и подсчет результата броска
    /// </summary>
    /// <param name="diceResultText">Текст результата броска</param>
    /// <param name="modsListTextTransform">Трансформ списка модификаторов</param>
    /// <param name="diceMods">Список модификаторов</param>
    /// <param name="result">Результат броска</param>
    /// <returns></returns>
    public IEnumerator StartAnimation(TextMeshProUGUI diceResultText, Transform modsListTextTransform, List<DiceModifier> diceMods, int result)
    {
        diceResultText.text = result.ToString();

        // Анимация будет проигрываться только, если есть модификаторы
        if (diceMods.Count > 0)
        {
            // Устанавливается начальная позиция, которая находится на месте игрового объекта с текстом, отображающим список модификаторов
            var initialPosition = modsListTextTransform.transform.position;
            
            // Задержка анимации
            yield return new WaitForSeconds(animationDelay);

            // Перебор модификаторов
            foreach (var modifier in diceMods)
            {
                transform.position = initialPosition;

                trail.Clear();

                // Установка текста TextMeshProUGUI на игровом объекте класса
                text.text = $"{modifier.Value:+#.##;-#.##;(0)}";
                text.alpha = 1;

                // Анимация перемещение игрового объекта от списка модификаторов к результату броска
                text.transform.DOMove(diceResultText.transform.position, animationDuration)
                    .SetEase(ease)
                    .OnComplete(() =>
                    {
                        // При завершении анимации устанавливается альфа текста,
                        text.alpha = 0;

                        // а также текст результата броска с прибавленным к нему значением текущего модификатора
                        diceResultText.text = $"{(result + modifier.Value).ToString():+#.##;-#.##;(0)}";
                    });

                // Задержка перед анимацией следующего модификатора
                yield return new WaitForSeconds(animationDelayBetweenMods + animationDuration);
            }
        }

    }
}
