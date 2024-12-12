using System.Linq.Expressions;

namespace ClinicalTrialsMetadataProcessing.Utils.QueryItems;

public class QueryItem<T, TResult>(
	Func<T, TResult> getActualValue,
	IEnumerable<TResult>? values,
	bool hasValue = true): QueryItemBase<T>
{
	private readonly Func<T, TResult> m_getActualValue = getActualValue;
	private readonly bool m_hasValue = hasValue;
	private readonly IEnumerable<TResult>? m_values = values;

	protected sealed override Expression<Func<T, bool>>? GetPredicate()
	{
		if (!m_hasValue)
		{
			return item => m_getActualValue(item) == null;
		}

		if (m_values != null)
		{
			return item => Contains(m_values, m_getActualValue(item));
		}

		return GetPredicate(m_getActualValue);
	}

	protected virtual bool Contains(IEnumerable<TResult> values, TResult item)
	{
		return values.Contains(item);
	}

	protected virtual Expression<Func<T, bool>>? GetPredicate(Func<T, TResult> getActualValue)
	{
		return null;
	}
}
