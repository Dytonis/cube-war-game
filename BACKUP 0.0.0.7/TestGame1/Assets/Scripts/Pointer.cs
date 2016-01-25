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
                int mask = LayerMask.GetMask("GroundMesh");
                RaycastHit hit;
                Physics.Raycast(new Ray(gameObject.transform.position, gameObject.transform.forward), out hit, maxDistance: 5f, layerMask: mask);

                if (hit.transform != null)
                {
                    Internal.ChunkVector v = Internal.ChunkConverter.ConvertWorldToChunkVector(hit.normal, hit.transform.gameObject, hit.point);
                    hit.transform.GetComponent<Chunk>().ReChunk(v);
                }
            }
        }
    }
}
