using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class FlexibleStringJsonConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDouble().ToString(),
                JsonTokenType.True => bool.TrueString,
                JsonTokenType.False => bool.FalseString,
                JsonTokenType.Null => null,
                _ => JsonDocument.ParseValue(ref reader).RootElement.GetRawText()
            };
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value);
        }
    }
}