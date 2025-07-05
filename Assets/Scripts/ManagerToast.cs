using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerToast : MonoBehaviour
{
    public static ManagerToast instance;
    public TextMeshProUGUI textContent;
    public CanvasGroup toastObject;
    public EzButton okButton;
    public bool isHasInternetConnection;
    public GameObject nShadow;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
//            DontDestroyOnLoad(gameObject);
        }

        okButton.onClick += OnClickOkButton;
    }

    private void Start()
    {
     //   CheckInternetConnection();
    }

    private void OnClickOkButton()
    {
        nShadow.gameObject.SetActive(false);
        toastObject.alpha = 1;
        toastObject.transform.DOScale(0, 0.5f).From(1);
        toastObject.DOFade(0, 0.5f).SetDelay(1).OnComplete(() => { toastObject.gameObject.SetActive(false); });
    }

    public void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Show();
            isHasInternetConnection = false;
        }
        else
        {
            isHasInternetConnection = true;
        }
    }

    public void Show()
    {
        nShadow.gameObject.SetActive(true);
        toastObject.gameObject.SetActive(true);
        toastObject.alpha = 0;
        toastObject.DOFade(1, 0.5f);
        toastObject.transform.DOScale(1, 0.5f).From(0);
    }
}