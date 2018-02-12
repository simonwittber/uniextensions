using System;
using UnityEngine;

/// <summary>
/// A Fixed list is simlar to a regular generic list, except that it is of a fixed size, and you can
/// access the internal array of values via the .Buffer property.
/// </summary>

namespace DifferentMethods.Extensions.Serialization
{
    [Serializable]
    public class PolySerializer
    {
        [System.Serializable] class ObjectMap : SerializableDictionary<string, UnityEngine.Object> { }
        [System.Serializable] class FloatMap : SerializableDictionary<string, float> { }
        [System.Serializable] class IntMap : SerializableDictionary<string, int> { }
        [System.Serializable] class StringMap : SerializableDictionary<string, string> { }
        [System.Serializable] class BoolMap : SerializableDictionary<string, bool> { }
        [System.Serializable] class EnumMap : SerializableDictionary<string, System.Enum> { }
        [System.Serializable] class Vector3Map : SerializableDictionary<string, Vector3> { }
        [System.Serializable] class Vector2Map : SerializableDictionary<string, Vector2> { }
        [System.Serializable] class Vector4Map : SerializableDictionary<string, Vector4> { }
        [System.Serializable] class ColorMap : SerializableDictionary<string, Color> { }
        [System.Serializable] class LayerMaskMap : SerializableDictionary<string, LayerMask> { }

        [SerializeField] ObjectMap objects = new ObjectMap();
        [SerializeField] FloatMap floats = new FloatMap();
        [SerializeField] IntMap ints = new IntMap();
        [SerializeField] StringMap strings = new StringMap();
        [SerializeField] BoolMap bools = new BoolMap();
        [SerializeField] EnumMap enums = new EnumMap();
        [SerializeField] Vector3Map vector3s = new Vector3Map();
        [SerializeField] Vector2Map vector2s = new Vector2Map();
        [SerializeField] Vector4Map vector4s = new Vector4Map();
        [SerializeField] ColorMap colors = new ColorMap();
        [SerializeField] LayerMaskMap layerMasks = new LayerMaskMap();

        public void Set(string name, object obj)
        {
            var type = obj.GetType();
            if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
                objects.Set(name, obj);
            else if (type == typeof(float))
                floats.Set(name, obj);
            else if (type == typeof(int))
                ints.Set(name, obj);
            else if (type == typeof(string))
                strings.Set(name, obj);
            else if (type == typeof(bool))
                bools.Set(name, obj);
            else if (type.IsSubclassOf(typeof(System.Enum)))
                enums.Set(name, obj);
            else if (type == typeof(Vector3))
                vector3s.Set(name, obj);
            else if (type == typeof(Vector2))
                vector2s.Set(name, obj);
            else if (type == typeof(Vector4))
                vector4s.Set(name, obj);
            else if (type == typeof(Color))
                colors.Set(name, obj);
            else if (type == typeof(LayerMaskMap))
                layerMasks.Set(name, obj);
            else
                throw new NotSupportedException($"Type {type.AssemblyQualifiedName} is not supported.");
        }

        public T Get<T>(string name)
        {
            return (T)Get(name, typeof(T));
        }

        public object Get(string name, System.Type type)
        {
            object obj = null;
            if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
                obj = objects.Get(name);
            else if (type == typeof(float))
                obj = floats.Get(name);
            else if (type == typeof(int))
                obj = ints.Get(name);
            else if (type == typeof(string))
                obj = strings.Get(name) ?? string.Empty;
            else if (type == typeof(bool))
                obj = bools.Get(name);
            else if (type.IsSubclassOf(typeof(System.Enum)))
                obj = enums.Get(name);
            else if (type == typeof(Vector3))
                obj = vector3s.Get(name);
            else if (type == typeof(Vector2))
                obj = vector2s.Get(name);
            else if (type == typeof(Vector4))
                obj = vector4s.Get(name);
            else if (type == typeof(Color))
                obj = colors.Get(name);
            else if (type == typeof(LayerMaskMap))
                obj = layerMasks.Get(name);
            else
                throw new NotSupportedException($"Type {type.AssemblyQualifiedName} is not supported. ({name})");
            if (obj == null && type.IsValueType)
                return Activator.CreateInstance(type);
            return obj;
        }

    }



}