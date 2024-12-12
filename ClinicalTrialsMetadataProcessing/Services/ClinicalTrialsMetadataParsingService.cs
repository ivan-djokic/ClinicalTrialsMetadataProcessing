using ClinicalTrialsMetadataProcessing.Models;
using ClinicalTrialsMetadataProcessing.Properties;
using ClinicalTrialsMetadataProcessing.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NJsonSchema;
using System.Text;

namespace ClinicalTrialsMetadataProcessing.Services;

public class ClinicalTrialsMetadataParsingService(IOptions<AppOptions> options) : IClinicalTrialsMetadataParsingService
{
	private JsonSchema? m_jsonSchema;
	private readonly AppOptions m_options = options.Value;

	public async Task<ClinicalTrialMetadata> ParseClinicalTrialMetadataAsync(IFormFile file)
	{
		var jsonContent = await ReadJsonContent(file);
		await ValidateJsonSchema(jsonContent);

		var clinicalTrialMetadata = DeserializeObject(jsonContent);
		ValidateClinicalTrialMetadata(clinicalTrialMetadata);

		return clinicalTrialMetadata;
	}

	private ClinicalTrialMetadata DeserializeObject(string jsonContent)
	{
		try
		{
			var clinicalTrialMetadata = JsonConvert.DeserializeObject<ClinicalTrialMetadata>(jsonContent)!;
			clinicalTrialMetadata.Initialize(m_options);

			return clinicalTrialMetadata;
		}
		catch (Exception ex)
		{
			throw new ValidationException(AppErrors.JsonDeserializationFailed, ex);
		}
	}

	private static string? GetClinicalTrialMetadataValidationError(ClinicalTrialMetadata item)
	{
		if (item.EndDate < item.StartDate)
		{
			return AppErrors.EndDateLessThanStart;
		}

		return item.Status switch
		{
			ClinicalTrialsMetadataStatus.Completed => item.EndDate < DateTime.Now ? null : AppErrors.CompletedStatusError,
			ClinicalTrialsMetadataStatus.NotStarted => item.StartDate > DateTime.Now ? null : AppErrors.NotStartedStatusError,
			ClinicalTrialsMetadataStatus.Ongoing => item.StartDate < DateTime.Now && item.EndDate > DateTime.Now ? null : AppErrors.OngoingStatusError,
			_ => null,
		};
	}

	private async Task<string> ReadJsonContent(IFormFile file)
	{
		ValidateFile(file);

		using var reader = new StreamReader(file.OpenReadStream());
		return await reader.ReadToEndAsync();
	}

	private static void ValidateClinicalTrialMetadata(ClinicalTrialMetadata item)
	{
		var errorMessage = GetClinicalTrialMetadataValidationError(item);

		if (errorMessage != null)
		{
			throw new ValidationException(AppErrors.ClinicalTrialMetadataValidationFailed, errorMessage);
		}
	}

	private void ValidateFile(IFormFile file)
	{
		if (!m_options.AllowedFileExtensions.Contains(Path.GetExtension(file.FileName)?.ToLower()))
		{
			throw new ValidationException(AppErrors.FileExtensionNotAllowed);
		}

		if (file.Length == 0)
		{
			throw new ValidationException(AppErrors.EmptyFile);
		}

		if (file.Length > m_options.MaxFileSize)
		{
			throw new ValidationException(AppErrors.FileSizeExceededLimit, m_options.MaxFileSize);
		}
	}

	public async Task ValidateJsonSchema(string jsonContent)
	{
		m_jsonSchema ??= await JsonSchema.FromJsonAsync(Resources.JsonSchema.GetString());
		var errors = m_jsonSchema.Validate(jsonContent);

		if (!errors.IsEmpty())
		{
			throw new ValidationException(AppErrors.JsonValidationFailed, string.Join(", ", errors));
		}
	}
}
