using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialaizer
{
    public class BinaryModelSerialaizer
    {
        BinaryFormatter _binaryFormatter;

        public BinaryModelSerialaizer(BinaryFormatter binaryFormatter)
        {
            _binaryFormatter = binaryFormatter;
        }

        public byte[] Serialize<T>(T obj)
        {
            using (MemoryStream s = new MemoryStream())
            {
                _binaryFormatter.Serialize(s, obj);

                return s.ToArray();
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            using (MemoryStream s = new MemoryStream(data))
            {
                _binaryFormatter.Binder = new BinarySerializationBinder(typeof(T));

                return (T)_binaryFormatter.Deserialize(s);

            }
        }
    }
}
