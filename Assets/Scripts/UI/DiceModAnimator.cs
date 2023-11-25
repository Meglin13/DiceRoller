using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DiceModAnimator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private TrailRenderer trail;

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

    private Vector3 initialPosition;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();

        text = GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
    }

    public IEnumerator StartAnimation(TextMeshProUGUI diceResultText, Transform modsListTextTransform, List<DiceModifier> diceMods, int initialDiceResult)
    {
        diceResultText.text = initialDiceResult.ToString();

        if (diceMods.Count > 0)
        {

            int result = initialDiceResult;

            initialPosition = modsListTextTransform.transform.position;

            yield return new WaitForSeconds(animationDelay);

            foreach (var modifier in diceMods)
            {
                transform.position = initialPosition;

                trail.Clear();

                text.text = $"{modifier.Value:+#.##;-#.##;(0)}, ";
                text.alpha = 1;

                text.transform.DOMove(diceResultText.transform.position, animationDuration)
                    .SetEase(ease)
                    .OnComplete(() =>
                    {
                        text.alpha = 0;

                        diceResultText.text = $"{(result + modifier.Value).ToString():+#.##;-#.##;(0)}";

                        float startFontSize = diceResultText.fontSize;

                        DOTween.To(() => startFontSize, x => diceResultText.fontSize = x, startFontSize + 10, animationDuration)
                            .SetLoops(1, LoopType.Yoyo);
                    });

                yield return new WaitForSeconds(animationDelayBetweenMods + animationDuration);
            }
        }

    }
}
