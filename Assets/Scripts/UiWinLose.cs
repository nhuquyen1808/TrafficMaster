using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public enum LOSETYPE
    {
        OUT_OF_MOVES,HUMAND_HITTED,TRUNK_TANKER_HITTED
    }
    public class UiWinLose : MonoBehaviour
    {
        public Button claimAdButton, rePlayButton, nextButton, homeButton;

        [SerializeField] List<GameObject> uiElements = new List<GameObject>();
        public GameObject nShadow;
        public Image iconWinlose, ribbon;
        public Sprite winRibbon, loseRibbon, iconWin,iconOutOfMove,iconHumandHitted,iconTrunkHitted;
        [SerializeField] EffectGetCoin effectGetCoin;
        public ParticleSystem confetifxParticle1, confetifxParticle2;
        private void Awake()
        {
            claimAdButton.onClick.AddListener(OnClickClaimAdButton);
            rePlayButton.onClick.AddListener(OnClickReplayButton);
            nextButton.onClick.AddListener(OnClickNextButton);
            homeButton.onClick.AddListener(OnClickHomeAdButton);
        }
        private void OnClickHomeAdButton()
        {
            ManagerScene.ins.LoadScene("SceneHome");
        }
        private void OnClickNextButton()
        {
            nextButton.interactable = false;
            claimAdButton.GetComponent<Image>().raycastTarget = false;
            /*int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, currentLevel + 1);*/
            effectGetCoin.RewardParentCoin(LogicGame.instance.coinsGet/10,10, LoadSceneGame);
        }

      

        private void OnClickReplayButton()
        {
            LoadSceneGame();
        }

        private void OnClickClaimAdButton()
        {
            claimAdButton.interactable = false;
            nextButton.GetComponent<Image>().raycastTarget = false;
                /*int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
            PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, currentLevel + 1);*/
            effectGetCoin.RewardParentCoin(LogicGame.instance.coinsGet*2/10,10, LoadSceneGame);
        }

        public void ShowWinPanel()
        {
            Debug.Log(LogicGame.instance.coinsGet);
            Duck.PlayParticle(confetifxParticle1);
            Duck.PlayParticle(confetifxParticle2);
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
                    break;
                case LOSETYPE.HUMAND_HITTED:
                    iconWinlose.sprite = iconHumandHitted;
                    break;
                case LOSETYPE.TRUNK_TANKER_HITTED:
                    iconWinlose.sprite = iconTrunkHitted;
                    break;
            }
        }
        
        private static void LoadSceneGame()
        {
            ManagerScene.ins.LoadScene("SceneGame");
        }
    }
}
