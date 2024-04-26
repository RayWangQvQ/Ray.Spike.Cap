using DotNetCore.CAP;using DotNetCore.CAP.Messages;using DotNetCore.CAP.Serialization;using Microsoft.AspNetCore.DataProtection;using Microsoft.Extensions.Configuration;using Microsoft.Extensions.Logging;using Microsoft.Extensions.Options;using System.Buffers;using System.Text.Json;namespace Ray.Spike.Cap.Common;/// <summary>/// https://github.com/dotnetcore/CAP/blob/master/src/DotNetCore.CAP/Serialization/ISerializer.JsonUtf8.cs/// </summary>public class BodyEncryptSerializer : ISerializer{    private readonly JsonUtf8Serializer _jsonUtf8Serializer;    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IDataProtector _protector;
    private readonly ILogger<BodyEncryptSerializer> _logger;
    private readonly IConfiguration _config;

    public BodyEncryptSerializer(        IOptions<CapOptions> capOptions,         IDataProtectionProvider dataProtectionProvider,        ILogger<BodyEncryptSerializer> logger,        IConfiguration config        )    {        _jsonSerializerOptions = capOptions.Value.JsonSerializerOptions;        _protector = dataProtectionProvider.CreateProtector("SamplePurpose");
        _logger = logger;
        _config = config;

        _jsonUtf8Serializer = new JsonUtf8Serializer(capOptions);
    }

    #region Message
    public string Serialize(Message message)    {        Message newMessage = new Message()
        {
            Headers= message.Headers,
            Value= message.Value
        };        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("加密");
            var bodyStr = JsonSerializer.Serialize(message.Value, _jsonSerializerOptions);
            var bodyStrEncrypt = _protector.Protect(bodyStr);
            newMessage.Value = bodyStrEncrypt;
        }        var str = JsonSerializer.Serialize(newMessage, _jsonSerializerOptions);
        return str;    }    public Message? Deserialize(string json)    {        var msg = JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("解密");
            msg.Value = _protector.Unprotect(json);
        }

        return msg;    }

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
    }}