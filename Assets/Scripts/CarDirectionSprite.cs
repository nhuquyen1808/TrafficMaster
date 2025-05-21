using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CarDirectionSprite : MonoBehaviour
    {
        public SpriteRenderer sprDirection;
        public Car car;
        public Sprite oneDirection, upRight, upLeft, threeDirection;

        private void Start()
        {
           // SetSpriteDirection();
        }

        void SetSpriteDirection()
        {
            int amountDirection = car.ListDirection.Count;
            switch (amountDirection)
            {
                case 1:
                    sprDirection.sprite = oneDirection;
                    break;
                case 2 :
                    string direction = car.ListDirection[0].ToString() + car.ListDirection[1].ToString();
                    if (car.ListDirection[1].ToString() == "LEFT")
                    {
                        sprDirection.sprite = upLeft;
                    }
                    else
                    {
                        sprDirection.sprite = upRight;
                    }
                    break;
                case 3 :
                    break;
            }
        }
    }
}
