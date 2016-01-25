using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Weapons.Effects
{
    public class Shell : MonoBehaviour
    {
        public Shell[] Subshells;
        float time;
        bool deploying = false;
        public void Deploy()
        {
            Destroy(gameObject, 4);
            deploying = true;

            foreach (Shell s in Subshells)
                s.Deploy();
        }

        public void Update()
        {
            if (deploying)
            {
                time += Time.deltaTime;

                if (time > 1.5)
                {
                    MeshRenderer r = GetComponent<MeshRenderer>();

                    Color c = new Color(r.material.color.r, r.material.color.g, r.material.color.b, Mathf.Lerp(1f, 0f, time - 1.5f));

                    GetComponent<MeshRenderer>().material.color = c;
                }
            }
        }
    }
}
