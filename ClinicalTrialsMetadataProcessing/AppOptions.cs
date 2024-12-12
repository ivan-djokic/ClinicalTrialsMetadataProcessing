namespace ClinicalTrialsMetadataProcessing.Utils;

public class AppOptions
{
	public string[] AllowedFileExtensions { get; set; } = [".json"];

	public bool AllowUpdate { get; set; }

	public int DefaultDurationInMonths { get; set; } = 1;

	public int DefaultFilterPageSize { get; set; } = 20;

	public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

	public int MinParticipants { get; set; } = 1;
}
