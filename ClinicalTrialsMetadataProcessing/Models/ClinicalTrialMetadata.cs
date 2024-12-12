using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicalTrialsMetadataProcessing.Utils;

namespace ClinicalTrialsMetadataProcessing.Models;

[Index(nameof(TrialId), IsUnique = true)]
public class ClinicalTrialMetadata
{
	[NotMapped] 
	public int? Duration
	{
		get => (EndDate - StartDate)?.Days;
	}

	public DateTime? EndDate { get; set; }

	public int Participants { get; set; }

	public required DateTime StartDate { get; set; }

	public required ClinicalTrialsMetadataStatus Status { get; set; }

	[Key]
	[MaxLength(50)]
	public required string TrialId { get; set; }

	[MaxLength(100)]
	public required string Title { get; set; }

	public void Initialize(AppOptions options)
	{
		if (EndDate == null && Status == ClinicalTrialsMetadataStatus.Ongoing)
		{
			EndDate = StartDate.AddMonths(options.DefaultDurationInMonths);
		}

		if (Participants == 0)
		{
			Participants = options.MinParticipants;
		}
	}
}
