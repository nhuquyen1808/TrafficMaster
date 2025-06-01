using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class PanelSettings : MonoBehaviour
    {
        public GameObject npopup, nShadow;
        public EzButton settingsButton, closeButton, soundButton, musicButton, homeButton;
        public Sprite sprButtonOn, sprButtonOff;
        
        private void Awake()
        {
            /*
            if (PlayerPrefs.GetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD) == 0)
            {
                Debug.Log("????//");
                PlayerPrefs.SetInt("MUSIC", 1);
                PlayerPrefs.SetInt("SOUND", 1);
                PlayerPrefs.SetInt(PlayerPrefsManager.helicopterAmount, 5);
                PlayerPrefs.SetInt(PlayerPrefsManager.hintAmount, 5);
                PlayerPrefs.SetInt(PlayerPrefsManager.Coin, 500);
                PlayerPrefs.SetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD, 1);
            }
            */

            settingsButton.onClick += (OnClickSettingsButton);
            closeButton.onClick += OnClickCloseButton;
            soundButton.onClick += OnClickSoundButton;
            musicButton.onClick += OnClickMusicButton;
            homeButton.onClick += OnClickHomeButton;
        }

        private void OnClickHomeButton()
        {
            GlobalData.isInGame = false;
            DOTween.KillAll();
            ManagerScene.ins.LoadScene("SceneHome");
        }

        private void Start()
        {
            SetUpButtonOnstart();
        }

        public void SetUpButtonOnstart()
        {
            int isMusic = PlayerPrefs.GetInt("MUSIC");
            int isSound = PlayerPrefs.GetInt("SOUND");
            if (isMusic == 1)
            {
                musicButton.GetComponent<Image>().sprite = sprButtonOn;
            }
            else
            {
                musicButton.GetComponent<Image>().sprite = sprButtonOff;
            }

            if (isSound == 1)
            {
                soundButton.GetComponent<Image>().sprite = sprButtonOn;
            }
            else
            {
                soundButton.GetComponent<Image>().sprite = sprButtonOff;
            }
        }

        private void OnClickMusicButton()
        {
            MusicButtonClicked();
        }

        public void MusicButtonClicked()
        {
            int isMusic = PlayerPrefs.GetInt("MUSIC");
            if (isMusic == 1)
            {
                AudioManager.instance.StopPlayMusic();
                PlayerPrefs.SetInt("MUSIC", 0);
                musicButton.GetComponent<Image>().sprite = sprButtonOff;
            }
            else
            {
                AudioManager.instance.ContinuePlayMusic();
                PlayerPrefs.SetInt("MUSIC", 1);
                musicButton.GetComponent<Image>().sprite = sprButtonOn;
            }
        }

        private void OnClickSoundButton()
        {
            SoundButtonClicked();
        }

        public void SoundButtonClicked()
        {
            int isMusic = PlayerPrefs.GetInt("SOUND");
            if (isMusic == 1)
            {
                AudioManager.instance.StopPlaySound();
                PlayerPrefs.SetInt("SOUND", 0);
                soundButton.GetComponent<Image>().sprite = sprButtonOff;
            }
            else
            {
                AudioManager.instance.ContinuePlaySound();
                PlayerPrefs.SetInt("SOUND", 1);
                soundButton.GetComponent<Image>().sprite = sprButtonOn;
            }
        }

        private void OnClickCloseButton()
        {
            npopup.transform.localScale = Vector3.one;
            npopup.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            npopup.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                nShadow.SetActive(false);
                ManagerGame.TIME_SCALE = 1;
                // GlobalData.isInGame = true;
            });
        }

        private void OnClickSettingsButton()
        {
            //  GlobalData.isInGame = false;
            ManagerGame.TIME_SCALE = 0;
            settingsButton.transform.DOScale(0.95f, 0.1f).OnComplete((() =>
            {
                settingsButton.transform.DOScale(1, 0.1f).OnComplete(() =>
                {
                    nShadow.SetActive(true);
                    npopup.transform.localScale = Vector3.zero;
                    npopup.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
                    npopup.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.OutBack);
                });
            }));
        }
    }
}