using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class RotateGlow : MonoBehaviour
    {
        [SerializeField] private float speed;
        private float rotZ;
        void Update()
        {
            rotZ += speed * Duck.TimeMod;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }
}
