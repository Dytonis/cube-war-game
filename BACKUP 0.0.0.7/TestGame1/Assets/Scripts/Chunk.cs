using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Internal;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Chunk : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        public Block[] BlockDatabase;
        public List<MeshFilter> MeshBuffer = new List<MeshFilter>();
        public GameObject Prefab;
        public MajorChunkVector ChunkPosition;

        public void Start()
        {
            Global.Manager.ChunkList.Add(ChunkPosition, this);
        }

        public void Create()
        {
            ChunkPosition.x = (byte)Mathf.RoundToInt(transform.position.x / 16);
            ChunkPosition.y = (byte)Mathf.RoundToInt(transform.position.y / 16);
            ChunkPosition.z = (byte)Mathf.RoundToInt(transform.position.z / 16);

            BlockDatabase = new Block[4096];

            for (int x = 0; x < 16; x++)
            {
                for(int z = 0; z < 16; z++)
                {
                    for(int y = 0; y < 16; y++)
                    {
                        Block b = new Block();
                        b.ParentChunk = this;
                        b.Position = new Internal.ChunkVector(x, y, z);
                        if(ChunkPosition.y >= 6)
                            b.BlockType = y < 10 ? (byte)1 : (byte)0;
                        else
                            b.BlockType = 1;
                        BlockDatabase[x + 16 * (y + 16 * z)] = b;                    
                    }
                }
            }

            Debug.Log(BlockDatabase.Length);

            GlobalUpdate();
            Fuse(MeshBuffer.ToArray());
            MeshBuffer.Clear();
        }

        public void Bridge(MajorChunkVector majorFromVector, Block fromBlock)
        {
            Debug.Log("Self Chunk: " + transform.name);
            Vector3 difference = new Vector3(ChunkPosition.x - majorFromVector.x, ChunkPosition.y - majorFromVector.y, ChunkPosition.z - majorFromVector.z);
            ChunkVector pos = new ChunkVector(Math.Abs(difference.x) >= 0.01 ? 15 - fromBlock.Position.x : fromBlock.Position.x, Math.Abs(difference.y) >= 0.01 ? 15 - fromBlock.Position.y : fromBlock.Position.y, Math.Abs(difference.z) >= 0.01 ? 15 - fromBlock.Position.z : fromBlock.Position.z);

            BlockDatabase[pos.To4096Index].BridgeUpdate(difference, fromBlock);

            Debug.Log("Bridge update! " + pos.x + "(" + fromBlock.Position.x + "), " + pos.y + "(" + fromBlock.Position.y + "), " + pos.z + "(" + fromBlock.Position.z + ")");
            Fuse(MeshBuffer.ToArray(), false);
            MeshBuffer.Clear();
        }

        public AccurateVector3 GetWorldPositionOfBlock(ChunkVector Position, bool add)
        {
            if (add)
            {
                return new AccurateVector3((decimal)transform.position.x + Position.x, (decimal)transform.position.y + Position.y, (decimal)transform.position.z + Position.z);
            }
            else
            {
                return new AccurateVector3((decimal)Position.x, (decimal)Position.y, (decimal)Position.z);
            }
        }

        public void GlobalUpdate()
        {
            int blocks = 1;
            foreach (Block b in BlockDatabase)
            {                
                if (b.BlockType == 0)
                {
                    blocks++;
                    b.ActiveUpdate();
                }
            }
            Debug.Log("Total blocks: " + blocks);
        }

        public void Fuse(MeshFilter[] AddedMeshes, bool Restart = true)
        {
            ChunkFactory factory = ScriptableObject.CreateInstance<ChunkFactory>();
            Mesh newMesh = factory.FuseToChunkMesh(GetComponent<MeshFilter>(), AddedMeshes, transform.position, Restart);

            GetComponent<MeshFilter>().sharedMesh = newMesh;
            GetComponent<MeshCollider>().sharedMesh = newMesh;
        }

        public void ReChunk(ChunkVector Block)
        {
            BlockDatabase[Block.To4096Index].BlockType = 0;
            BlockDatabase[Block.To4096Index].ActiveUpdate(true);
            Fuse(MeshBuffer.ToArray(), false);
            MeshBuffer.Clear();
        }

        public void RemoveFace(MeshCollider mc, MeshFilter mf, int tIndex)
        {
            List<int> newTris = new List<int>(mc.sharedMesh.triangles);
            Mesh newMesh = new Mesh();
            newMesh.vertices = mc.sharedMesh.vertices;
            if (tIndex % 2 == 0)
            {
                newTris.RemoveRange(tIndex * 3, 6);
            }
            else
            {
                newTris.RemoveRange(tIndex * 3 - 3, 6);
            }
            newMesh.triangles = newTris.ToArray();

            mc.sharedMesh = newMesh;
            mf.sharedMesh.triangles = newTris.ToArray();

        }
    }
}
