using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class GlobalAssigner : MonoBehaviour
    {
        public GameObject QuadPrefab;

        // Use this for initialization
        void Start()
        {
            Global.QuadPrefab = QuadPrefab;
            Global.GlobalObject = gameObject;
        }
    }
}
