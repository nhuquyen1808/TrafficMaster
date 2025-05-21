using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class ShopIAPController : MonoBehaviour
    {
        public static ShopIAPController ins;
        public Button closeShopButton;

        public Button choice1Button,
            choice2Button,
            choice3Button,
            choice4Button,
            choice5Button,
            choice6Button,
            choice7Button;

        public GameObject shadow;
        public CanvasGroup nPopup;

        [Header("tabs")] public Button IAPButton;
        public Button landSkinButton;
        public GameObject IAPTab, landSkinTab;

        private void Awake()
        {
            ins = this;
        }

        void Start()
        {
            IAPButton.onClick.AddListener(OnClickIAPButton);
            landSkinButton.onClick.AddListener(OnClickLandSkinButton);


            closeShopButton.onClick.AddListener(OnClickCloseShopButton);
            choice1Button.onClick.AddListener(OnClickChoice1Button);
            choice2Button.onClick.AddListener(OnClickChoice2Button);
            choice3Button.onClick.AddListener(OnClickChoice3Button);
            choice4Button.onClick.AddListener(OnClickChoice4Button);
            choice5Button.onClick.AddListener(OnClickChoice5Button);
            choice6Button.onClick.AddListener(OnClickChoice6Button);
            choice7Button.onClick.AddListener(OnClickChoice7Button);
        }


        private void OnClickChoice7Button()
        {
            Debug.Log("Handle buy button 7 clicked");
        }

        private void OnClickChoice6Button()
        {
            Debug.Log("Handle buy button 6 clicked");
        }

        private void OnClickChoice5Button()
        {
            Debug.Log("Handle buy button 5 clicked");
        }

        private void OnClickChoice4Button()
        {
            Debug.Log("Handle buy button 4 clicked");
        }

        private void OnClickChoice3Button()
        {
            Debug.Log("Handle buy button 3 clicked");
        }

        private void OnClickChoice2Button()
        {
            Debug.Log("Handle buy button 2 clicked");
        }

        private void OnClickChoice1Button()
        {
            Debug.Log("Handle buy button 1 clicked");
            choice1Button.transform.DOScale(0.95f, 0.1f).OnComplete(() =>
            {
                choice1Button.transform.DOScale(1, 0.1f);
            });
            float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
            if (coin >= 1000)
            {
                Debug.Log("buy button 1 clicked");
                coin -= 1000;
                PlayerPrefs.SetInt(PlayerPrefsManager.hintAmount,
                    PlayerPrefs.GetInt(PlayerPrefsManager.hintAmount) + 5);
                PlayerPrefs.SetInt(PlayerPrefsManager.helicopterAmount,
                    PlayerPrefs.GetInt(PlayerPrefsManager.helicopterAmount) + 5);
                PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);
                Observer.Notify(EventAction.EVENT_UPDATE_COIN, 1000);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        private void OnClickCloseShopButton()
        {
            closeShopButton.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);
            nPopup.DOFade(0, 0.3f).OnComplete((() => { nPopup.gameObject.SetActive(false); }));
            //  UiDuck.HidePopup(shadow,nPopup);
        }

        private void OnClickLandSkinButton()
        {
            landSkinTab.SetActive(true);
            IAPTab.SetActive(false);
            IAPButton.GetComponent<Image>().color = new Color32(154, 154, 154, 255);
            landSkinButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        private void OnClickIAPButton()
        {
            landSkinTab.SetActive(false);
            IAPTab.SetActive(true);
            landSkinButton.GetComponent<Image>().color = new Color32(154, 154, 154, 255);
            IAPButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}