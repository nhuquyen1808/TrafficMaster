using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace DevDuck
{
    public class UiHome : MonoBehaviour
    {
        [SerializeField] EzButton shopButton, playButton, noAdsButton, luckyWheelButton;
        [SerializeField] GameObject shadowShop;
        [SerializeField] CanvasGroup nPopupShop;
        [SerializeField] private GameObject car;

        [Header("Somethings Show : ")] [SerializeField]
        GameObject levelBoard;

        [SerializeField] GameObject stopBoard, barrier;
        [SerializeField] TextMeshPro levelText;
        [SerializeField] List<GameObject> objectsShowed = new List<GameObject>();
        int currentLevel;
        public ScrollRect scrollRectShop;
        public NoAdsManager noAdsManager;
        [SerializeField] GameObject luckyWheelPanel;
        [SerializeField] private GameObject handtut;

        private void Awake()
        {
            playButton.onClick += OnClickPlayButton;
            shopButton.onClick += OnClickShopButton;
            noAdsButton.onClick += OnClickNoAdsButton;
            luckyWheelButton.onClick += OnClickLuckyWheelButtonClicked;
        }

        private void OnClickLuckyWheelButtonClicked()
        {
            Debug.Log("Show LuckyWheel");
            luckyWheelPanel.SetActive(true);
        }

        private void OnClickNoAdsButton()
        {
            noAdsManager.ShowNoAds();
        }

        private void Start()
        {
            Addressables.InitializeAsync();
            StartCoroutine(ShowElements());
            if (AudioManager.instance.sourcesMusic.Count == 0)
            {
                AudioManager.instance.PlayBGMSound("BGM");
            }

            int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock, 0);
            if (currentLevel == 0)
            {
                handtut.SetActive(true);
            }
            else
            {
                handtut.SetActive(false);
            }
        }

        IEnumerator ShowElements()
        {
            yield return new WaitForSeconds(0.5f);
            stopBoard.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack);
            barrier.SetActive(true);
            barrier.transform.DOMoveY(0, 0.3f);
            levelBoard.SetActive(true);
            levelBoard.transform.DORotate(new Vector3(0, 180, 0), 0.3f).SetEase(Ease.Linear).SetDelay(0.4f);
            for (int i = 0; i < objectsShowed.Count; i++)
            {
                var a = i;
                objectsShowed[a].transform.DOScale(1f, 0.3f).SetDelay(a * 0.3f).SetEase(Ease.OutBack);
            }

            playButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

            currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            if (currentLevel < 1)
            {
                levelText.text = "Level: 1";
            }
            else
            {
                levelText.text = "Level: " + PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock).ToString();
            }
        }

        private void OnClickShopButton()
        {
            ShopIAPController.ins.closeShopButton.gameObject.SetActive(true);
            shadowShop.SetActive(true);
            nPopupShop.gameObject.SetActive(true);
            // nPopupShop.transform.DOScale(1,0.3f).SetEase(Ease.OutBack);
            nPopupShop.DOFade(1, 0.3f).OnUpdate(() => { scrollRectShop.verticalNormalizedPosition = 1f; });
            //  UiDuck.ShowPopup(shadowShop,nPopupShop);
        }

        private void OnClickPlayButton()
        {
            shopButton.imageButton.raycastTarget = false;
            shopButton.GetComponent<RectTransform>().DOAnchorPos(shopButton.GetComponent<RectTransform>().anchoredPosition + new Vector2(300,0), 0.5f);
            luckyWheelButton.imageButton.raycastTarget = false;
            luckyWheelButton.GetComponent<RectTransform>().DOAnchorPos(luckyWheelButton.GetComponent<RectTransform>().anchoredPosition + new Vector2(-300,0), .5f);
            noAdsButton.imageButton.raycastTarget = false;
            noAdsButton.GetComponent<RectTransform>().DOAnchorPos(noAdsButton.GetComponent<RectTransform>().anchoredPosition + new Vector2(-300,0), 0.5f);
            car.transform.DOMoveZ(2.5f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                ManagerScene.ins.LoadScene("SceneGame");
                GlobalData.isInGame = true;
            });
            playButton.GetComponent<Image>().raycastTarget = false;
            playButton.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        }
    }
}