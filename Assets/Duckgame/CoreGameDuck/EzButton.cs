using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EzButton : MonoBehaviour, IPointerClickHandler
{
    public event Action onClick;
    public bool changeColorOnHover = true;
    public Color hoverColor = Color.gray;
    private Color originalColor;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.transform.DOScale(0.9f, 0.12f).OnComplete(() =>
        {
            this.transform.DOScale(1, 0.12f).OnComplete(() =>
            {
                onClick?.Invoke();
            });
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (changeColorOnHover && image != null)
            image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (changeColorOnHover && image != null)
            image.color = originalColor;
    }


}
