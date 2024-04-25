using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Text.Json;

namespace Ray.Spike.Cap;

/// <summary>
/// https://github.com/dotnetcore/CAP/blob/master/src/DotNetCore.CAP/Serialization/ISerializer.JsonUtf8.cs
/// </summary>
public class YourSerializer : ISerializer
{
    private readonly ILogger<YourSerializer> _logger;

    public YourSerializer(ILogger<YourSerializer> logger)
    {
        _logger = logger;
    }

    public Task<TransportMessage> SerializeAsync(Message message)
    {
        _logger.LogInformation("SerializeAsync");

        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.Value == null)
        {
            return Task.FromResult(new TransportMessage(message.Headers, null));
        }

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message.Value);
        return Task.FromResult(new TransportMessage(message.Headers, jsonBytes));
    }

    public Task<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
    {
        _logger.LogInformation("DeserializeAsync");

        if (valueType == null || transportMessage.Body == null)
        {
            return Task.FromResult(new Message(transportMessage.Headers, null));
        }

        var obj = JsonSerializer.Deserialize(transportMessage.Body, valueType);

        return Task.FromResult(new Message(transportMessage.Headers, obj));
    }

    public string Serialize(Message message)
    {
        _logger.LogInformation("Serialize");

        return JsonSerializer.Serialize(message);
    }

    public Message Deserialize(string json)
    {
        _logger.LogInformation("Deserialize");

        return JsonSerializer.Deserialize<Message>(json);
    }

    public object Deserialize(object value, Type valueType)
    {
        _logger.LogInformation("Deserialize to object");

        if (value is JsonElement jToken)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                jToken.WriteTo(writer);
            }
            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, valueType);
        }
        throw new NotSupportedException("Type is not of type JToken");
    }

    public bool IsJsonType(object jsonObject)
    {
        _logger.LogInformation("IsJsonType");

        return jsonObject is JsonElement;
    }

}