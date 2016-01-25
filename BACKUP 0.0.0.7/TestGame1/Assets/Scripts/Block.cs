using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Internal;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Block
    {
        [SerializeField]
        public Chunk ParentChunk;
        [SerializeField]
        private byte _bt;
        [SerializeField]
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
        [SerializeField]
        public ChunkVector Position;

        public void ActiveUpdate(bool forcedResetCheck = false)
        {
            try
            {
                if(Position.y >= 15)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Above].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Above.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                if (Position.y <= 0)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Below].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Below.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                if (Position.z >= 15)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Front].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Front.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                if (Position.z <= 0)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Behind].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Behind.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                if (Position.x >= 15)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Right].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Right.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
            try
            {
                if (Position.x <= 0)
                {
                    Global.Manager.ChunkList[ParentChunk.ChunkPosition.Left].Bridge(ParentChunk.ChunkPosition, this);
                }
                else
                    ParentChunk.BlockDatabase[Position.Left.To4096Index].SubactiveUpdate(Position, forcedResetCheck);
            }
            catch { }
        }

        public void BridgeUpdate(Vector3 Direction, Block OtherBlock)
        {
            AccurateVector3 oldPos = ParentChunk.GetWorldPositionOfBlock(OtherBlock.Position, true);
            AccurateVector3 newPos = ParentChunk.GetWorldPositionOfBlock(Position, true);
            AccurateVector3 offset = new AccurateVector3(newPos.x + -(decimal)(Direction.x / 2), newPos.y + -(decimal)(Direction.y / 2), newPos.z + -(decimal)(Direction.z / 2));
            if (BlockType != 0)
            {              
                GameObject go = GameObject.Instantiate(ParentChunk.Prefab, offset.ToVector3(), Quaternion.Euler(Vector3.zero)) as GameObject;
                go.transform.LookAt(oldPos.ToVector3());
                ParentChunk.MeshBuffer.Add(go.GetComponent<MeshFilter>());
                Debug.Log("Parent chunk: " + ParentChunk.transform.name);
            }
            else
            {
                RaycastHit hit;
                int layerMask = LayerMask.GetMask("GroundMesh");
                Physics.Raycast(new Ray(newPos.ToVector3(), -(Direction / 1.8f)), out hit, 2f, layerMask);
                if (hit.transform)
                {
                    Debug.Log("HIT: " + hit.transform.name);
                    hit.transform.GetComponent<Chunk>().RemoveFace(OtherBlock.ParentChunk.GetComponent<MeshCollider>(), OtherBlock.ParentChunk.GetComponent<MeshFilter>(), hit.triangleIndex);
                }

                Debug.DrawRay(newPos.ToVector3(), -(Direction / 1.8f), Color.green, 30f);
                Debug.DrawRay(newPos.ToVector3(), -(Direction / 1.8f) * 2, Color.blue, 5f);
            }
        }

        public void SubactiveUpdate(ChunkVector OriginalPosition, bool focedResetCheck = false)
        {
            AccurateVector3 oldPos = ParentChunk.GetWorldPositionOfBlock(OriginalPosition, true);
            AccurateVector3 newPos = ParentChunk.GetWorldPositionOfBlock(Position, true);
            AccurateVector3 offset = new AccurateVector3((decimal)Math.Round((oldPos.x - newPos.x) / 2, 1), (decimal)Math.Round((oldPos.y - newPos.y) / 2, 1), (decimal)Math.Round((oldPos.z - newPos.z) / 2, 1));
            AccurateVector3 placePos = new AccurateVector3((decimal)Math.Round((oldPos.x - offset.x), 1), (decimal)Math.Round((oldPos.y - offset.y), 1), (decimal)Math.Round((oldPos.z - offset.z), 1));

            Vector3 vPP = new Vector3((float)placePos.x, (float)placePos.y, (float)placePos.z);

            if (BlockType != 0)
            {
                GameObject go = GameObject.Instantiate(ParentChunk.Prefab, vPP, Quaternion.Euler(Vector3.zero)) as GameObject;
                go.transform.LookAt(newPos.ToVector3());
                ParentChunk.MeshBuffer.Add(go.GetComponent<MeshFilter>());
            }
            else
            {
                if (focedResetCheck)
                {
                    RaycastHit hit;
                    int layerMask = LayerMask.GetMask("GroundMesh");
                    Physics.Raycast(new Ray(newPos.ToVector3(), offset.ToVector3()), out hit, 2f, layerMask);
                    if (hit.transform)
                    { 
                        ParentChunk.RemoveFace(ParentChunk.GetComponent<MeshCollider>(), ParentChunk.GetComponent<MeshFilter>(), hit.triangleIndex);
                    }

                    Debug.DrawRay(newPos.ToVector3(), offset.ToVector3(), Color.green, 30f);
                    Debug.DrawRay(newPos.ToVector3(), offset.ToVector3() * 2, Color.blue, 5f);
                }
            }
        }
    }
}
