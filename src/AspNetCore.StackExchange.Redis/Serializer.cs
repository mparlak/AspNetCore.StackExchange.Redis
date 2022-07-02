using System.Text.Json;

namespace AspNetCore.StackExchange.Redis;

public class Serializer : ISerializer
{
    public string Serialize(object data)
    {
        return JsonSerializer.Serialize(data);
    }
    
    public string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data);
    }
    
    public string Serialize(object data, JsonSerializerOptions jsonSerializerOptions)
    {
        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    public T Deserialize<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }
}