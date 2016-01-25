using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Internal
{
    [System.Serializable]
    public struct ChunkVector
    {
        [SerializeField]
        public byte x;
        [SerializeField]
        public byte y;
        [SerializeField]
        public byte z;

        [SerializeField]
        public ChunkVector(byte x, byte y, byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [SerializeField]
        public ChunkVector(int x, int y, int z)
        {
            this.x = (byte)x;
            this.y = (byte)y;
            this.z = (byte)z;
        }

        [SerializeField]
        public ChunkVector Above
        {
            get
            {
                if (y >= 15)
                    return new ChunkVector(x, (byte)15, z);
                else
                    return new ChunkVector(x, y + 1, z);
            }
        }

        [SerializeField]
        public ChunkVector Below
        {
            get
            {
                if(y <= 0)
                    return new ChunkVector(x, (byte)0, z);
                else
                    return new ChunkVector(x, y - 1, z);
            }
        }

        [SerializeField]
        public ChunkVector Right
        {
            get
            {
                if(x >= 15)
                    return new ChunkVector((byte)15, y, z);
                else
                    return new ChunkVector(x + 1, y, z);
            }
        }

        [SerializeField]
        public ChunkVector Left
        {
            get
            {
                if(x <= 0)
                    return new ChunkVector((byte)0, y, z);
                else
                    return new ChunkVector(x - 1, y, z);
            }
        }

        [SerializeField]
        public ChunkVector Front
        {
            get
            {
                if(z >= 15)
                    return new ChunkVector(x, y, (byte)15);
                else
                    return new ChunkVector(x, y, z + 1);
            }
        }

        [SerializeField]
        public ChunkVector Behind
        {
            get
            {
                if(z <= 0)
                    return new ChunkVector(x, y, (byte)0);
                else
                    return new ChunkVector(x, y, z - 1);
            }
        }   
        
        public int To4096Index
        {
            get
            {
                return x + 16 * (y + 16 * z);
            }
        }    
    }
}
