using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Text.Json;

namespace Ray.Spike.Cap.Common;

/// <summary>
/// https://github.com/dotnetcore/CAP/blob/master/src/DotNetCore.CAP/Serialization/ISerializer.JsonUtf8.cs
/// </summary>
public class YourSerializer : ISerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public YourSerializer(IOptions<CapOptions> capOptions)
    {
        _jsonSerializerOptions = capOptions.Value.JsonSerializerOptions;
    }

    public ValueTask<TransportMessage> SerializeAsync(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        if (message.Value == null) return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, null));

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message.Value, _jsonSerializerOptions);

        return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, jsonBytes));
    }

    public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type? valueType)
    {
        if (valueType == null || transportMessage.Body.Length == 0)
            return new ValueTask<Message>(new Message(transportMessage.Headers, null));

        var obj = JsonSerializer.Deserialize(transportMessage.Body.Span, valueType, _jsonSerializerOptions);

        return new ValueTask<Message>(new Message(transportMessage.Headers, obj));
    }

    public string Serialize(Message message)
    {
        return JsonSerializer.Serialize(message, _jsonSerializerOptions);
    }

    public Message? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);
    }

    public object? Deserialize(object value, Type valueType)
    {
        if (value is JsonElement jsonElement) return jsonElement.Deserialize(valueType, _jsonSerializerOptions);

        throw new NotSupportedException("Type is not of type JsonElement");
    }

    public bool IsJsonType(object jsonObject)
    {
        return jsonObject is JsonElement;
    }
}