using NJsonSchema.Validation;
using System.Text;

namespace ClinicalTrialsMetadataProcessing.Utils;

public static class Extensions
{
	public static string GetString(this byte[] items)
	{
		return Encoding.Default.GetString(items);
	}

	public static bool IsEmpty<T>(this ICollection<T> items)
	{
		// Do not use items.Any() because of performance issues
		return items.Count == 0;
	}
}
