using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CarColor : MonoBehaviour
    {
        public Material blackMaterial,blueMaterial, darkblueMaterial,
            brownMaterial, GrayMaterial, greenMaterial,orangeMaterial,pinkMaterial, redMaterial,
            turquoiseMaterial, violetMaterial, whiteMaterial, yellowMaterial;

        public MeshRenderer meshRenderer;

        private void Start()
        {
            SetColor(PlayerPrefs.GetInt(PlayerPrefsManager.carSkin));
        }

        private void Awake()
        {
            Observer.AddObserver(EventAction.EVENT_BUY_CARMAT, ChangeCarColor);
            
        }
        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_BUY_CARMAT, ChangeCarColor);
        }
        private void ChangeCarColor(object obj)
        {
            int idSelected = (int)obj;
            PlayerPrefs.SetInt(PlayerPrefsManager.carSkin, idSelected);
            SetColor(idSelected);
            
        }

        private void SetColor(int idSelected)
        {
            switch (idSelected)
            {
                case 0:
                    meshRenderer.material = yellowMaterial;
                    break;
                case 1:
                    meshRenderer.material = blackMaterial;
                    break;
                case 2:
                    meshRenderer.material = blueMaterial;
                    break;
                case 3:
                    meshRenderer.material = darkblueMaterial;
                    break;
                case 4:
                    meshRenderer.material = brownMaterial;
                    break;
                case 5:
                    meshRenderer.material = GrayMaterial;
                    break;
                case 6:
                    meshRenderer.material = greenMaterial;
                    break;
                case 7:
                    meshRenderer.material = orangeMaterial;
                    break;
                case 8:
                    meshRenderer.material = pinkMaterial;
                    break;
                case 9:
                    meshRenderer.material = redMaterial;
                    break;
                case 10:
                    meshRenderer.material = turquoiseMaterial;
                    break;
                case 11:
                    meshRenderer.material = violetMaterial;
                    break;
                case 12:
                    meshRenderer.material = whiteMaterial;
                    break;
            }
        }
    }
}