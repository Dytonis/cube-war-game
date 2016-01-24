using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Internal;

namespace Assets.Scripts.Mapping
{
    [System.Serializable]
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField]
        public Dictionary<MajorChunkVector, Chunk> ChunkList = new Dictionary<MajorChunkVector, Chunk>();
    }
}
