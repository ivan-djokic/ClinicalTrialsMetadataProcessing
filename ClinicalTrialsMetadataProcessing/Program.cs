using ClinicalTrialsMetadataProcessing.Middlewares;
using ClinicalTrialsMetadataProcessing.Repository;
using ClinicalTrialsMetadataProcessing.Services;
using ClinicalTrialsMetadataProcessing.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RepositoryContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettings.CONNECTION_STRING)));

builder.Services.AddSingleton<IClinicalTrialsMetadataParsingService, ClinicalTrialsMetadataParsingService>();
builder.Services.AddScoped<IClinicalTrialsMetadataRepositoryService, ClinicalTrialsMetadataRepositoryService>();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppSettings.OPTIONS));

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MigrateDatabase();

app.Run();
