using System.Net;
using System.Text;
using System.Text.Json;

namespace XChat.Api.Helpers.Http;

internal abstract class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; } = [];

    protected Response(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public abstract byte[] GetBodyBytes(Encoding? encoding = null, JsonSerializerOptions? options = null);
}

internal class Response<T> : Response
{
    public T? Data { get; set; }

    public Response(HttpStatusCode statusCode, T data = default!) : base(statusCode)
    {
        Data = data;
    }

    public override byte[] GetBodyBytes(Encoding? encoding = null, JsonSerializerOptions? options = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetBytes(ToJson(options));
    }

    private string ToJson(JsonSerializerOptions? options = null)
    {
        if (Data == null) return string.Empty;
        options ??= new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return JsonSerializer.Serialize(Data, options);
    }
}
