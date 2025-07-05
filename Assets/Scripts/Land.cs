using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class Land : MonoBehaviour
    {
        public Material landDefaultMat;
        public Material land1Mat, land2Mat, land3Mat, land4Mat, land5Mat, land6Mat, land7Mat;
        public MeshRenderer landMeshRenderer;

        private void Start()
        {
            SetLand(PlayerPrefs.GetInt(PlayerPrefsManager.land));
        }

        private void SetLand(int index)
        {
            if (index == 0) landMeshRenderer.material = landDefaultMat;
            else
            {
                landMeshRenderer.material = Resources.Load<Material>("Materials/Land " + index);
            }
        }
    }
}