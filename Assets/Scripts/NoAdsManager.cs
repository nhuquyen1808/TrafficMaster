
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class NoAdsManager : MonoBehaviour
    {
        public GameObject nShadow;

        public CanvasGroup nPopup;
        public EzButton BuyButton;
        public Button CloseButton;

        void Awake()
        {
            CloseButton.onClick.AddListener(OnClickCloseButton);
            BuyButton.onClick+=(OnClickBuyButton);

        }
        private void OnClickBuyButton()
        {
            if (ManagerToast.instance.isHasInternetConnection == false)
            {
                ManagerToast.instance.Show();
                return;
            } 
          
            
            if (PlayerPrefs.GetInt("IAPPurchased") == 0)
            {
                PlayerPrefs.SetInt("IAPPurchased", 1);
                Debug.Log("Buy button clicked");
                int hintAmount = PlayerPrefs.GetInt(PlayerPrefsManager.hintAmount);
                float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
                int helicopterAmount = PlayerPrefs.GetInt(PlayerPrefsManager.helicopterAmount);
                hintAmount += 5;
                coin += 1000;
                helicopterAmount += 5;
                PlayerPrefs.SetInt(PlayerPrefsManager.hintAmount, hintAmount);
                PlayerPrefs.SetInt(PlayerPrefsManager.helicopterAmount, helicopterAmount);
                PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);
                Observer.Notify(EventAction.EVENT_UPDATE_COIN, coin);
                BuyButton.imageButton.raycastTarget = false;
                BuyButton.imageButton.color = new Color32(154, 154, 154, 255);
            }
        }

        private void OnClickCloseButton()
        {
            UiDuck.HidePopup(nShadow, nPopup);
        }


        public void ShowNoAds()
        {
            UiDuck.ShowPopup(nShadow, nPopup);
            if (PlayerPrefs.GetInt("IAPPurchased") == 1)
            {
                BuyButton.imageButton.raycastTarget = false;
                BuyButton.imageButton.color = new Color32(154, 154, 154, 255);
            }
        }
    }
}