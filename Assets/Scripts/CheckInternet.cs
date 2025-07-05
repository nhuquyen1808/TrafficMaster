using UnityEngine;
using UnityEngine.UI;

public class CheckInternet : MonoBehaviour
{
    public Button checkButton;

    private void Awake()
    {
        checkButton.onClick.AddListener(CheckInternetButton);
    }

    private void CheckInternetButton()
    {
        /*if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            ManagerToast.instance.Show("Error. Check internet connection!");
        }
        else
        {
            ManagerToast.instance.Show("your device has internet connection!");
        }*/
    }
}
