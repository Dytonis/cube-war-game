using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Pointer : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                int mask = ~(1 << 9);
                RaycastHit hit;
                Physics.Raycast(new Ray(gameObject.transform.position, gameObject.transform.forward), out hit, maxDistance: 5f, layerMask: mask);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, Color.red, 10f);

                try
                {
                    Internal.ChunkVector v = Internal.ChunkConverter.ConvertWorldToChunkVector(hit.normal, hit.transform.gameObject, hit.point);
                
                if (hit.transform != null)
                    hit.transform.GetComponent<Chunk>().ReChunk(v);
                }
                catch { }
            }

            int mask2 = ~(1 << 9);
            RaycastHit hit2;
            Physics.Raycast(new Ray(gameObject.transform.position, gameObject.transform.forward), out hit2, maxDistance: 5f, layerMask: mask2);
            try
            {
                Internal.ChunkVector v2 = Internal.ChunkConverter.ConvertWorldToChunkVector(hit2.normal, hit2.transform.gameObject, hit2.point);
                Debug.DrawRay(new Vector3(v2.x, v2.y, v2.z), hit2.normal, Color.black, 0.1f);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, Color.red, 0.1f);
            } catch { }
        }
    }
}
