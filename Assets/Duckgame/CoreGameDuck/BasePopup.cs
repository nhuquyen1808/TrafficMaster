using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    /*[Header("Animation Settings")]
    public float fadeDuration = 0.25f;
    public float scaleDuration = 0.25f;
    public Vector3 showScale = Vector3.one;
    public Vector3 hideScale = Vector3.zero;

    private CanvasGroup canvasGroup;*/
    public GameObject shadow;

    protected virtual void Awake()
    {
        /*canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;
        transform.localScale = hideScale;
        gameObject.SetActive(false);*/
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
      /*  canvasGroup.DOFade(1, fadeDuration).SetEase(Ease.OutSine);
        transform.DOScale(showScale, scaleDuration).SetEase(Ease.OutBack);*/
    }

    public virtual void Close()
    {
       /* canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InSine);
        transform.DOScale(hideScale, scaleDuration).SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));*/
    }

}
