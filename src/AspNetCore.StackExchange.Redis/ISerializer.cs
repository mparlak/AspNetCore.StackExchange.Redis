using System.Text.Json;

namespace AspNetCore.StackExchange.Redis;

public interface ISerializer
{
    string Serialize(object data);
    string Serialize<T>(T data);
    string Serialize(object data, JsonSerializerOptions jsonSerializerOptions);
    T Deserialize<T>(string data);
}