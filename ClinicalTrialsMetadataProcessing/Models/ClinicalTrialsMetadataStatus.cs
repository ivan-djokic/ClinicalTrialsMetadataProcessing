using Newtonsoft.Json;
using ClinicalTrialsMetadataProcessing.Utils;

namespace ClinicalTrialsMetadataProcessing.Models;

[JsonConverter(typeof(IgnoreWhiteSpacesEnumConverter<ClinicalTrialsMetadataStatus>))]
public enum ClinicalTrialsMetadataStatus
{
	Completed = 0,
	NotStarted = 1,
	Ongoing = 2
}
