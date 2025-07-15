using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class EffectGetCoin : MonoBehaviour
    {
        [SerializeField] GameObject coinParent;
        public Vector3[] initialPos;
        public Quaternion[] initialRotation;
        public RectTransform startPosition, destination;
        int coinNum = 10;
        public Vector3 des, originPos;
        public TextMeshProUGUI coinText;

        private void Start()
        {
            SetStartPosition();
        }

        public virtual void SetStartPosition()
        {
            initialPos = new Vector3[coinNum];
            initialRotation = new Quaternion[coinNum];
            originPos = startPosition.GetComponent<RectTransform>().localPosition;
            des = destination.GetComponent<RectTransform>().position;

            coinParent.GetComponent<RectTransform>().localPosition = originPos;
            for (int i = 0; i < coinParent.transform.childCount; i++)
            {
                initialPos[i] = coinParent.transform.GetChild(i).position;
                initialRotation[i] = coinParent.transform.GetChild(i).rotation;
            }
        }

        protected virtual void Reset()
        {
            for (int i = 0; i < coinParent.transform.childCount; i++)
            {
                coinParent.transform.GetChild(i).GetComponent<RectTransform>().position = initialPos[i];
                coinParent.transform.GetChild(i).rotation = initialRotation[i];
                coinParent.transform.GetChild(i).localScale = Vector3.zero;
                coinParent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public virtual void GetReward(float coinGet, int amountImageShow, Action action)
        {
            Reset();
            des = destination.GetComponent<RectTransform>().position;
            int count = 0;
            for (int i = 0; i < coinParent.transform.childCount / 2; i++)
            {
                var a = i;
                coinParent.transform.GetChild(a).DOScale(1, 0.3f).SetDelay(a * 0.01f).SetEase(Ease.OutBack);
                coinParent.transform.GetChild(a).GetComponent<RectTransform>().DOMove(des, 0.5f)
                    .SetDelay(a * 0.1f + 0.25f)
                    .SetEase(Ease.InBack);
                coinParent.transform.GetChild(a).DOScale(0.9f, 0.5f).SetDelay(a * 0.1f + 0.25f).OnComplete((() =>
                {
                    coinParent.transform.GetChild(a).gameObject.SetActive(false);
                }));
            }

            for (int i = coinParent.transform.childCount / 2; i < coinParent.transform.childCount; i++)
            {
                var a = i;
                coinParent.transform.GetChild(a).DOScale(1, 0.3f).SetDelay(a * 0.01f).SetEase(Ease.OutBack);
                coinParent.transform.GetChild(a).DOScale(0.9f, 0.5f).SetDelay(a * 0.1f + 0.35f);
                coinParent.transform.GetChild(a).GetComponent<RectTransform>().DOMove(des, 0.5f)
                    .SetDelay(a * (0.05f) + 0.35f)
                    .SetEase(Ease.InBack).OnComplete((() =>
                    {
                        count++;
                        coinParent.transform.GetChild(a).gameObject.SetActive(false);
                        ShowCoinText(coinGet, amountImageShow);
                        if (count >= 5)
                        {
                            action?.Invoke();
                        }
                    }));
            }
        }

        private void ShowCoinText(float coinGet, int amountImageShow)
        {
            PlayerPrefs.SetFloat(PlayerPrefsManager.Coin,
                PlayerPrefs.GetFloat(PlayerPrefsManager.Coin) + (2 * coinGet) / amountImageShow);
            coinText.text = Mathf.Round(PlayerPrefs.GetFloat(PlayerPrefsManager.Coin)).ToString();
        }
    }
}