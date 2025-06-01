using System;
using System.Collections;
using System.Collections.Generic;
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
        public GameObject nShadow;
        public CanvasGroup nTutorial;
        public EzButton nextTutorial, prevTutorial,OKButton;
        public Image imgContent;
        public TextMeshProUGUI txtContent;


        [Header("List Content")] public List<Content> contentGamePlayTut = new List<Content>();
        public List<Content> contentHintTut = new List<Content>();
        public List<Content> contentHelicopterTut = new List<Content>();
        public List<Content> contentTrunkTankerTut = new List<Content>();
        public List<Content> contentHitHumanTut = new List<Content>();
        public List<Content> contentHitRedLightTut = new List<Content>();

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
            Debug.Log("  Prev Tutorial ");
            --currentContentShow;
            ShowContent(currentContentShow);
        }

        private void OnClickNextTutButton()
        {
            if(currentContentShow == totalContentShow-1) return;
            currentContentShow++;
            ShowContent(currentContentShow);
            Debug.Log("  Next Tutorial ");
        }

        public void ShowTutorial(TutotialType type)
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
            imgContent.sprite = currentContent[currentContentShow].sprite;
            txtContent.text = currentContent[currentContentShow].contentTut;
            
            
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
    }
}