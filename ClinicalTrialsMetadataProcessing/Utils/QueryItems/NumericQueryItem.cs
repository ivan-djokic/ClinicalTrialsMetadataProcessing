using System.Linq.Expressions;

namespace ClinicalTrialsMetadataProcessing.Utils.QueryItems;

public class NumericQueryItem<T>(
		Func<T, int?> getActualValue,
		IEnumerable<int?>? values,
		int? numberFrom,
		int? numberTo,
		bool hasValue = true) : QueryItem<T, int?>(getActualValue, values, hasValue)
{
	private readonly int? m_numberFrom = numberFrom;
	private readonly int? m_numberTo = numberTo;

	protected override Expression<Func<T, bool>>? GetPredicate(Func<T, int?> getActualValue)
	{
		if (m_numberFrom != null && m_numberTo != null)
		{
			return item => getActualValue(item) >= m_numberFrom && getActualValue(item) <= m_numberTo;
		}

		if (m_numberFrom != null)
		{
			return item => getActualValue(item) >= m_numberFrom;
		}

		if (m_numberTo != null)
		{
			return item => getActualValue(item) <= m_numberTo;
		}

		return null;
	}
}
