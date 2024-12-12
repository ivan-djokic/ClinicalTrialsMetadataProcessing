using System.Linq.Expressions;

namespace ClinicalTrialsMetadataProcessing.Utils.QueryItems;

public class DateQueryItem<T>(
		Func<T, DateTime?> getActualValue,
		IEnumerable<DateTime?>? values,
		DateTime? dateFrom,
		DateTime? dateTo,
		bool hasValue = true) : QueryItem<T, DateTime?>(getActualValue, values, hasValue)
{
	private readonly DateTime? m_dateFrom = dateFrom;
	private readonly DateTime? m_dateTo = dateTo;

	protected override Expression<Func<T, bool>>? GetPredicate(Func<T, DateTime?> getActualValue)
	{
		if (m_dateFrom != null && m_dateTo != null)
		{
			return item => getActualValue(item) >= m_dateFrom && getActualValue(item) <= m_dateTo;
		}

		if (m_dateFrom != null)
		{
			return item => getActualValue(item) >= m_dateFrom;
		}

		if (m_dateTo != null)
		{
			return item => getActualValue(item) <= m_dateTo;
		}

		return null;
	}
}
