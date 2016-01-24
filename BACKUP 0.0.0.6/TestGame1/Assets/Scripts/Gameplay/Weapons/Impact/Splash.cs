using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Weapons.Impact
{
    public class Splash : MonoBehaviour
    {
        public Debris DebrisPrefab;
        public float Scale;

        public void Deploy(Vector3 Normal, float Size)
        {
            float Adj = Size * Scale;
            float SizeAdj = Adj / 170;
            float speedAdj = Adj * 6;
            for(int i = 0; i < Adj / 5; i++)
            {
                Debris s = Instantiate(DebrisPrefab, transform.position, transform.rotation) as Debris;
                s.Lifespan = UnityEngine.Random.Range(SizeAdj * 5, SizeAdj * 9f);

                float size = UnityEngine.Random.Range(SizeAdj / 1.5f, SizeAdj);

                s.transform.localScale = new Vector3(size, size, size);

                Rigidbody r = s.GetComponent<Rigidbody>();
                r.AddForce(new Vector3(Normal.x * speedAdj, Normal.y * speedAdj, Normal.z * speedAdj)); //Upwards
                r.AddForce(new Vector3(UnityEngine.Random.Range(speedAdj / 2, SizeAdj), UnityEngine.Random.Range(speedAdj / 2, SizeAdj), UnityEngine.Random.Range(speedAdj / 2, SizeAdj))); //Random

                Debug.DrawRay(transform.position, Normal, Color.green, 10f);

                s.Initiate();
            }
        }
    }
}
