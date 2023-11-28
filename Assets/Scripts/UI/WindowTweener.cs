using UnityEngine;
using DG.Tweening;

public class WindowTweener : MonoBehaviour
{
    #region References
    private RectTransform rect;
    private CanvasGroup canvasGroup; 
    #endregion

    #region [Tween Config]
    [SerializeField]
    private Ease ease;

    [SerializeField]
    [Range(0, 10f)]
    private float duration;

    [SerializeField]
    private int endAnchorValue = 20; 
    #endregion

    #region [Fade Config]
    [Header("Fade")]
    [SerializeField]
    private Ease fadeEase;

    [SerializeField]
    [Range(0, 5f)]
    private float fadeDuration; 
    #endregion

    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = rect.GetComponent<CanvasGroup>();

        OnOpen();
    }

    /// <summary>
    /// Метод, отвечающий за анимацию окна при его открытии
    /// </summary>
    [ContextMenu("Play Open")]
    public void OnOpen()
    {
        Vector2 startPos = new Vector2(rect.anchoredPosition.x, -rect.rect.height / 2);

        rect.anchoredPosition = startPos;

        rect.DOAnchorPosY(endAnchorValue, duration).SetEase(ease);

        canvasGroup.alpha = 0;

        canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase);
    }
}
