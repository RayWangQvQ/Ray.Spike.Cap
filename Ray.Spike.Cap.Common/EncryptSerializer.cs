using DotNetCore.CAP;using DotNetCore.CAP.Messages;using DotNetCore.CAP.Serialization;using Microsoft.AspNetCore.DataProtection;using Microsoft.Extensions.Configuration;using Microsoft.Extensions.Logging;using Microsoft.Extensions.Options;using System.Buffers;using System.Text.Json;namespace Ray.Spike.Cap.Common;







/// <summary>/// https://github.com/dotnetcore/CAP/blob/master/src/DotNetCore.CAP/Serialization/ISerializer.JsonUtf8.cs/// </summary>                                                                                                                                        public class EncryptSerializer : ISerializer{    private readonly JsonUtf8Serializer _jsonUtf8Serializer;    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IDataProtector _protector;
    private readonly ILogger<EncryptSerializer> _logger;
    private readonly IConfiguration _config;

    public EncryptSerializer(        IOptions<CapOptions> capOptions,
        IDataProtectionProvider dataProtectionProvider,        ILogger<EncryptSerializer> logger,        IConfiguration config        )    {        _jsonSerializerOptions = capOptions.Value.JsonSerializerOptions;        _protector = dataProtectionProvider.CreateProtector("SamplePurpose");
        _logger = logger;
        _config = config;

        _jsonUtf8Serializer = new JsonUtf8Serializer(capOptions);
    }

    public string Serialize(Message message)    {
        _logger.LogInformation("这是序列化：Message->string");

        var str = JsonSerializer.Serialize(message, _jsonSerializerOptions);        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("加密");
            str = _protector.Protect(str);
        }

        return str;    }    public Message? Deserialize(string json)    {
        _logger.LogInformation("这是反序列化：string->Message");

        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("解密");
            json = _protector.Unprotect(json);
        }

        var msg = JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);        return msg;    }


    public ValueTask<TransportMessage> SerializeAsync(Message message)
    {
        _logger.LogInformation("这是序列化：Message->TransportMessage");
        return _jsonUtf8Serializer.SerializeAsync(message);
    }

    public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type? valueType)
    {
        _logger.LogInformation("这是反序列化：TransportMessage->Message");
        return _jsonUtf8Serializer.DeserializeAsync(transportMessage, valueType);
    }

    public object? Deserialize(object value, Type valueType)
    {
        _logger.LogInformation("这是反序列化：object->object");
        return _jsonUtf8Serializer.Deserialize(value, valueType);
    }

    public bool IsJsonType(object jsonObject)
    {
        var re = _jsonUtf8Serializer.IsJsonType(jsonObject);
        _logger.LogInformation("这是判断IsJsonType：{re}", re);
        return re;
    }
}