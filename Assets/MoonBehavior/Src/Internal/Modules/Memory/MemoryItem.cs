using UnityEngine;

namespace MoonBehavior.Memory
{
    /// <summary>
    /// MemoryItem data Types
    /// </summary>
    public enum ItemType : int
    {
        STRING = 0,
        FLOAT = 1,
        INT = 2,
        BOOLEAN = 3,
        VECTOR2 = 4,
        VECTOR3 = 5,
        VECTOR4 = 6,
        COLOR = 7,
        OBJECT = 8,
        LAYERMASK = 9
    }
    /// <summary>
    /// class that work with MoonMemory for Inspector based Memory elements
    /// </summary>
    [System.Serializable]
    public class MemoryItem : System.Object
    {
        /// <summary>
        /// Converts ItemType to corresponding System.Type
        /// </summary>
        /// <param name="t">ItemType</param>
        /// <returns>Corresponding System.Type</returns>
        public static System.Type ConvertType(ItemType t)
        {
            if (t == ItemType.STRING) return typeof(string);
            else if (t == ItemType.FLOAT) return typeof(float);
            else if (t == ItemType.INT) return typeof(int);
            else if (t == ItemType.BOOLEAN) return typeof(bool);
            else if (t == ItemType.VECTOR2) return typeof(Vector2);
            else if (t == ItemType.VECTOR3) return typeof(Vector3);
            else if (t == ItemType.VECTOR4) return typeof(Vector4);
            else if (t == ItemType.COLOR) return typeof(Color);
            else if (t == ItemType.OBJECT) return typeof(Object);
            else if (t == ItemType.LAYERMASK) return typeof(LayerMask);

            return null;
        }
        
        /// <summary>
        /// Current type
        /// </summary>
        [SerializeField]
        private ItemType type;

        public ItemType Type { get { return type; } }
        
        [SerializeField]
        private bool IsConstant = true;

        [SerializeField]
        private bool constantOnly;
        
        /// <summary>
        /// Are the element a constant ?
        /// </summary>
        public bool isConstant { get { return IsConstant; } }

        /// <summary>
        /// Stored predefined type
        /// </summary>
        [SerializeField]
        private bool _PredefinedType = false;

        /// <summary>
        /// Are the user defined the data type ?
        /// </summary>
        public bool PrefefinedType
        {
            get
            {
                return _PredefinedType;
            }
        }

        /// <summary>
        /// MoonMemory Key name
        /// </summary>
        public string key
        {
            get { return Key; }
        }

        // Stored data ...

        [SerializeField]
        private string Key;
        [SerializeField]
        private bool BoolValue = false;
        [SerializeField]
        private string StringValue;
        [SerializeField]
        private float floatValue;
        [SerializeField]
        private int intValue;
        [SerializeField]
        private Vector2 Vector2Value;
        [SerializeField]
        private Vector3 Vector3Value;
        [SerializeField]
        private Vector4 Vector4Value;
        [SerializeField]
        private Color ColorValue = Color.white;
        [SerializeField]
        private Object objectValue;
        [SerializeField]
        private LayerMask LayerValue;

        /// <summary>
        /// Value reference, don't use it directly , use GetValue Instead.
        /// </summary>
        public object Value
        {
            get
            {
                if (type == ItemType.COLOR)
                    return ColorValue;
                else if (type == ItemType.FLOAT)
                    return floatValue;
                else if (type == ItemType.INT)
                    return intValue;
                else if (type == ItemType.OBJECT)
                    return objectValue;
                else if (type == ItemType.STRING)
                    return StringValue;
                else if (type == ItemType.VECTOR2)
                    return Vector2Value;
                else if (type == ItemType.VECTOR3)
                    return Vector3Value;
                else if (type == ItemType.VECTOR4)
                    return Vector4Value;
                else if (type == ItemType.BOOLEAN)
                    return BoolValue;
                else if (type == ItemType.LAYERMASK)
                    return LayerValue;

                return null;
            }

            set
            {
                if (type == ItemType.COLOR)
                    ColorValue = (Color)value;
                else if (type == ItemType.FLOAT)
                    floatValue = (float)value;
                else if (type == ItemType.INT)
                    intValue = (int)value;
                else if (type == ItemType.OBJECT)
                    objectValue = (Object)value;
                else if (type == ItemType.STRING)
                    StringValue = (string)value;
                else if (type == ItemType.VECTOR2)
                    Vector2Value = (Vector2)value;
                else if (type == ItemType.VECTOR3)
                    Vector3Value = (Vector3)value;
                else if (type == ItemType.VECTOR4)
                    Vector4Value = (Vector4)value;
                else if (type == ItemType.BOOLEAN)
                    BoolValue = (bool)value;
                else if (type == ItemType.LAYERMASK)
                    LayerValue = (LayerMask)value;
            }
        }
        
        /// <summary>
        /// Returns the value of this Memory Element
        /// </summary>
        /// <typeparam name="T">Value type to get</typeparam>
        /// <param name="Memory">Memory component</param>
        /// <returns></returns>
        public T GetValue<T>(MoonMemory Memory)
        {
            if ((IsConstant | constantOnly) || string.IsNullOrEmpty(Key) || Memory == null)
            {
                try
                {
                    return (T)Value;
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                return Memory.GetValue<T>(Key);
            }
        }

        /// <summary>
        /// Memory item constructor
        /// </summary>
        /// <param name="type">Data type</param>
        public MemoryItem(ItemType type)
        {
            constantOnly = false;
            _PredefinedType = true;
            this.type = type;
        }

        /// <summary>
        /// Memory item constructor
        /// </summary>
        public MemoryItem(bool constantOnly = false)
        {
            this.constantOnly = constantOnly;
            _PredefinedType = false;
        }
        
        public override string ToString()
        {
            if (Value != null)
            {
                if (IsConstant)
                {
                    return Value.ToString();
                }
                else
                {
                    return Key + " (Memory)";
                }
            }
            return base.ToString();
        }
    }
}