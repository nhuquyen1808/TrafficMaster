using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class LandSkinShopController : MonoBehaviour
    {
        public List<LandMaterialButton>  landMaterialButtons = new List<LandMaterialButton>();

        private void Start()
        {
            Observer.AddObserver(EventAction.EVENT_BUY_LANDMAT,HandleBuyLandMaterial);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_BUY_LANDMAT,HandleBuyLandMaterial);
        }

        private void HandleBuyLandMaterial(object obj)
        {
           int id = (int)obj;
           for (int i = 0; i < landMaterialButtons.Count; i++)
           {
               if (landMaterialButtons[i].Id != id && landMaterialButtons[i].STATE == State.UNLOCKED || landMaterialButtons[i].STATE == State.USING)
               {
                   landMaterialButtons[i].SetUnLocked();
               }
               if (landMaterialButtons[i].Id == id )
               {
                   landMaterialButtons[i].SetUsing();
               }
           }
        }
    }
}
