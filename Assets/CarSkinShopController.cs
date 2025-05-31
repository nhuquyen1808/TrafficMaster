using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    public class CarSkinShopController : MonoBehaviour
    {
        [FormerlySerializedAs("landMaterialButtons")] public List<CarSkinMatButton>  carMaterialButtons = new List<CarSkinMatButton>();

        private void Start()
        {
            Observer.AddObserver(EventAction.EVENT_BUY_CARMAT,HandleBuyCarSkinMaterial);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_BUY_CARMAT,HandleBuyCarSkinMaterial);
        }

        private void HandleBuyCarSkinMaterial(object obj)
        {
            int id = (int)obj;
            for (int i = 0; i < carMaterialButtons.Count; i++)
            {
                if (carMaterialButtons[i].Id != id && carMaterialButtons[i].STATE == State.UNLOCKED || carMaterialButtons[i].STATE == State.USING)
                {
                    carMaterialButtons[i].SetUnLocked();
                }
                if (carMaterialButtons[i].Id == id )
                {
                    carMaterialButtons[i].SetUsing();
                }
            }
        }
    }
}
