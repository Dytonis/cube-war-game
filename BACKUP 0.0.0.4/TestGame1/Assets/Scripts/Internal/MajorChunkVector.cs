using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Internal
{
    [System.Serializable]
    public struct MajorChunkVector
    {
        [SerializeField]
        public byte x;
        [SerializeField]
        public byte y;
        [SerializeField]
        public byte z;

        [SerializeField]
        public MajorChunkVector(byte x, byte y, byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [SerializeField]
        public MajorChunkVector(int x, int y, int z)
        {
            this.x = (byte)x;
            this.y = (byte)y;
            this.z = (byte)z;
        }

        [SerializeField]
        public MajorChunkVector Above
        {
            get
            {
                if (y >= 5)
                    return new MajorChunkVector(x, (byte)5, z);
                else
                    return new MajorChunkVector(x, y + 1, z);
            }
        }

        [SerializeField]
        public MajorChunkVector Below
        {
            get
            {
                if (y <= 0)
                    return new MajorChunkVector(x, (byte)0, z);
                else
                    return new MajorChunkVector(x, y - 1, z);
            }
        }

        [SerializeField]
        public MajorChunkVector Right
        {
            get
            {
                if (x >= 255)
                    return new MajorChunkVector((byte)255, y, z);
                else
                    return new MajorChunkVector(x + 1, y, z);
            }
        }

        [SerializeField]
        public MajorChunkVector Left
        {
            get
            {
                if (x <= 0)
                    return new MajorChunkVector((byte)0, y, z);
                else
                    return new MajorChunkVector(x - 1, y, z);
            }
        }

        [SerializeField]
        public MajorChunkVector Front
        {
            get
            {
                if (z >= 255)
                    return new MajorChunkVector(x, y, (byte)255);
                else
                    return new MajorChunkVector(x, y, z + 1);
            }
        }

        [SerializeField]
        public MajorChunkVector Behind
        {
            get
            {
                if (z <= 0)
                    return new MajorChunkVector(x, y, (byte)0);
                else
                    return new MajorChunkVector(x, y, z - 1);
            }
        }

        public MajorChunkVector From(Vector3 direction)
        {
            return new MajorChunkVector((byte)(x + -direction.x), (byte)(y + -direction.y), (byte)(z + -direction.z));
        }
    }
}
