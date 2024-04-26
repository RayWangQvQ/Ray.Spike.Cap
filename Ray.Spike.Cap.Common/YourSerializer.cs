using DotNetCore.CAP;using DotNetCore.CAP.Messages;using DotNetCore.CAP.Serialization;using Microsoft.AspNetCore.DataProtection;using Microsoft.Extensions.Configuration;using Microsoft.Extensions.Logging;using Microsoft.Extensions.Options;using System.Buffers;using System.Text.Json;namespace Ray.Spike.Cap.Common;/// <summary>/// https://github.com/dotnetcore/CAP/blob/master/src/DotNetCore.CAP/Serialization/ISerializer.JsonUtf8.cs/// </summary>public class YourSerializer : ISerializer{    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IDataProtector _protector;
    private readonly ILogger<YourSerializer> _logger;
    private readonly IConfiguration _config;

    public YourSerializer(        IOptions<CapOptions> capOptions,         IDataProtectionProvider dataProtectionProvider,        ILogger<YourSerializer> logger,        IConfiguration config        )    {        _jsonSerializerOptions = capOptions.Value.JsonSerializerOptions;        _protector = dataProtectionProvider.CreateProtector("SamplePurpose");
        _logger = logger;
        _config = config;
    }

    #region Message
    public string Serialize(Message message)    {        var str = JsonSerializer.Serialize(message, _jsonSerializerOptions);        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("加密");
            str = _protector.Protect(str);
        }

        return str;    }    public Message? Deserialize(string json)    {        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("解密");
            json = _protector.Unprotect(json);
        }

        var msg = JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);        return msg;    }
    #endregion


    public ValueTask<TransportMessage> SerializeAsync(Message message)    {        if (message == null) throw new ArgumentNullException(nameof(message));        if (message.Value == null) return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, null));        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message.Value, _jsonSerializerOptions);        return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, jsonBytes));    }    public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type? valueType)    {        if (valueType == null || transportMessage.Body.Length == 0)            return new ValueTask<Message>(new Message(transportMessage.Headers, null));        var obj = JsonSerializer.Deserialize(transportMessage.Body.Span, valueType, _jsonSerializerOptions);        return new ValueTask<Message>(new Message(transportMessage.Headers, obj));    }    public object? Deserialize(object value, Type valueType)    {        if (value is JsonElement jsonElement) return jsonElement.Deserialize(valueType, _jsonSerializerOptions);        throw new NotSupportedException("Type is not of type JsonElement");    }    public bool IsJsonType(object jsonObject)    {        return jsonObject is JsonElement;    }}