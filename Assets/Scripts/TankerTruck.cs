using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class TankerTruck : MonoBehaviour
    {
        public ParticleSystem fireExplosion1, fireExplosion2;
        public GameObject tanker;
        
        public void PlayFireExplosion()
        {
            tanker.SetActive(false);
            Duck.PlayParticle(fireExplosion1);
            Duck.PlayParticle(fireExplosion2);
        }
    }
}
