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
            if (car != null )
            {
                if (tagObject.tagObj == "Red")
                {
                    
                Debug.Log("Hit Red");
                }
                else
                {
                    Debug.Log("Hit Yellow");
                }
            }
        }
        
    }
}
