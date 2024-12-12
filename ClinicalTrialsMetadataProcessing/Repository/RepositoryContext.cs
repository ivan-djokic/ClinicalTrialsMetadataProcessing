using Microsoft.EntityFrameworkCore;
using ClinicalTrialsMetadataProcessing.Models;

namespace ClinicalTrialsMetadataProcessing.Repository;

public class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
{
	public DbSet<ClinicalTrialMetadata> ClinicalTrialsMetadata { get; set; }
}
