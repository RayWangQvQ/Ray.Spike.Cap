﻿using DotNetCore.CAP;
    private readonly IDataProtector _protector;
    private readonly ILogger<BodyEncryptSerializer> _logger;
    private readonly IConfiguration _config;

    public BodyEncryptSerializer(
        _logger = logger;
        _config = config;

        _jsonUtf8Serializer = new JsonUtf8Serializer(capOptions);
    }

    #region Message
    public string Serialize(Message message)
        {
            Headers= message.Headers,
            Value= message.Value
        };
        {
            _logger.LogInformation("加密");
            var bodyStr = JsonSerializer.Serialize(message.Value, _jsonSerializerOptions);
            var bodyStrEncrypt = _protector.Protect(bodyStr);
            newMessage.Value = bodyStrEncrypt;
        }
        return str;
        {
            _logger.LogInformation("解密");
            msg.Value = _protector.Unprotect(json);
        }

        return msg;

    #endregion

    public ValueTask<TransportMessage> SerializeAsync(Message message)
    {
        return _jsonUtf8Serializer.SerializeAsync(message);
    }

    public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type? valueType)
    {
        return _jsonUtf8Serializer.DeserializeAsync(transportMessage, valueType);
    }

    public object? Deserialize(object value, Type valueType)
    {
        return _jsonUtf8Serializer.Deserialize(value, valueType);
    }

    public bool IsJsonType(object jsonObject)
    {
        return _jsonUtf8Serializer.IsJsonType(jsonObject);
    }