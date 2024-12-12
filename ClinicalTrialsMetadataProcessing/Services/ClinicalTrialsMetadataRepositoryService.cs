using ClinicalTrialsMetadataProcessing.Models;
using ClinicalTrialsMetadataProcessing.Properties;
using ClinicalTrialsMetadataProcessing.Repository;
using ClinicalTrialsMetadataProcessing.Utils;
using ClinicalTrialsMetadataProcessing.Utils.QueryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClinicalTrialsMetadataProcessing.Services;

public class ClinicalTrialsMetadataRepositoryService(RepositoryContext context, IOptions<AppOptions> options) : IClinicalTrialsMetadataRepositoryService
{
	private readonly RepositoryContext m_context = context;
	private readonly object m_locker = new();
	private readonly AppOptions m_options = options.Value;

	public async Task<ClinicalTrialMetadata?> GetClinicalTrialMetadataByIdAsync(string id)
	{
		return await m_context.ClinicalTrialsMetadata.FirstOrDefaultAsync(trial => trial.TrialId == id);
	}

	public async Task<ClinicalTrialsMetadata> GetClinicalTrialsMetadataByQueryAsync(IEnumerable<QueryItemBase<ClinicalTrialMetadata>> queryItems, int page, int? pageSize)
	{
		var clinicalTrialsMetadata = m_context.ClinicalTrialsMetadata.AsQueryable();

		foreach (var queryItem in queryItems)
		{
			clinicalTrialsMetadata = queryItem.Filter(clinicalTrialsMetadata);
		}

		pageSize ??= m_options.DefaultFilterPageSize;
		var totalCount = await clinicalTrialsMetadata.CountAsync();

		return new ClinicalTrialsMetadata
		{
			Items = totalCount > 0 ? await clinicalTrialsMetadata.Skip(page * pageSize.Value).Take(pageSize.Value).ToListAsync() : [],
			Page = page,
			PageSize = pageSize.Value,
			TotalCount = totalCount
		};
	}

	public void ProcessClinicalTrialMetadataSafely(ClinicalTrialMetadata item)
	{
		lock (m_locker)
		{
			AddOrUpdateClinicalTrialMetadata(item);
			m_context.SaveChanges();
		}
	}

	private void AddOrUpdateClinicalTrialMetadata(ClinicalTrialMetadata item)
	{
		var existingItem = GetClinicalTrialMetadataById(item.TrialId);

		if (existingItem == null)
		{
			m_context.ClinicalTrialsMetadata.Add(item);
			return;
		}

		if (!m_options.AllowUpdate)
		{
			throw new ValidationException(AppErrors.UpdateNotAllowed, item.TrialId);
		}

		m_context.Entry(existingItem).CurrentValues.SetValues(item);
	}

	private ClinicalTrialMetadata? GetClinicalTrialMetadataById(string id)
	{
		return m_context.ClinicalTrialsMetadata.FirstOrDefault(trial => trial.TrialId == id);
	}
}
