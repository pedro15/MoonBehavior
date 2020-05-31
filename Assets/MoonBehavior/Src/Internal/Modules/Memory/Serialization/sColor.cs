using UnityEngine;

namespace MoonBehavior.Memory.Serialization
{
    /// <summary>
    /// serializable color class
    /// </summary>
    [System.Serializable]
    public struct sColor
    {
        public float r, g, b, a;

        public sColor(float r , float g , float b )
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 1f; 
        }

        public sColor (float r , float g , float b , float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Color ( sColor col )
        {
            return new Color(col.r, col.g, col.b, col.a);
        }
    }
}
