using System;

namespace Xcepto.Rest.Data;

public class RequestBody
{

    public RequestBody(Type requestType, object requestObject, Func<object, string> serializationMethod)
    {
        RequestType = requestType;
        RequestObject = requestObject;
        SerializationMethod = serializationMethod;
    }

    public Type RequestType { get; }
    public object RequestObject { get; }
    public Func<object, string> SerializationMethod { get; }

}