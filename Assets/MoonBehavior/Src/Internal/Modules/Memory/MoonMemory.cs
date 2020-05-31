using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

using MoonBehavior.Security;
using MoonBehavior.Memory.Serialization;

namespace MoonBehavior.Memory
{
    /// <summary>
    /// Runtime memory class
    /// </summary>
    public class MoonMemory : MonoBehaviour
    {
        /// <summary>
        /// Working memory table
        /// </summary>
        Hashtable memory = new Hashtable();

        /// <summary>
        /// User defined memory items
        /// </summary>
        [SerializeField]
        private List<MemoryItem> MemoryItems = new List<MemoryItem>();
        
        public void Init()
        {
            if (MemoryItems.Count <= 0) return;

            for (int i = 0; i < MemoryItems.Count; i++)
            {
                string currentKey = MemoryItems[i].key;

                if (!string.IsNullOrEmpty(currentKey))
                {

                    if (memory.ContainsKey(currentKey))
                    {
                        Debug.LogWarning("[MoonMemory] Duplicated key detected: '" + currentKey + "' ignoring it..");
                        continue;
                    }else
                    {
                        memory.Add(currentKey, MemoryItems[i].Value);
                    }
                }
                else
                {
                    Debug.LogWarning("[MoonMemory] Empty key detected ! ignoring it..");
                    continue;
                }
            }

            // clean user defined memory after adding it to working memory
            MemoryItems.Clear();
        }

        /// <summary>
        /// Returns a value from the memory based on the key and type
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Key to find the value</param>
        /// <param name="OnFailure">Called if the operation failed casting the value type to the generic type</param>
        /// <returns></returns>
        public T GetValue<T>(string key , System.Func<string,T> OnFailure = null)
        {
            try
            {
                return (T) memory[key];
            }catch
            {
                try
                {
                    if (OnFailure != null)
                        return OnFailure.Invoke(key);
                    else return default(T);
                }catch
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// sets a value to the memory
        /// </summary>
        /// <param name="key">Value key</param>
        /// <param name="value">Value</param>
        public void SetValue(string key , object value)
        {
            if (memory.ContainsKey(key))
            {
                memory[key] = value;
            }else
            {
                memory.Add(key, value);
            }
        }

        /// <summary>
        /// Remove a value from the memory
        /// </summary>
        /// <param name="key">Value key</param>
        public void RemoveValue(string key)
        {
            memory.Remove(key);
        }

        /// <summary>
        /// Clears the memory
        /// </summary>
        public void Clear()
        {
            memory.Clear();
        }
        
        /// <summary>
        /// Save the memory for later use.
        /// Note: it will not save GameObjects.
        /// </summary>
        /// <returns>Byte array memory data</returns>
        public byte[] Save()
        {
            Dictionary<string, object> MemoryData = new Dictionary<string, object>();

            string[] keys = new string[memory.Keys.Count];

            memory.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; i++)
            {
                string currentKey = keys[i];

                object currentVal = memory[currentKey];

                if (currentVal == null)
                {
                    continue;
                }else
                {
                    System.Type currtype = currentVal.GetType();

                    if (currtype == typeof(Color) || currtype == typeof(Color32))
                    {
                        Color col = (Color)currentVal;
                        MemoryData.Add(currentKey, new sColor(col.r, col.g, col.b, col.a));
                    }else if (currtype == typeof(Vector2))
                    {
                        Vector2 v2 = (Vector2)currentVal;
                        MemoryData.Add(currentKey, new sVector(v2.x, v2.y));
                    }else if (currtype == typeof(Vector3))
                    {
                        Vector3 v3 = (Vector3)currentVal;
                        MemoryData.Add(currentKey, new sVector(v3.x, v3.y, v3.z));
                    }else if (currtype == typeof(Vector4))
                    {
                        Vector4 v4 = (Vector4)currentVal;
                        MemoryData.Add(currentKey, new sVector(v4.x, v4.y, v4.z, v4.w));
                    }else if (currtype == typeof(LayerMask))
                    {
                        LayerMask mask = (LayerMask)currentVal;
                        MemoryData.Add(currentKey, new sLayerMask() { Value = mask.value });
                    }else if (currtype.BaseType == typeof(Object))
                    {
                        continue;
                    }else
                    {
                        object[] serdata = currentVal.GetType().GetCustomAttributes(typeof(System.SerializableAttribute), true);
                        if (serdata != null && serdata.Length > 0)
                        {
                            MemoryData.Add(currentKey, currentVal);
                        }
                    }
                }
            }

            MemoryStream mem = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(mem, MemoryData);

            byte[] encryptedData = MoonSecurity.AES_Encrypt(mem.ToArray());

            mem.Close();

            return encryptedData;
        }
        
        /// <summary>
        /// Load pre saved memory
        /// </summary>
        /// <param name="data">Memory data</param>
        /// <param name="Clear">Clear memory before load ?</param>
        public void Load(byte[] data , bool Clear = true)
        {
            byte[] decryptedData = MoonSecurity.AES_Decrypt(data);

            if (decryptedData == null || decryptedData.Length == 0) return;

            if (Clear) memory.Clear();

            MemoryStream mem = new MemoryStream(decryptedData);

            BinaryFormatter formatter = new BinaryFormatter();

            Dictionary<string, object> dic = (Dictionary<string, object>)formatter.Deserialize(mem);

            mem.Close();

            string[] keys = new string[dic.Keys.Count];

            dic.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; i++)
            {
                object currentValue;

                if (dic.TryGetValue(keys[i], out currentValue))
                {
                    System.Type etype = currentValue.GetType();

                    string ekey = keys[i];

                    if (etype == typeof(sColor))
                    {
                        Color col = (sColor)currentValue;
                        SetValue(ekey, col);
                    }else if (etype == typeof(sVector))
                    {
                        sVector svec = (sVector)currentValue;
                        switch(svec.Type)
                        {
                            case sVector.VectorType.Vector2:

                                Vector2 v2 = svec;

                                SetValue(ekey, v2);

                                break;

                            case sVector.VectorType.Vector3:

                                Vector3 v3 = svec;

                                SetValue(ekey, v3);

                                break;
                            case sVector.VectorType.Vector4:

                                Vector4 v4 = svec;

                                SetValue(ekey, v4);

                                break;
                        }
                    }else if (etype == typeof(sLayerMask))
                    {
                        LayerMask mask = (sLayerMask)currentValue;
                        SetValue(ekey, mask);
                    }else
                    {
                        SetValue(ekey, currentValue);
                    }
                }

            }
        }
    }
}