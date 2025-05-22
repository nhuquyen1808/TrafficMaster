using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class NoAdsManager : MonoBehaviour
    {
        public GameObject nShadow;

        public CanvasGroup nPopup;
        public Button CloseButton,BuyButton;
        void Awake()
        {
            CloseButton.onClick.AddListener(OnClickCloseButton);
            BuyButton.onClick.AddListener(OnClickBuyButton);
        }

        private void OnClickBuyButton()
        {
            Debug.Log("Buy button clicked");
        }

        private void OnClickCloseButton()
        {
            UiDuck.HidePopup(nShadow,nPopup);
        }


        public void ShowNoAds()
        {
            UiDuck.ShowPopup(nShadow,nPopup);
        }
        
    }
}
