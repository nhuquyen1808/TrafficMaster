using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EzButton : MonoBehaviour, IPointerClickHandler
{
    public event Action onClick;
    public bool changeColorOnHover = true;
    public Color hoverColor = Color.gray;
    private Color originalColor;
    [FormerlySerializedAs("image")] public Image imageButton;

    private void Awake()
    {
        imageButton = GetComponent<Image>();
        originalColor = imageButton.color;
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
        if (changeColorOnHover && imageButton != null)
            imageButton.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (changeColorOnHover && imageButton != null)
            imageButton.color = originalColor;
    }


}
