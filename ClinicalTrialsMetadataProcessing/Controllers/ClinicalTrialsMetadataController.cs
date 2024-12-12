using ClinicalTrialsMetadataProcessing.Models;
using ClinicalTrialsMetadataProcessing.Services;
using ClinicalTrialsMetadataProcessing.Utils;
using ClinicalTrialsMetadataProcessing.Utils.QueryItems;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalTrialsMetadataProcessing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClinicalTrialsMetadataController(
	IClinicalTrialsMetadataParsingService parsingService,
	IClinicalTrialsMetadataRepositoryService repositoryService,
	ILogger<ClinicalTrialsMetadataController> logger) : ControllerBase
{
	private readonly ILogger<ClinicalTrialsMetadataController> m_logger = logger;
	private readonly IClinicalTrialsMetadataParsingService m_parsingService = parsingService;
	private readonly IClinicalTrialsMetadataRepositoryService m_repositoryService = repositoryService;

	[HttpPost("upload")]
	public async Task<IActionResult> ProcessClinicalTrialMetadataFromJsonFile(IFormFile file)
	{
		var item = await m_parsingService.ParseClinicalTrialMetadataAsync(file);
		m_repositoryService.ProcessClinicalTrialMetadataSafely(item);

		return Ok(item);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetClinicalTrialMetadataById(string id)
	{
		var clinicalTrialMetadata = await m_repositoryService.GetClinicalTrialMetadataByIdAsync(id);

		if (clinicalTrialMetadata == null)
		{
			m_logger.LogInformation($"Record with TrialId: {id} not found");
			return NotFound();
		}

		m_logger.LogInformation($"Record with TrialId: {id} was found");
		return Ok(clinicalTrialMetadata);
	}

	[HttpGet]
	public async Task<IActionResult> GetClinicalTrialsMetadata(
		[FromQuery] IEnumerable<string>? trialIds,
		[FromQuery] IEnumerable<string>? titles,
		[FromQuery] string? subTitle,
		[FromQuery] string? titleStart,
		[FromQuery] string? titleEnd,
		[FromQuery] IEnumerable<DateTime?>? startDates,
		[FromQuery] DateTime? startDateFrom,
		[FromQuery] DateTime? startDateTo,
		[FromQuery] IEnumerable<DateTime?>? endDates,
		[FromQuery] DateTime? endDateFrom,
		[FromQuery] DateTime? endDateTo,
		[FromQuery] IEnumerable<int?>? participants,
		[FromQuery] int? participantsFrom,
		[FromQuery] int? participantsTo,
		[FromQuery] IEnumerable<ClinicalTrialsMetadataStatus>? status,
		[FromQuery] IEnumerable<int?>? durations,
		[FromQuery] int? durationFrom,
		[FromQuery] int? durationTo,
		[FromQuery] bool titleCaseSensitive = true,
		[FromQuery] bool hasEndDate = true,
		[FromQuery] bool hasParticipants = true,
		[FromQuery] bool hasDuration = true,
		[FromQuery] int page = 0,
		[FromQuery] int? pageSize = null)
	{
		var queryItems = new QueryItemBase<ClinicalTrialMetadata>[]
		{
			new QueryItem<ClinicalTrialMetadata, string>(
				item => item.TrialId, trialIds),
			new TextualQueryItem<ClinicalTrialMetadata>(
				item => item.Title, titles, subTitle, titleStart, titleEnd, titleCaseSensitive),
			new DateQueryItem<ClinicalTrialMetadata>(
				item => item.StartDate, startDates, startDateFrom, startDateTo),
			new DateQueryItem<ClinicalTrialMetadata>(
				item => item.EndDate, endDates, endDateFrom, endDateTo, hasEndDate),
			new NumericQueryItem<ClinicalTrialMetadata>(
				item => item.Participants, participants, participantsFrom, participantsTo, hasParticipants),
			new QueryItem<ClinicalTrialMetadata, ClinicalTrialsMetadataStatus>(
				item => item.Status, status),
			new NumericQueryItem<ClinicalTrialMetadata>(
				item => item.Duration, durations, durationFrom, durationTo, hasDuration)
		};

		var clinicalTrialsMetadata = await m_repositoryService.GetClinicalTrialsMetadataByQueryAsync(queryItems, page, pageSize);

		if (clinicalTrialsMetadata.Items.IsEmpty())
		{
			return NotFound();
		}

		return Ok(clinicalTrialsMetadata);
	}
}
