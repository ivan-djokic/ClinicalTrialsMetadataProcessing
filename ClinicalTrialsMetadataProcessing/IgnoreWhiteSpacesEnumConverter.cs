using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClinicalTrialsMetadataProcessing.Utils;

public class IgnoreWhiteSpacesEnumConverter<T> : StringEnumConverter where T : struct
{
	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		return Enum.Parse<T>(reader.Value?.ToString()?.Replace(" ", string.Empty) ?? string.Empty);
	}
}