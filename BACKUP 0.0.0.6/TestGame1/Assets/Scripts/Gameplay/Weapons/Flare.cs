using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Weapons
{
    public class Flare : MonoBehaviour
    {
        private float time;
        private float timeKill = 0.01f;
        private bool latch = false;
        public void Update()
        {
            if (latch == true)
            {
                if (time >= timeKill)
                    Destroy(gameObject);
                else
                    time += Time.deltaTime;
            }
            else
                latch = true;
        }

        public void Awake()
        {
            transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360)));
        }
    }
}
