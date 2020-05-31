using UnityEngine;

namespace MoonBehavior.Memory.Serialization
{
    /// <summary>
    /// serializable layer mask
    /// </summary>
    [System.Serializable]
    public struct sLayerMask
    {
        public int Value;

        public sLayerMask(int value)
        {
            Value = value;
        }

        public static implicit operator LayerMask(sLayerMask mask)
        {
            return new LayerMask() { value = mask.Value };
        }
    }
}
