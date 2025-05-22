using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class UiHome : MonoBehaviour
    {
        [SerializeField] Button shopButton,playButton,noAdsButton;
        [SerializeField] GameObject shadowShop;
        [SerializeField] CanvasGroup nPopupShop;
        [SerializeField] private GameObject car;

        [Header("Somethings Show : ")] 
        [SerializeField] GameObject levelBoard;
        [SerializeField] GameObject stopBoard,barrier;
        [SerializeField] TextMeshPro levelText;
        [SerializeField] List<GameObject> objectsShowed =  new List<GameObject>();
        int currentLevel ;
        public ScrollRect scrollRectShop;
        public NoAdsManager noAdsManager;
        private void Awake()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            shopButton.onClick.AddListener(OnClickShopButton);
            noAdsButton.onClick.AddListener(OnClickNoAdsButton);
        }

        private void OnClickNoAdsButton()
        {
            noAdsManager.ShowNoAds();
        }

        private void Start()
        {

            StartCoroutine(ShowElements()) ;
            AudioManager.instance.PlayBGMSound("BGM");
          
        }

        IEnumerator  ShowElements() 
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
                objectsShowed[a].transform.DOScale(1f, 0.3f).SetDelay(a*0.3f).SetEase(Ease.OutBack);
            }
            playButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            
            currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            if (currentLevel < 1)
            {
                levelText.text = "Level: 1" ;
            }
            else
            {
                levelText.text = "Level: "+ PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock).ToString();
            }
        }

        private void OnClickShopButton()
        {
            ShopIAPController.ins.closeShopButton.gameObject.SetActive(true);
            shadowShop.SetActive(true);
            nPopupShop.gameObject.SetActive(true);
           // nPopupShop.transform.DOScale(1,0.3f).SetEase(Ease.OutBack);
            nPopupShop.DOFade(1, 0.3f).OnUpdate(() =>
            {
                scrollRectShop.verticalNormalizedPosition = 1f;
            });
          //  UiDuck.ShowPopup(shadowShop,nPopupShop);
        }
        private void OnClickPlayButton()
        {
            shopButton.gameObject.SetActive(false);
            car.transform.DOMoveZ(3.5f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                ManagerScene.ins.LoadScene("SceneGame");

            });
            playButton.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GlobalData.isInGame = true;
            });
        }
    }
}
