using System;

namespace Xcepto.Rest.Data;

public class ResponseValidation
{
    public ResponseValidation(Type responseType, Predicate<object> validationMethod, Func<string, object> deserializationMethod)
    {
        ResponseType = responseType;
        ValidationMethod = validationMethod;
        DeserializationMethod = deserializationMethod;
    }

    public Type ResponseType { get; }
    public Predicate<object> ValidationMethod { get; }
    public Func<string, object> DeserializationMethod { get; }
}