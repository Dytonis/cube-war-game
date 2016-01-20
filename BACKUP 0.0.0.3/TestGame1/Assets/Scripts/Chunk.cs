using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Internal;

namespace Assets.Scripts
{
    public class Chunk : MonoBehaviour
    {
        public Dictionary<ChunkVector, Block> BlockDatabase = new Dictionary<ChunkVector, Block>();
        public List<MeshFilter> MeshBuffer = new List<MeshFilter>();

        public void Start()
        {
            for(int x = 0; x < 16; x++)
            {
                for(int z = 0; z < 16; z++)
                {
                    for(int y = 0; y < 16; y++)
                    {
                        Block b = ScriptableObject.CreateInstance<Block>();
                        b.ParentChunk = this;
                        b.Position = new Internal.ChunkVector(x, y, z);
                        b.BlockType = y < 10 ? (byte)1 : (byte)0;
                        BlockDatabase.Add(new ChunkVector(x, y, z), b);                    
                    }
                }
            }

            BlockDatabase[new ChunkVector(3, 9, 3)].BlockType = 0;
            BlockDatabase[new ChunkVector(3, 8, 3)].BlockType = 0;
            BlockDatabase[new ChunkVector(3, 9, 2)].BlockType = 0;
            BlockDatabase[new ChunkVector(2, 9, 3)].BlockType = 0;
            BlockDatabase[new ChunkVector(3, 8, 4)].BlockType = 0;
            BlockDatabase[new ChunkVector(3, 7, 3)].BlockType = 0;
            BlockDatabase[new ChunkVector(3, 7, 4)].BlockType = 0;

            GlobalUpdate();
            Fuse(MeshBuffer.ToArray());
            MeshBuffer.Clear();
        }

        public AccurateVector3 GetWorldPositionOfBlock(ChunkVector Position)
        {
            return new AccurateVector3((decimal)transform.position.x + Position.x, (decimal)transform.position.y + Position.y, (decimal)transform.position.z + Position.z);
        }

        public void GlobalUpdate()
        {
            int blocks = 1;
            foreach (KeyValuePair<ChunkVector, Block> b in BlockDatabase)
            {                
                if (b.Value.BlockType == 0)
                {
                    blocks++;
                    b.Value.ActiveUpdate();
                }
            }
            Debug.Log("Total blocks: " + blocks);
        }

        public void Fuse(MeshFilter[] AddedMeshes, bool Restart = true)
        {
            ChunkFactory factory = new ChunkFactory();
            Mesh newMesh = factory.FuseToChunkMesh(GetComponent<MeshFilter>(), AddedMeshes, Restart);

            GetComponent<MeshFilter>().sharedMesh = newMesh;
            GetComponent<MeshCollider>().sharedMesh = newMesh;
        }

        public void ReChunk(ChunkVector Block)
        {
            BlockDatabase[Block].BlockType = 0;
            BlockDatabase[Block].ActiveUpdate(true);
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
