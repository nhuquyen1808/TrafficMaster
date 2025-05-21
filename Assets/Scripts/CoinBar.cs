using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class CoinBar : MonoBehaviour
    {
        public  TextMeshProUGUI  coinText;
        // Start is called before the first frame update
        void Start()
        {
            coinText.text = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin).ToString();
            Observer.AddObserver(EventAction.EVENT_UPDATE_COIN,UpdateCoin);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_UPDATE_COIN,UpdateCoin);
        }

        private void UpdateCoin(object obj)
        {
            float coinValue = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
            coinText.text = coinValue.ToString();               
        }
    }
}
