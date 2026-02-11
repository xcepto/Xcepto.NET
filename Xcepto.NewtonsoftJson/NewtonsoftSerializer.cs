using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Xcepto.Interfaces;

namespace Xcepto.NewtonsoftJson
{
    public class NewtonsoftSerializer: ISerializer
    {
        public T Deserialize<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject) ?? 
                   throw new SerializationException($"String could not be deserialized to {typeof(T).FullName}: {serializedObject}");
        }

        public string Serialize<T>(T deserializedObject)
        {
            return JsonConvert.SerializeObject(deserializedObject);
        }
    }
}