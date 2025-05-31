using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// use : attach this script to Canvas
public class ManagerPopup : MonoBehaviour
{
    public static ManagerPopup Instance;

    private Dictionary<string, GameObject> popupPool = new Dictionary<string, GameObject>();
    //  private Transform popupRoot;
    public RectTransform canvasRoot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            canvasRoot = GetComponent<RectTransform>();
            /* popupRoot = new GameObject("PopupRoot").transform;
             popupRoot.transform.SetParent(canvasRoot, false);*/
            DontDestroyOnLoad(canvasRoot.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(string popupName)
    {
        if (popupPool.ContainsKey(popupName))
        {
            popupPool[popupName].GetComponent<BasePopup>()?.Open();
            Debug.Log("Has already exist just open");
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>($"Popups/{popupName}");
            if (prefab != null)
            {
                GameObject popup = Instantiate(prefab, canvasRoot);
                popup.name = popupName;
                popupPool[popupName] = popup;
                popup.GetComponent<BasePopup>()?.Open();
                Debug.Log("instantiate new popup");
            }
            else
            {
                Debug.LogError($"{popupName} not found");
            }
        }
    }

    /* public void HidePopup(string popupName)
     {
         if (popupPool.ContainsKey(popupName))
         {
             popupPool[popupName].GetComponent<BasePopup>()?.Close();
         }
     }

     public void HideAllPopups()
     {
         foreach (var popup in popupPool.Values)
         {
             popup.GetComponent<BasePopup>()?.Close();
         }
     }*/

    /* public T ShowPopup<T>(string popupName) where T : BasePopup
     {
         if (!popupPool.ContainsKey(popupName))
         {
             GameObject prefab = Resources.Load<GameObject>($"Popups/{popupName}");
             GameObject popup = Instantiate(prefab, canvasRoot);
             popup.name = popupName;
             popupPool[popupName] = popup;
         }

         var popupComponent = popupPool[popupName].GetComponent<T>();
         if (popupComponent == null)
 {
     Debug.LogError($"Popup '{popupName}' không có component kiểu {typeof(T).Name}");
     return null;
 }
 popupComponent.Open();
 return popupComponent;
     }*/

}
