using UnityEngine;

namespace DevDuck
{
    public class CarSkinMatButton : LandMaterialButton
    {
        protected override void OnClickEquipButton()
        {
            if (STATE == State.DEFAULT || STATE == State.UNLOCKED || STATE == State.USING)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.land, Id);
                Observer.Notify(EventAction.EVENT_BUY_CARMAT, Id);
            }
            else if (STATE == State.USING)
            {
            }
            else if (STATE == State.LOCKED)
            {
                float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
                if (coin >= Price)
                {
                    coin -= Price;
                    PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);
                    STATE = State.UNLOCKED;
                    PlayerPrefs.SetInt($"CARMAT_{Id}", 1);
                    PlayerPrefs.SetInt(PlayerPrefsManager.carSkin, Id);
                    ownerText.text = "USING";
                    ownerText.gameObject.SetActive(true);
                    priceText.gameObject.SetActive(false);
                    Observer.Notify(EventAction.EVENT_UPDATE_COIN, Price);
                    Observer.Notify(EventAction.EVENT_BUY_CARMAT, Id);
                }
                else
                {
                    Handheld.Vibrate();
                }
            }
        }

        protected override void OnEnable()
        {
            PlayerPrefs.SetInt("CARMAT_0", 1);
            int type = PlayerPrefs.GetInt($"CARMAT_{Id}");
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
    }
}
