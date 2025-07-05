using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    [Serializable]
    public class Content
    {
        public Sprite sprite;
        public string contentTut;
    }

    public enum TutotialType
    {
        NONE,
        GAMEPLAY_TUT,
        HINT_TUT,
        HELICOPTER_TUT,
        TRUNK_TANKER_TUT,
        HIT_HUMMAN_TUT,
        HIT_REDlIGHT_TUT,
    }

    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private GameObject nShadow;
        [SerializeField] private CanvasGroup nTutorial;
        [SerializeField] private EzButton nextTutorial, prevTutorial,OKButton;
        [SerializeField] private Image imgContent;
        [SerializeField] private TextMeshProUGUI txtContent;
        [SerializeField] private CanvasGroup nGroupContent;

        [Header("List Content")] public List<Content> contentGamePlayTut = new List<Content>();
        [SerializeField] private List<Content> contentHintTut = new List<Content>();
        [SerializeField] private List<Content> contentHelicopterTut = new List<Content>();
        [SerializeField] private List<Content> contentTrunkTankerTut = new List<Content>();
        [SerializeField] private List<Content> contentHitHumanTut = new List<Content>();
        [SerializeField] private List<Content> contentHitRedLightTut = new List<Content>();

        [Header("Current content : ")] [SerializeField]
        private int totalContentShow, currentContentShow;

        public List<Content> currentContent = new List<Content>();

        private void Awake()
        {
            nextTutorial.onClick += OnClickNextTutButton;
            prevTutorial.onClick += OnClickPrevTutButton;
            OKButton.onClick += OnClickOKButton;
        }

        private void OnClickOKButton()
        {
            UiDuck.HidePopup(nShadow, nTutorial);
            GlobalData.isInGame = true;
        }

        private void OnClickPrevTutButton()
        {
            --currentContentShow;
            ShowContent(currentContentShow);
        }

        private void OnClickNextTutButton()
        {
            if(currentContentShow == totalContentShow-1) return;
            currentContentShow++;
            ShowContent(currentContentShow);
        }

        private void ShowTutorial(TutotialType type)
        {
            GlobalData.isInGame = false;
            SetUpTutorial(type);
            ShowContent(0);
            UiDuck.ShowPopup(nShadow, nTutorial);
        }

        private void SetUpTutorial(TutotialType type)
        {
            switch (type)
            {
                case TutotialType.NONE:
                    break;
                case TutotialType.HINT_TUT:
                    currentContent = contentHintTut;
                    break;
                case TutotialType.HELICOPTER_TUT:
                    currentContent = contentHelicopterTut;
                    break;
                case TutotialType.GAMEPLAY_TUT:
                    currentContent = contentGamePlayTut;
                    break;
                case TutotialType.HIT_HUMMAN_TUT:
                    currentContent = contentHitHumanTut;
                    break;
                case TutotialType.HIT_REDlIGHT_TUT:
                    currentContent = contentHitRedLightTut;
                    break;
                case TutotialType.TRUNK_TANKER_TUT:
                    currentContent = contentTrunkTankerTut;
                    break;
            }

            totalContentShow = currentContent.Count;
        }

        private void ShowContent(int currentContentShow)
        {
            nGroupContent.alpha = 0;
            imgContent.sprite = currentContent[currentContentShow].sprite;
            txtContent.text = currentContent[currentContentShow].contentTut;
            nGroupContent.DOFade(1, 0.2f);
            if (currentContentShow == 0)
            {
                prevTutorial.gameObject.SetActive(false);
            }
            else
            {
                prevTutorial.gameObject.SetActive(true);
            }
            
            if (currentContentShow == totalContentShow-1)
            {
                nextTutorial.gameObject.SetActive(false);
            }
            else
            {
                nextTutorial.gameObject.SetActive(true);
            }
        }

        public void SetTutAndShow()
        {
            switch (PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock))
            {
                case 1:
                    ShowTutorial(TutotialType.GAMEPLAY_TUT);
                    break;
                case 3:
                    ShowTutorial(TutotialType.HELICOPTER_TUT);
                    break;
                case 4:
                    ShowTutorial(TutotialType.TRUNK_TANKER_TUT);
                    break;
                case 6:
                    ShowTutorial(TutotialType.HINT_TUT);
                    break;
                case 9:
                    ShowTutorial(TutotialType.HIT_REDlIGHT_TUT);
                    break;
                case 14:
                    ShowTutorial(TutotialType.HIT_HUMMAN_TUT);
                    break;
                default:
                    Debug.Log("No tut at this level");
                    break;
            }
        }
    }
}