using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Internal;
using UnityEngine;

namespace Assets.Scripts
{
    public class Block : ScriptableObject
    {
        public Chunk ParentChunk;
        public List<int> TriangleIndexes = new List<int>();
        private byte _bt;
        public byte BlockType
        {
            get
            {
                return _bt;
            }
            set
            {
                _bt = value;
            }
        }
        public ChunkVector Position;

        public void ActiveUpdate(bool forcedResetCheck = false)
        {
            try
            {
                ParentChunk.BlockDatabase[Position.Above].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                ParentChunk.BlockDatabase[Position.Below].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                ParentChunk.BlockDatabase[Position.Front].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                ParentChunk.BlockDatabase[Position.Behind].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                ParentChunk.BlockDatabase[Position.Left].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                ParentChunk.BlockDatabase[Position.Right].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
        }

        public void SubactiveUpdate(ChunkVector OriginalPosition, bool focedResetCheck = false)
        {
            AccurateVector3 oldPos = ParentChunk.GetWorldPositionOfBlock(OriginalPosition);
            AccurateVector3 newPos = ParentChunk.GetWorldPositionOfBlock(Position);
            AccurateVector3 offset = new AccurateVector3((decimal)Math.Round((oldPos.x - newPos.x) / 2, 1), (decimal)Math.Round((oldPos.y - newPos.y) / 2, 1), (decimal)Math.Round((oldPos.z - newPos.z) / 2, 1));
            AccurateVector3 placePos = new AccurateVector3((decimal)Math.Round((oldPos.x - offset.x), 1), (decimal)Math.Round((oldPos.y - offset.y), 1), (decimal)Math.Round((oldPos.z - offset.z), 1));

            Vector3 vPP = new Vector3((float)placePos.x, (float)placePos.y, (float)placePos.z);

            if (BlockType != 0)
            {
                GameObject go = Instantiate(Global.QuadPrefab, vPP, Quaternion.Euler(Vector3.zero)) as GameObject;
                go.transform.LookAt(newPos.ToVector3());
                ParentChunk.MeshBuffer.Add(go.GetComponent<MeshFilter>());
            }
            else
            {
                if (focedResetCheck)
                {
                    RaycastHit hit;
                    Physics.Raycast(new Ray(newPos.ToVector3(), offset.ToVector3()), out hit, 2f, ~(1 >> 9));
                    if (hit.transform)
                    { 
                        Debug.Log("Found object!");
                        ParentChunk.RemoveFace(ParentChunk.GetComponent<MeshCollider>(), ParentChunk.GetComponent<MeshFilter>(), hit.triangleIndex);
                    }

                    Debug.DrawRay(newPos.ToVector3(), offset.ToVector3(), Color.green, 30f);
                    Debug.DrawRay(newPos.ToVector3(), offset.ToVector3() * 2, Color.blue, 5f);
                }
            }
        }
    }
}
