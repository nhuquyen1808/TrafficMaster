using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class Loadingbar : MonoBehaviour
    {
        private void Start()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD )== 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD, 1);
                PlayerPrefs.SetFloat(PlayerPrefsManager.Coin,200);
            }
        }

        public void LoadSceneHome()
        {
            ManagerScene.ins.LoadScene("SceneHome");
        }
    }
}
