namespace Xcepto.Interfaces;

public interface ISerializer
{
    public T Deserialize<T>(string serializedObject);
    public string Serialize<T>(T deserializedObject);
}