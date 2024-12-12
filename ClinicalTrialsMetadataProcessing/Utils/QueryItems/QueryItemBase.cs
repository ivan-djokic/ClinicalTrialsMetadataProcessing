using System.Linq.Expressions;

namespace ClinicalTrialsMetadataProcessing.Utils.QueryItems;

public abstract class QueryItemBase<T>
{
	public IQueryable<T> Filter(IQueryable<T> items)
	{
		var predicate = GetPredicate();
		return predicate == null ? items : items.Where(predicate);
	}

	protected abstract Expression<Func<T, bool>>? GetPredicate();
}
