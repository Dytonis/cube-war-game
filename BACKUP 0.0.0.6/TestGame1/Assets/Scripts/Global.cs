using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Assets.Scripts.Mapping;

namespace Assets.Scripts
{
    [Serializable]
    public static class Global
    {
        public static GameObject QuadPrefab;
        public static GameObject AirSpace;
        public static GameObject GlobalObject;
        public static ChunkManager Manager;
    }
}
