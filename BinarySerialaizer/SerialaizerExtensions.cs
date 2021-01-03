using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Serialaizer;

namespace Serialaizer.Extensions
{
    public static class SerialaizerExtension
    {
        /// <summary>
        /// Serialize class model using BinaryFormatter
        /// </summary>
        /// <param name="obj">class model</param>
        /// <returns>byte array</returns>
        public static byte[] BinarySerialize<T>(this T obj)
        {
            return new BinaryModelSerialaizer(new BinaryFormatter()).Serialize(obj);
        }

        /// <summary>
        /// Deserialize byte array using BinaryFormatter
        /// </summary>
        /// <param name="data">byte array</param>
        /// <returns>calss model</returns>
        public static T BinaryDeserialize<T>(this byte[] data)
        {
            return new BinaryModelSerialaizer(new BinaryFormatter()).Deserialize<T>(data);
        }

        /// <summary>
        /// Serialize class model as Json
        /// </summary>
        /// <param name="obj">class model</param>
        /// <returns>byte array</returns>
        public static byte[] JsonSerialize<T>(this T obj)
        {
            return null;
        }

        /// <summary>
        /// Deserialize byte array as Json
        /// </summary>
        /// <param name="data">byte array</param>
        /// <returns>calss model</returns>
        public static T JsonDeserialize<T>(this byte[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Serialize class model as XML
        /// </summary>
        /// <param name="obj">class model</param>
        /// <returns>byte array</returns>
        public static byte[] XmlSerialize<T>(this T obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserialize byte array as XML
        /// </summary>
        /// <param name="data">byte array</param>
        /// <returns>calss model</returns>
        public static T XmlDeserialize<T>(this byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
