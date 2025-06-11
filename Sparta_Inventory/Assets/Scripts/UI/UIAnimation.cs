using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]

public class UIAnimation : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.3f;

    private void InitializeCanvas()
    {
        if(canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Open() // 스르륵 열리기
    {
        InitializeCanvas();

        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public virtual void Close() // 스르륵 닫히기
    {
        InitializeCanvas();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
