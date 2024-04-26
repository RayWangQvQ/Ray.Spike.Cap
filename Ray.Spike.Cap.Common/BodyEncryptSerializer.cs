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
    public string Serialize(Message message)    {        _logger.LogInformation("这是序列化：Message->string");

        Message newMessage = new Message()
        {
            Headers= message.Headers,
            Value= message.Value
        };        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("加密");
            var bodyStr = JsonSerializer.Serialize(message.Value, _jsonSerializerOptions);//todo
            var bodyStrEncrypt = _protector.Protect(bodyStr);
            newMessage.Value = bodyStrEncrypt;
        }        var str = JsonSerializer.Serialize(newMessage, _jsonSerializerOptions);
        return str;    }    public Message? Deserialize(string json)    {        _logger.LogInformation("这是反序列化：string->Message");

        var msg = JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions);        if (_config.GetSection("CAP:StorageEncryptEnable").Get<bool>())
        {
            _logger.LogInformation("解密");
            var bodyStr = msg.Value.ToString();//todo
            var jsonStr = _protector.Unprotect(bodyStr);
            msg.Value = JsonSerializer.Deserialize<JsonElement>(jsonStr);
        }

        return msg;    }

    #endregion

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
    }}