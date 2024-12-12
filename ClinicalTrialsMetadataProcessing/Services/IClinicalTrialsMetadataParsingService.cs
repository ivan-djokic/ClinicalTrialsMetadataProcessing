using ClinicalTrialsMetadataProcessing.Models;

namespace ClinicalTrialsMetadataProcessing.Services;

public interface IClinicalTrialsMetadataParsingService
{
	Task<ClinicalTrialMetadata> ParseClinicalTrialMetadataAsync(IFormFile file);
}
