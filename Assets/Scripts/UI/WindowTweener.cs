using UnityEngine;
using DG.Tweening;

public class WindowTweener : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Ease ease;

    [SerializeField]
    private int LoopCount = 1;

    [SerializeField]
    [Range(0, 10f)]
    private float duration;

    [SerializeField]
    private int endAnchorValue = 20;

    [Header("Fade")]
    [SerializeField]
    private Ease fadeEase;

    [SerializeField]
    [Range(0, 5f)]
    private float fadeDuration;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = rect.GetComponent<CanvasGroup>();

        OnOpen();
    }

    [ContextMenu("Play Open")]
    public void OnOpen()
    {
        Vector2 startPos = new Vector2(rect.anchoredPosition.x, -rect.rect.height / 2);

        rect.anchoredPosition = startPos;

        rect.DOAnchorPosY(endAnchorValue, duration).SetEase(ease);

        canvasGroup.alpha = 0;

        canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase);
    }

    public void OnClose()
    {

    }
}
