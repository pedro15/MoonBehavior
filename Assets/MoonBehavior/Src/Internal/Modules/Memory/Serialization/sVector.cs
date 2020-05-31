using UnityEngine;

namespace MoonBehavior.Memory.Serialization
{
    /// <summary>
    /// Serializable Vector class
    /// </summary>
    [System.Serializable]
    public struct sVector
    {
        public float x, y, z, w;

        public enum VectorType
        {
            Vector2 , Vector3 , Vector4
        }

        public VectorType Type;
        
        public sVector ( float x , float y)
        {
            this.x = x;
            this.y = y;
            z = 0;
            w = 0;
            Type = VectorType.Vector2;
        }

        public sVector(float x , float y , float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 0;
            Type = VectorType.Vector3;
        }

        public sVector(float x , float y , float z , float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            Type = VectorType.Vector4;
        }

        public static implicit operator Vector2(sVector vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static implicit operator Vector3(sVector vec)
        {
            return new Vector3(vec.x, vec.y, vec.z);
        }

        public static implicit operator Vector4(sVector vec)
        {
            return new Vector4(vec.x, vec.y, vec.z , vec.w);
        }
    }
}
