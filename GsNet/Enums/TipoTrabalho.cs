using System.Text.Json.Serialization;

namespace GsNetApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoTrabalho
    {
        REMOTO = 1,
        PRESENCIAL = 2,
        HIBRIDO = 3
    }
}