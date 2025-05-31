using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum State
    {
        DEFAULT = 0,
        USING = 1,
        LOCKED = 2,
        UNLOCKED = 3
    }

    public class LandMaterialButton : MonoBehaviour
    {
        public int Id, Price;
        public TextMeshProUGUI ownerText, priceText;
        public Image buttonImage;
        public State STATE;
        public Sprite ownerSprite, usingSprite, defaultSprite;
        public Button equipButton;


        private void Awake()
        {
            equipButton.onClick.AddListener(OnClickEquipButton);
        }

        protected virtual void OnClickEquipButton()
        {
            if (STATE == State.DEFAULT || STATE == State.UNLOCKED || STATE == State.USING)
            {
                Debug.Log("Equip material");
                PlayerPrefs.SetInt(PlayerPrefsManager.land, Id);
                Observer.Notify(EventAction.EVENT_BUY_LANDMAT, Id);
            }
            else if (STATE == State.USING)
            {
            }
            else if (STATE == State.LOCKED)
            {
                float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
                Debug.Log("Coin: " + coin);
                if (coin >= Price)
                {
                    Debug.Log("Buy material");
                    coin -= Price;
                    PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);
                    STATE = State.UNLOCKED;
                    PlayerPrefs.SetInt($"LANDMAT_{Id}", 1);
                    PlayerPrefs.SetInt(PlayerPrefsManager.land, Id);
                    ownerText.text = "USING";
                    ownerText.gameObject.SetActive(true);
                    priceText.gameObject.SetActive(false);
                    Observer.Notify(EventAction.EVENT_UPDATE_COIN, Price);
                    Observer.Notify(EventAction.EVENT_BUY_LANDMAT, Id);
                }
                else
                {
                    Handheld.Vibrate();
                }
            }
        }

        protected virtual void OnEnable()
        {
            PlayerPrefs.SetInt("LANDMAT_0", 1);
            int type = PlayerPrefs.GetInt($"LANDMAT_{Id}");
            if (type == 1)
            {
                STATE = State.UNLOCKED;
                SetUnLocked();
            }
            else if (type == 0)
            {
                STATE = State.LOCKED;
            }
            //
            if( PlayerPrefs.GetInt(PlayerPrefsManager.land) == Id)
            {
                STATE  = State.USING;
            }

            SetData(STATE);
        }

        public void SetData(State state)
        {
            switch (state)
            {
                case State.USING:
                    priceText.gameObject.SetActive(false);
                    ownerText.text = "USING";
                    buttonImage.sprite = usingSprite;

                    break;
                case State.UNLOCKED:
                    priceText.gameObject.SetActive(false);
                    ownerText.text = "EQUIP";
                    // ownerText.color = Color.white;
                    buttonImage.sprite = ownerSprite;
                    break;
                case State.LOCKED:
                    priceText.gameObject.SetActive(true);
                    ownerText.gameObject.SetActive(false);
                    priceText.text = Price.ToString();
                    buttonImage.sprite = defaultSprite;
                    break;
                default:
                    priceText.gameObject.SetActive(false);
                    ownerText.text = "EQUIP";
                    // ownerText.color = Color.white;
                    buttonImage.sprite = ownerSprite;
                    break;
            }
        }

        public void SetUnLocked()
        {
            priceText.gameObject.SetActive(false);
            ownerText.text = "EQUIP";
            // ownerText.color = Color.white;
            buttonImage.sprite = ownerSprite;
            ownerText.gameObject.SetActive(true);
        }

        public void SetUsing()
        {
            priceText.gameObject.SetActive(false);
            ownerText.text = "USING";
            buttonImage.sprite = usingSprite;
        }
    }
}