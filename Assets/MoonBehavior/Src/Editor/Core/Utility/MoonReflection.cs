using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

using MoonBehavior;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// Reflection Tool
    /// </summary>
    public class MoonReflection
    {
        /// <summary>
        /// Gets a type based on type name 
        /// </summary>
        /// <param name="typeName">Name</param>
        /// <returns>Type or null</returns>
        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        
        /// <summary>
        /// Returns all class tyoes form all assemblys based on the derived type
        /// </summary>
        /// <param name="DerivedType">Derived type</param>
        /// <returns>Class types array</returns>
        public static IEnumerable<Type> GetAllDerivedTypes(Type DerivedType)
        {
            Assembly[] allAsm = GetAssemblys();
            
            for (int i = 0; i < allAsm.Length; i++)
            {
                Assembly curr = allAsm[i];
                if (curr != null)
                {
                    List<Type> valids = GetAllDerivedTypes(curr, DerivedType);
                    if (valids.Count > 0)
                    {
                        foreach (Type t in valids) yield return t; 
                    }
                }
            }
        }

        /// <summary>
        /// Returns all class tyoes form an assemblys based on the derived type
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="derivedType">Derived type</param>
        /// <returns>Type list</returns>
        public static List<Type> GetAllDerivedTypes(Assembly assembly, Type derivedType)
        {

            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                    ).ToList();

        }

        public static Assembly[] GetAssemblys()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where((Assembly asm) =>
                asm.FullName.Contains("Assembly-CSharp")).ToArray();
        }
        
        /// <summary>
        /// Gets all field from an object that have allowed attributes
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>all fields from the object</returns>
        public static FieldInfo[] GetFields(object obj )
        {
            if (obj != null)
            {
                FieldInfo[] ff = GetAllFields(obj);

                List<FieldInfo> validfields = new List<FieldInfo>();
                

                for (int i = 0; i < ff.Length; i++)
                {
                    FieldInfo curr = ff[i];
                    bool Canadd = false;
                    
                    if (curr.IsPrivate)
                    {
                        var serfiledatt = GetFieldAttribute(curr, typeof(UnityEngine.SerializeField), true);
                        Canadd = serfiledatt != null;
                    }
                    else
                    {
                        Canadd = true; 
                    }

                    if (Canadd)
                    {
                        validfields.Add(curr);
                    }
                }
                return validfields.ToArray();
            }
            else return null;
        }


        /// <summary>
        /// Gets all field from an object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>all fields from the object</returns>
        public static FieldInfo[] GetAllFields(object obj)
        {
            return obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Finds Specific field form an object
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="obj">object</param>
        /// <returns>Field</returns>
        public static FieldInfo GetField(string name, object obj)
        {
            FieldInfo[] allf = GetAllFields(obj);
            for (int i = 0; i < allf.Length; i++)
            {
                if (allf[i].Name == name)
                {
                    return allf[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets clean node name from a node type
        /// </summary>
        /// <param name="Nodetype">Node type</param>
        /// <returns>clean node name</returns>
        public static InfoAttribute GetNodeData(Type Nodetype)
        {
            InfoAttribute infoatt = GetAttribute(Nodetype, typeof(InfoAttribute), true) as InfoAttribute;
            
            if (infoatt != null )
            {
                if (string.IsNullOrEmpty(infoatt.Name))
                {
                    infoatt.Name = UnityEditor.ObjectNames.NicifyVariableName(Nodetype.Name);
                }
            }else
            {
                infoatt = new InfoAttribute();
                infoatt.Name = Nodetype.Name;
            }

            return infoatt;
        }

        /// <summary>
        /// Gets an Attribute from a field
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="attributeType">Attribute to search</param>
        /// <param name="inherit">Inherit ?</param>
        /// <returns>Attribute found</returns>
        public static Attribute GetFieldAttribute(MemberInfo field, Type attributeType, bool inherit)
        {
            return Attribute.GetCustomAttribute(field, attributeType, inherit);
        }


        /// <summary>
        /// Gets an Attribute from a type
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="attributeType">Attribute type to search</param>
        /// <param name="Inherit">Inherit ?</param>
        /// <returns>Attribute found</returns>
        public static Attribute GetAttribute(Type t, Type attributeType, bool Inherit)
        {
            object[] atts = t.GetCustomAttributes(attributeType, Inherit);

            if (atts != null && atts.Length > 0) return (Attribute)atts[0];

            return null;
        }
    }
}
