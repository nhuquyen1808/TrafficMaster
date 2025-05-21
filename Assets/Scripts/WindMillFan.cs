using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class WindMillFan : MonoBehaviour
    {
        private float rotZ;

        public float speed;

        void Update()
        {
            rotZ += speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }
}