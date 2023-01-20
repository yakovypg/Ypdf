using Avalonia.Media;
using Newtonsoft.Json;
using System;
using YpdfDesktop.Infrastructure.Services;

namespace YpdfDesktop.Infrastructure.Serialization
{
    internal class ISolidColorBrushConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ISolidColorBrush) ||
                   objectType == typeof(string);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            string? value = reader.Value as string;

            return string.IsNullOrEmpty(value)
                ? null
                : SolidColorBrush.Parse(value);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not ISolidColorBrush colorBrush)
                writer.WriteValue(string.Empty);
            else
                writer.WriteValue(ColorService.GetHex(colorBrush.Color));

            writer.Flush();
        }
    }
}
