using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Serialaizer
{
    sealed class BinarySerializationBinder : SerializationBinder
    {
        Type _type;
        public BinarySerializationBinder(Type type)
        {
            _type = type;
        }
        public override Type BindToType(string assemblyName, string typeName)
        {
            return _type;
        }
    }
}
