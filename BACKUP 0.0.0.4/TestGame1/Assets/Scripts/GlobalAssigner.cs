using UnityEngine;
using System.Collections;
using Assets.Scripts.Mapping;

namespace Assets.Scripts
{
    public class GlobalAssigner : MonoBehaviour
    {
        public GameObject QuadPrefab;
        public GameObject Manager;

        // Use this for initialization
        void Start()
        {
            Global.QuadPrefab = QuadPrefab;
            Global.GlobalObject = gameObject;
            Global.Manager = Manager.GetComponent<ChunkManager>();
        }
    }
}
