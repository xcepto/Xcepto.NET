using System;

namespace Xcepto.Rest.Data;

public class RequestBody
{

    public RequestBody(Type requestType, Func<object> requestObjectPromise, Func<object, string> serializationMethod)
    {
        RequestType = requestType;
        RequestObjectPromise = requestObjectPromise;
        SerializationMethod = serializationMethod;
    }

    public Type RequestType { get; }
    public Func<object> RequestObjectPromise { get; }
    public Func<object, string> SerializationMethod { get; }

}