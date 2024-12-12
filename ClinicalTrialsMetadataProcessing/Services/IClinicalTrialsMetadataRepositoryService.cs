using ClinicalTrialsMetadataProcessing.Models;
using ClinicalTrialsMetadataProcessing.Utils.QueryItems;

namespace ClinicalTrialsMetadataProcessing.Services;

public interface IClinicalTrialsMetadataRepositoryService
{
	Task<ClinicalTrialMetadata?> GetClinicalTrialMetadataByIdAsync(string id);

	Task<ClinicalTrialsMetadata> GetClinicalTrialsMetadataByQueryAsync(IEnumerable<QueryItemBase<ClinicalTrialMetadata>> queryItems, int page, int? pageSize);

	void ProcessClinicalTrialMetadataSafely(ClinicalTrialMetadata item);
}
