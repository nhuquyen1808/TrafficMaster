using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace DevDuck
{
    public enum PrizeType
    {
        NONE = 0,
        GOLD = 1,
        HELICOPTER = 2,
        HINT = 3,
    }

    [Serializable]
    public class Prize
    {
        public int id;
        public float weight;
        public PrizeType type;
        public float amount;
    }

    public class LuckyWheelController : MonoBehaviour
    {
        public List<Prize> prizeList = new List<Prize>();
        public EzButton SpinButton, SpinButtonAd, GetButton, GetButtonAd;
        //  public GameObject gameObj1, gameObj2;

        [SerializeField] private GameObject PanelGetPrize;
        [SerializeField] Animator getPrizeAnimator;
        public Button closeButton;
        [Header(("Panel Get Prize"))] public Image Icon;
        public TextMeshProUGUI amountPrizeTxt;
        public Sprite coinSprite, helicopterSprite, hintSprite;
        Prize currentPrize;

        private void Awake()
        {
            SpinButton.onClick += (OnClickSpinButton);
            SpinButtonAd.onClick += (OnClickSpinAdButton);
            GetButton.onClick += (OnClickGetButton);
            GetButtonAd.onClick += (OnClickGetButtonAd);
            closeButton.onClick.AddListener(OnClickCloseButton);
        }

        private void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }

        private void OnClickSpinAdButton()
        {
            Debug.Log("Spin by watch ad");
        }

        private void OnClickGetButtonAd()
        {
            PanelGetPrize.SetActive(false);
            Debug.Log("Get x2 prize");
            SetPrizeGeted(currentPrize, 2);
        }

        private void OnClickGetButton()
        {
            Debug.Log("Get prize");
            PanelGetPrize.SetActive(false);
            SetPrizeGeted(currentPrize, 1);
            //gameObj1.GetComponent<RectTransform>().DOMove(gameObj2.GetComponent<RectTransform>().position, 1.2f).SetEase(Ease.InBack);
        }

        private void OnClickSpinButton()
        {
            AudioManager.instance.PlaySound("LuckyWheel");
            currentPrize = GetPrize();
            Debug.Log(currentPrize.type + "    " + currentPrize.amount);
            RotateWheel(currentPrize);
            switch (currentPrize.type)
            {
                case PrizeType.GOLD:
                    Icon.sprite = coinSprite;
                    break;
                case PrizeType.HELICOPTER:
                    Icon.sprite = helicopterSprite;
                    break;
                case PrizeType.HINT:
                    Icon.sprite = hintSprite;
                    break;
                default:
                    Icon.sprite = null;
                    break;
            }

            amountPrizeTxt.text = currentPrize.amount.ToString();
        }

        public Prize GetPrize()
        {
            //  float totalWeight = prizes.Sum(prize => prize.weight);
            float totalWeight = 0;

            foreach (Prize prize in prizeList)
            {
                totalWeight += prize.weight;
            }

            float RandomValue = UnityEngine.Random.Range(0f, totalWeight);

            float currentWieght = 0f;
            foreach (Prize prize in prizeList)
            {
                currentWieght += prize.weight;
                if (RandomValue <= currentWieght)
                {
                    return prize;
                }
            }

            return null;
        }

        [Header("Lucky Wheel elements : ")] public GameObject wheel;
        public float finalAngle;

        public void RotateWheel(Prize prize)
        {
            finalAngle = 360 / 8 * prize.id + UnityEngine.Random.Range(0, 45);
            wheel.transform
                .DORotate(new Vector3(0, 0, finalAngle + 360 * 7), 7f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart)
                .OnComplete(() => { DOVirtual.DelayedCall(1f, () => ShowPopupPrize(prize)); });
        }

        public void ShowPopupPrize(Prize prize)
        {
            PanelGetPrize.SetActive(true);
            getPrizeAnimator.Play("Show", 0, 0);
        }

        public void SetPrizeGeted(Prize prize, int multiple)
        {
            if (prize != null)
            {
                switch (prize.type)
                {
                    case PrizeType.GOLD:
                        float golds = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
                        Debug.Log(golds);
                        golds += prize.amount;
                        PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, golds);
                        Debug.Log(golds);
                        Observer.Notify(EventAction.EVENT_UPDATE_COIN, null);
                        break;
                    case PrizeType.HELICOPTER:
                        int currenHelicopter = PlayerPrefs.GetInt(PlayerPrefsManager.helicopterAmount);
                        currenHelicopter += (int)prize.amount;
                        PlayerPrefs.SetInt(PlayerPrefsManager.helicopterAmount, currenHelicopter);
                        break;
                    case PrizeType.HINT:
                        int currenHint = PlayerPrefs.GetInt(PlayerPrefsManager.hintAmount);
                        currenHint += (int)prize.amount;
                        PlayerPrefs.SetInt(PlayerPrefsManager.hintAmount, currenHint);
                        break;
                }

                prize = null;
            }
            else
            {
                Debug.LogError("Prize not found");
            }
        }
    }
}