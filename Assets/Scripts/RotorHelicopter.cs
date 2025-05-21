using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum RotoType
    {
        UP_ROTO,
        BACK_ROTO
    }

    public class RotorHelicopter : MonoBehaviour
    {
        public int speed;
        public float rotY;
        public RotoType rotoType;

        void Update()
        {
            rotY += speed * 5 * Duck.TimeMod;
            if (rotoType == RotoType.UP_ROTO)
            {
                transform.rotation = Quaternion.Euler(0, rotY, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, rotY);
            }
        }
    }
}