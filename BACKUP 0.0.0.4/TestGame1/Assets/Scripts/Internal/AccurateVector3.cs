using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Internal
{
    [Serializable]
    public struct AccurateVector3
    {
        [SerializeField]
        private decimal _x;
        [SerializeField]
        private decimal _y;
        [SerializeField]
        private decimal _z;

        public decimal x
        {
            get
            {
                return Math.Round(_x, 1);
            }
        }
        public decimal y
        {
            get
            {
                return Math.Round(_y, 1);
            }
        }
        public decimal z
        {
            get
            {
                return Math.Round(_z, 1);
            }
        }

        public AccurateVector3(decimal x, decimal y, decimal z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}
