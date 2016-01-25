using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Internal
{
    [Serializable]
    public class ChunkFactory : ScriptableObject
    {
        public Mesh FuseToChunkMesh(MeshFilter ChunkMesh, MeshFilter NewMesh)
        {
            CombineInstance[] combine = new CombineInstance[2];
            List<MeshFilter> MeshList = new List<MeshFilter>();
            MeshList.Add(ChunkMesh);
            MeshList.Add(NewMesh);
            for (int i = 0; i < combine.Length; i++)
            {                
                combine[i].transform = MeshList[i].transform.localToWorldMatrix;
                Destroy(MeshList[i].gameObject);
            }

            ChunkMesh.sharedMesh.CombineMeshes(combine);

            return ChunkMesh.sharedMesh;
        }
        public Mesh FuseToChunkMesh(MeshFilter ChunkMesh, MeshFilter[] NewMeshes, Vector3 ChunkPos, bool Restart = true)
        {
            //store old positions
            foreach (MeshFilter filter in NewMeshes)
            {
                filter.transform.position -= ChunkPos;
            }
            ChunkMesh.transform.position = Vector3.zero;
            CombineInstance[] combine = new CombineInstance[NewMeshes.Length + 1];
            List<MeshFilter> MeshList = new List<MeshFilter>(NewMeshes);
            for (int i = 0; i < combine.Length - 1; i++)
            {
                combine[i].mesh = MeshList[i].sharedMesh;
                combine[i].transform = MeshList[i].transform.localToWorldMatrix;
                DestroyImmediate(MeshList[i].gameObject);
            }

            MeshList.Add(ChunkMesh);

            combine[NewMeshes.Length].mesh = MeshList[NewMeshes.Length].sharedMesh;
            combine[NewMeshes.Length].transform = MeshList[NewMeshes.Length].transform.localToWorldMatrix;

            Mesh m = new Mesh();
            m.CombineMeshes(combine, true, true);

            ChunkMesh.transform.position = ChunkPos;

            return m;
        }
    }
}
