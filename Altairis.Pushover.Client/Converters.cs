using System.Text.Json;
using System.Text.Json.Serialization;

namespace Altairis.Pushover.Client;

internal class NumericBooleanConverter : JsonConverter<bool> {

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch {
            JsonTokenType.Number => reader.TryGetInt32(out var number) && number > 0,
            JsonTokenType.True => true,
            _ => false,
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value ? 1 : 0);

}

internal class UnixToDateTimeOffsetConverter : JsonConverter<DateTimeOffset> {

    public override bool HandleNull => true;

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var ts = reader.GetInt64();
        return ts == 0 ? DateTimeOffset.MinValue : DateTimeOffset.FromUnixTimeSeconds(ts);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value == DateTimeOffset.MinValue ? 0 : value.ToUnixTimeSeconds());

}

internal class UnixToNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?> {

    public override bool HandleNull => true;

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var ts = reader.GetInt64();
        return ts == 0 ? null : DateTimeOffset.FromUnixTimeSeconds(ts);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.HasValue ? value.Value.ToUnixTimeSeconds() : 0);

}