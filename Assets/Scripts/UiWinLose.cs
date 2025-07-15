using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum LOSETYPE
    {
        OUT_OF_MOVES,
        HUMAND_HITTED,
        TRUNK_TANKER_HITTED
    }

    public class UiWinLose : MonoBehaviour
    {
        public EzButton claimAdButton, rePlayButton, nextButton, homeButton;
        [SerializeField] List<GameObject> uiElements = new List<GameObject>();
        [SerializeField] private GameObject nShadow;
        [SerializeField] private Image iconWinlose, ribbon;
        [SerializeField] private Sprite winRibbon, loseRibbon, iconWin, iconOutOfMove, iconHumandHitted, iconTrunkHitted;
        [SerializeField] EffectGetCoin effectGetCoin;
        [SerializeField] private ParticleSystem confetifxParticle1, confetifxParticle2, sparkleWin;
        [SerializeField] private TextMeshProUGUI ReasionText;
        private List<String> WinTextList = new List<String>(){"Incredible!", "Perfect!","Wow!","Unbelievable!"};
        private void Awake()
        {
            claimAdButton.onClick += (OnClickClaimAdButton);
            rePlayButton.onClick += (OnClickReplayButton);
            nextButton.onClick += (OnClickNextButton);
            homeButton.onClick += (OnClickHomeAdButton);
        }
        private void OnClickHomeAdButton()
        {
            ManagerScene.ins.LoadScene("SceneHome");
        }
        private void OnClickNextButton()
        {
            AudioManager.instance.PlaySound("Coin");
            nextButton.imageButton.raycastTarget = false;
            claimAdButton.GetComponent<Image>().raycastTarget = false;
            /*int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, currentLevel + 1);*/
            effectGetCoin.GetReward(LogicGame.instance.coinsGet, 10, LoadSceneGame);
        }
        private void OnClickReplayButton()
        {
            LoadSceneGame();
        }
        private void OnClickClaimAdButton()
        {
            AudioManager.instance.PlaySound("Coin");
            claimAdButton.imageButton.raycastTarget = false;
            nextButton.GetComponent<Image>().raycastTarget = false;
            /*int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
        PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, currentLevel + 1);*/
            effectGetCoin.GetReward(LogicGame.instance.coinsGet * 2, 10, LoadSceneGame);
        }
        public void ShowWinPanel()
        {
            Duck.PlayParticle(confetifxParticle1);
            Duck.PlayParticle(confetifxParticle2);
            Duck.PlayParticle(sparkleWin);
            nShadow.SetActive(true);
            
            iconWinlose.sprite = iconWin;
            ribbon.sprite = winRibbon;
            rePlayButton.gameObject.SetActive(false);
            homeButton.gameObject.SetActive(false);
            for (int i = 0; i < uiElements.Count; i++)
            {
                var a = i;
                uiElements[a].transform.DOScale(1, 0.2f).SetDelay(a * 0.1f);
            }
            ReasionText.text = WinTextList[Random.Range(0, WinTextList.Count)];
        }
        public void ShowLosePanel(LOSETYPE loseType)
        {
            nShadow.SetActive(true);
            ribbon.sprite = loseRibbon;
            nextButton.gameObject.SetActive(false);
            claimAdButton.gameObject.SetActive(false);
            for (int i = 0; i < uiElements.Count; i++)
            {
                var a = i;
                uiElements[a].transform.DOScale(1, 0.2f).SetDelay(a * 0.1f);
            }

            switch (loseType)
            {
                case LOSETYPE.OUT_OF_MOVES:
                    iconWinlose.sprite = iconOutOfMove;
                    ReasionText.text = "Oop!Out of moves!";
                    break;
                case LOSETYPE.HUMAND_HITTED:
                    iconWinlose.sprite = iconHumandHitted;
                    ReasionText.text = "Oop!Don't hit pedestrian!";
                    break;
                case LOSETYPE.TRUNK_TANKER_HITTED:
                    iconWinlose.sprite = iconTrunkHitted;
                    ReasionText.text = "Oop!Don't hit trunk tanker!";
                    break;
            }
        }

        private static void LoadSceneGame()
        {
            ManagerScene.ins.LoadScene("SceneGame");
        }
    }
}