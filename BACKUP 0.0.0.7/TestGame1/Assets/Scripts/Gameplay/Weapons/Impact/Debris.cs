using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Weapons.Impact
{
    public class Debris : MonoBehaviour
    {
        public float Lifespan = 0f;

        public void Initiate()
        {
            Destroy(gameObject, Lifespan);
        }
    }
}
