﻿using System;
using System.IO;
using System.Text;

namespace TMDbLib.Utilities.Serializer
{
    public static class SerializerExtensions
    {
        [Obsolete]
        public static void Serialize<T>(this ITMDbSerializer serializer, Stream target, T @object)
        {
            serializer.Serialize(target, @object, typeof(T));
        }

        [Obsolete]
        public static byte[] SerializeToBytes<T>(this ITMDbSerializer serializer, T @object)
        {
            using MemoryStream ms = new MemoryStream();

            serializer.Serialize(ms, @object, typeof(T));

            return ms.ToArray();
        }

        [Obsolete]
        public static string SerializeToString<T>(this ITMDbSerializer serializer, T @object)
        {
            using MemoryStream ms = new MemoryStream();

            serializer.Serialize(ms, @object, typeof(T));

            ms.Seek(0, SeekOrigin.Begin);

            using StreamReader sr = new StreamReader(ms, Encoding.UTF8);

            return sr.ReadToEnd();
        }

        [Obsolete]
        public static T ObsoleteDeserialize<T>(this ITMDbSerializer serializer, Stream source)
        {
            return (T)serializer.Deserialize(source, typeof(T));
        }

        [Obsolete]
        public static T DeserializeFromString<T>(this ITMDbSerializer serializer, string json)
        {
            // TODO: Better method
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using MemoryStream ms = new MemoryStream(bytes);

            return serializer.ObsoleteDeserialize<T>(ms);
        }

        [Obsolete]
        public static object DeserializeFromString(this ITMDbSerializer serializer, string json, Type type)
        {
            // TODO: Better method
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using MemoryStream ms = new MemoryStream(bytes);

            return serializer.Deserialize(ms, type);
        }
    }
}