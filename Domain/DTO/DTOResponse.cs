using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class DTOResponse<T>
    {
        [JsonPropertyName(nameof(Message))]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName(nameof(StatusCode))]
        public int StatusCode { get; set; }
        [JsonPropertyName(nameof(Data))]
        public T? Data { get; set; }
    }
}
