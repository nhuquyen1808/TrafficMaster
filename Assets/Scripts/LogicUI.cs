using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class LogicUI : MonoBehaviour
    {
        [SerializeField] LogicGame logicGame;
        [SerializeField] private EzButton hintButton, helicopterButton;
        public static LogicUI ins;
        [SerializeField] TextMeshProUGUI hintAmountText, heicopterAmountText, leveltText, movesText, coinText;

        [Header("Win Lose elements : ")] [SerializeField]
        public TextMeshProUGUI titleText;

        [SerializeField] private GameObject bubbleHelicopter;
        [SerializeField] UiWinLose uiWinLose;
        
        [SerializeField] Sprite lockSprite,helicopterIconSprite,hintIconSprite;
        
        private void Awake()
        {
            ins = this;
            hintButton.onClick += OnClickHintButton;
            helicopterButton.onClick += OnClickHelicopterButton;
        }

        private void OnClickHelicopterButton()
        {
            logicGame.CallHelicopter();
        }

        private void OnClickHintButton()
        {
            logicGame.Hint();
        }

        public void ShowWinPopup(int coin)
        {
            titleText.text = "YOU WIN";
            coinText.text = (coin).ToString();
            AudioManager.instance.PlaySound("Win");
            uiWinLose.ShowWinPanel();
        }

        public void ShowLosePopup(LOSETYPE loseType)
        {
            titleText.text = "YOU LOSE";
            coinText.text = "0";
            AudioManager.instance.PlaySound("Lose");
            uiWinLose.ShowLosePanel(loseType);
        }

        public void PerformBubbleHelicopter(bool isShow)
        {
            if (isShow) bubbleHelicopter.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            else bubbleHelicopter.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public void UpdateHintAmountText(int amount)
        {
            hintAmountText.text = amount.ToString();
            PlayerPrefs.SetInt(PlayerPrefsManager.hintAmount, amount);
        }

        public void UpdateHelicopterAmountText(int amount)
        {
            heicopterAmountText.text = amount.ToString();
            PlayerPrefs.SetInt(PlayerPrefsManager.helicopterAmount, amount);
        }

        public void UpdateCoinText(int coin)
        {
            coinText.text = coin.ToString();
        }

        public void SetupLevelText(int level)
        {
            leveltText.text = "Level : " + level.ToString();
        }

        public void UpdateMovesText(int move)
        {
            movesText.text = "Moves : " + move.ToString();
        }

        public void SetLockBooster()
        {
            int currentLevelUnlock = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            if (currentLevelUnlock < 3)
            {
                Debug.Log(" < 3");
                helicopterButton.imageButton.sprite = lockSprite;
                hintButton.imageButton.sprite = lockSprite;
                helicopterButton.imageButton.raycastTarget = false;
                hintButton.imageButton.raycastTarget = false;
            }
            else if  (currentLevelUnlock >= 3 && currentLevelUnlock < 6)
            {
                Debug.Log(" < 3 && < 6");

                helicopterButton.imageButton.sprite = helicopterIconSprite;
                hintButton.imageButton.sprite = lockSprite;
                hintButton.imageButton.raycastTarget = false;
            }
            else /*if  (currentLevelUnlock == 6)*/
            {
                helicopterButton.imageButton.sprite = helicopterIconSprite;
                hintButton.imageButton.sprite = hintIconSprite;
            }
        }
    }
}