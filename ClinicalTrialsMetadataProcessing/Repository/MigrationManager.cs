using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialsMetadataProcessing.Repository;

public static class MigrationManager
{
	public static void MigrateDatabase(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
		context.Database.Migrate();
	}
}
