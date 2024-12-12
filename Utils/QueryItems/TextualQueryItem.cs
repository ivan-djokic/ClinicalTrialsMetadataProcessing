using System.Linq.Expressions;

namespace ClinicalTrialsMetadataProcessing.Utils.QueryItems;

public class TextualQueryItem<T>(
		Func<T, string> getActualValue,
		IEnumerable<string>? values,
		string? subText = null,
		string? textStart = null,
		string? textEnd = null,
		bool caseSensitive = true) : QueryItem<T, string>(getActualValue, values)
{
	private readonly StringComparison m_comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
	private readonly string? m_subText = subText;
	private readonly string? m_textEnd = textEnd;
	private readonly string? m_textStart = textStart;

	protected override bool Contains(IEnumerable<string> values, string item)
	{
		if (m_comparisonType == StringComparison.OrdinalIgnoreCase)
		{
			return values.Contains(item, StringComparer.OrdinalIgnoreCase);
		}

		return base.Contains(values, item);
	}

	protected override Expression<Func<T, bool>>? GetPredicate(Func<T, string> getActualValue)
	{
		if (!string.IsNullOrEmpty(m_subText))
		{
			return item => getActualValue(item).Contains(m_subText, m_comparisonType);
		}

		if (!string.IsNullOrEmpty(m_textStart) && !string.IsNullOrEmpty(m_textEnd))
		{
			return item => getActualValue(item).StartsWith(m_textStart, m_comparisonType) && getActualValue(item).EndsWith(m_textEnd, m_comparisonType);
		}

		if (!string.IsNullOrEmpty(m_textStart))
		{
			return item => getActualValue(item).StartsWith(m_textStart, m_comparisonType);
		}

		if (!string.IsNullOrEmpty(m_textEnd))
		{
			return item => getActualValue(item).EndsWith(m_textEnd, m_comparisonType);
		}

		return null;
	}
}
