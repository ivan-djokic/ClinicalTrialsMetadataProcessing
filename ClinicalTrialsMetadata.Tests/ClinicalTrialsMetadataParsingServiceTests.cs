using ClinicalTrialsMetadataProcessing.Controllers;
using ClinicalTrialsMetadataProcessing.Models;
using ClinicalTrialsMetadataProcessing.Services;
using ClinicalTrialsMetadataProcessing.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ClinicalTrialsMetadataProcessing.Tests
{
	[TestClass]
	public class ClinicalTrialsMetadataParsingServiceTests
	{
		private readonly ClinicalTrialsMetadataController m_controller;
		private readonly Mock<ILogger<ClinicalTrialsMetadataController>> m_loggerMock = new();
		private readonly AppOptions m_options = new();
		private readonly Mock<IOptions<AppOptions>> m_optionsMock = new();
		private readonly Mock<IClinicalTrialsMetadataRepositoryService> m_repositoryServiceMock = new();

		public ClinicalTrialsMetadataParsingServiceTests()
		{
			m_optionsMock.Setup(_ => _.Value).Returns(m_options);
			m_controller = new(new ClinicalTrialsMetadataParsingService(m_optionsMock.Object), m_repositoryServiceMock.Object, m_loggerMock.Object);
		}

		[TestMethod]
		public async Task TestEmptyFileValidation()
		{
			await ExecuteNegativeTest(GetFile(string.Empty));
		}

		[TestMethod]
		public async Task TestTooLargeFileValidation()
		{
			await ExecuteNegativeTest(
				GetFile(TestFiles.ValidComplited.GetString(), m_options.MaxFileSize + 1));
		}

		[TestMethod]
		public async Task TestWrongFileExtensionValidation()
		{
			await ExecuteNegativeTest(
				GetFile(TestFiles.ValidComplited.GetString(), fileName: "fileName.txt"));
		}

		[TestMethod]
		public async Task TestInvalidMinValueValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.InvalidMinValue.GetString()));
		}

		[TestMethod]
		public async Task TestUnknownEnumMemberValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.UnknownEnumMember.GetString()));
		}

		[TestMethod]
		public async Task TestMissingRequiredFieldValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.MissingRequiredField.GetString()));
		}

		[TestMethod]
		public async Task TestEndDateLessThanStartValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.EndDateLessThanStart.GetString()));
		}

		[TestMethod]
		public async Task TestInvalidComplitedValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.InvalidComplited.GetString()));
		}

		[TestMethod]
		public async Task TestInvalidNotStartedValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.InvalidNotStarted.GetString()));
		}

		[TestMethod]
		public async Task TestInvalidOngoingValidation()
		{
			await ExecuteNegativeTest(GetFile(TestFiles.InvalidOngoing.GetString()));
		}

		[TestMethod]
		public async Task TestValidJsonValidation()
		{
			var result = await m_controller.ProcessClinicalTrialMetadataFromJsonFile(
				GetFile(TestFiles.ValidComplited.GetString()));

			CheckClinicalTrialMetadata(
				(result as ObjectResult)?.Value as ClinicalTrialMetadata,
				"CTM-78293",
				"Valid record with Ongoing status",
				new DateTime(2024, 1, 1),
				new DateTime(2024, 3, 31),
				12,
				ClinicalTrialsMetadataStatus.Completed);
		}

		[TestMethod]
		public async Task TestValidJsonWithDefaultParticipantsValidation()
		{
			var result = await m_controller.ProcessClinicalTrialMetadataFromJsonFile(
				GetFile(TestFiles.ValidNotStarted.GetString()));

			CheckClinicalTrialMetadata(
				(result as ObjectResult)?.Value as ClinicalTrialMetadata,
				"CTM-11233",
				"Valid NotStarted record with min Participants",
				new DateTime(3024, 5, 5),
				new DateTime(3024, 5, 5),
				m_options.MinParticipants,
				ClinicalTrialsMetadataStatus.NotStarted);
		}

		[TestMethod]
		public async Task TestValidJsonWithDefaultEndDateValidation()
		{
			var result = await m_controller.ProcessClinicalTrialMetadataFromJsonFile(
				GetFile(TestFiles.ValidOngoing.GetString().Replace("#startDatePlaceholder", DateTime.Now.ToString("yyyy-MM-dd"))));

			CheckClinicalTrialMetadata(
				(result as ObjectResult)?.Value as ClinicalTrialMetadata,
				"CTM-78293",
				"Valid Ongoing record with default EndDate",
				DateTime.Now.Date,
				DateTime.Now.Date.AddMonths(m_options.DefaultDurationInMonths),
				9,
				ClinicalTrialsMetadataStatus.Ongoing);
		}

		private static void CheckClinicalTrialMetadata(
			ClinicalTrialMetadata? item,
			string id,
			string title,
			DateTime start,
			DateTime end,
			int participants,
			ClinicalTrialsMetadataStatus status)
		{
			if (item == null)
			{
				Assert.Fail();
			}

			Assert.AreEqual(id, item.TrialId);
			Assert.AreEqual(title, item.Title);
			Assert.AreEqual(start, item.StartDate);
			Assert.AreEqual(end, item.EndDate);
			Assert.AreEqual(participants, item.Participants);
			Assert.AreEqual(status, item.Status);
			Assert.AreEqual((item.EndDate - item.StartDate)!.Value.Days, item.Duration);
		}

		private async Task ExecuteNegativeTest(IFormFile file)
		{
			try
			{
				await m_controller.ProcessClinicalTrialMetadataFromJsonFile(file);
			}
			catch (ValidationException)
			{
				return;
			}

			Assert.Fail();
		}

		private static IFormFile GetFile(string content, long? length = null, string fileName = "file.json")
		{
			var stram = GetStream(content);

			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(_ => _.OpenReadStream()).Returns(stram);
			fileMock.Setup(_ => _.FileName).Returns(fileName);
			fileMock.Setup(_ => _.Length).Returns(length ?? stram.Length);

			return fileMock.Object;
		}

		private static MemoryStream GetStream(string content)
		{
			// Do not dispose open stream
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);

			writer.Write(content);
			writer.Flush();
			stream.Position = 0;

			return stream;
		}
	}
}
