using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementService.Messaging.Converters;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        DateTime.ParseExact(reader.GetString()!,
            "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture);

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTimeValue,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture));
    }
}