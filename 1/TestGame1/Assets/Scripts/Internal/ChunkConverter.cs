using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Internal
{
    public static class ChunkConverter
    {
        public static ChunkVector ConvertWorldToChunkVector(Vector3 normal, GameObject chunk, Vector3 world)
        {
            Vector3 offset = world - chunk.transform.position;

            Vector3 v = new Vector3((float)Math.Round((offset.x + 0.5f) % 16f, 3), (float)Math.Round((offset.y + 0.5f) % 16, 3), (float)Math.Round((offset.z + 0.5f) % 16, 3));
            v -= normal.normalized / 2;

            Debug.Log(" x: " + v.x + ", y: " + v.y + ", z: " + v.z);

            ChunkVector c = new ChunkVector((byte)(v.x), (byte)(v.y), (byte)(v.z));

            return c;
        }
    }
}
