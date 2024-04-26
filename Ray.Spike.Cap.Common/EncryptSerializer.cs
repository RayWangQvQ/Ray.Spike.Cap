﻿using DotNetCore.CAP;







/// <summary>
    private readonly IDataProtector _protector;
    private readonly ILogger<EncryptSerializer> _logger;
    private readonly IConfiguration _config;

    public EncryptSerializer(
        IDataProtectionProvider dataProtectionProvider,
        _logger = logger;
        _config = config;

        _jsonUtf8Serializer = new JsonUtf8Serializer(capOptions);
    }

    public string Serialize(Message message)
        _logger.LogInformation("这是序列化：Message->string");

        var str = JsonSerializer.Serialize(message, _jsonSerializerOptions);
        {
            _logger.LogInformation("加密");
            str = _protector.Protect(str);
        }

        return str;
        _logger.LogInformation("这是反序列化：string->Message");

        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("解密");
            json = _protector.Unprotect(json);
        }

        var msg = JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);


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