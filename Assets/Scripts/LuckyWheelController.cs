using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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
        public int amount;
        
    }
    public class LuckyWheelController : MonoBehaviour
    {
       public List<Prize> prizeList = new List<Prize>();
       public Button SpinButton,SpinButtonAd, GetButton,GetButtonAd;
     //  public GameObject gameObj1, gameObj2;

       [SerializeField] private GameObject PanelGetPrize;
       [SerializeField] Animator getPrizeAnimator;
       public Button closeButton;
       private void Awake()
       {
           SpinButton.onClick.AddListener(OnClickSpinButton);
           SpinButtonAd.onClick.AddListener(OnClickSpinAdButton);
           GetButton.onClick.AddListener(OnClickGetButton);
           GetButtonAd.onClick.AddListener(OnClickGetButtonAd);
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
       }

       private void OnClickGetButton()
       {
           Debug.Log("Get prize");
           PanelGetPrize.SetActive(false);
           //gameObj1.GetComponent<RectTransform>().DOMove(gameObj2.GetComponent<RectTransform>().position, 1.2f).SetEase(Ease.InBack);
       }

       private void OnClickSpinButton()
       {
           var prize = GetPrize();
           Debug.Log(prize.type +"    " + prize.amount );
           RotateWheel(prize);

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
               currentWieght+= prize.weight;
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
           finalAngle = 360/8 * prize.id + UnityEngine.Random.Range(0, 45);
           wheel.transform
               .DORotate(new Vector3(0, 0, finalAngle + 360 * 5), 5f, RotateMode.FastBeyond360)
               .SetEase(Ease.OutQuart)
               .OnComplete(() =>
               {
                   DOVirtual.DelayedCall(1f, () =>ShowPopupPrize(prize));
               });
       }

       public void ShowPopupPrize(Prize prize)
       {
           // Setup panel here ???
           Debug.Log("Show Popup get Prize");
           PanelGetPrize.SetActive(true);
           getPrizeAnimator.Play("Show",0,0);
       }
    }
}

