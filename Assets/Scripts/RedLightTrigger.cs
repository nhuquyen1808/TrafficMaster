using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class RedLightTrigger : MonoBehaviour
    {
        [SerializeField] tagObject tagObject;

        private void OnTriggerEnter(Collider other)
        {
            Car car = other.GetComponent<Car>();
            if (car != null && car.isCheckRedLight)
            {
                if (tagObject.tagObj == "Red")
                {
                    Debug.Log("Hit Red");
                    Observer.Notify(EventAction.EVENT_CAR_HIT_REDLIGHT, 2);
                }
                else
                {
                    Debug.Log("Hit Yellow");
                    Observer.Notify(EventAction.EVENT_CAR_HIT_REDLIGHT, 1);
                }
            }
        }
    }
}