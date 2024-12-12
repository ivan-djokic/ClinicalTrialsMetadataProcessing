namespace ClinicalTrialsMetadataProcessing.Models;

public class ClinicalTrialsMetadata
{
	public required ICollection<ClinicalTrialMetadata> Items { get; set; }

	public required int Page { get; set; }

	public required int PageSize { get; set; }

	public required int TotalCount { get; set; }
}
