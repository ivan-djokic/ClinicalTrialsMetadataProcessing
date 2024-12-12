using ClinicalTrialsMetadataProcessing.Utils.QueryItems;

namespace ClinicalTrialsMetadataProcessing.Tests
{
	[TestClass]
	public class QueryItemsTests
	{
		private readonly DateTime?[] m_dates = [new DateTime(2024), new DateTime(2022), null, new DateTime(2026)];
		private readonly int?[] m_numbers = [2024, 2022, null, 2026];
		private readonly string[] m_textValues = ["abcd", "acbd", "bcda", "bacd"];

		[TestMethod]
		public void TestDateQueryItem()
		{
			CheckValues([null], Filter([new DateTime(2023), new DateTime(2022)], new DateTime(2020), new DateTime(2030), false));
			CheckValues([new DateTime(2022)], Filter([new DateTime(2023), new DateTime(2022)], new DateTime(2020), new DateTime(2030)));
			CheckValues([new DateTime(2024), new DateTime(2022)], Filter(null, new DateTime(2020), new DateTime(2025)));
			CheckValues([new DateTime(2024), new DateTime(2022), new DateTime(2026)], Filter(null, new DateTime(2020), null));
			CheckValues([new DateTime(2024), new DateTime(2022)], Filter(null, null, new DateTime(2025)));
			CheckValues(m_dates, Filter(null, null, null));
		}

		[TestMethod]
		public void TestNumericQueryItem()
		{
			CheckValues([null], FilterNums([2023, 2022], 2020, 2030, false));
			CheckValues([2022], FilterNums([2023, 2022], 2020, 2030));
			CheckValues([2024, 2022], FilterNums(null, 2020, 2025));
			CheckValues([2024, 2022, 2026], FilterNums(null, 2020, null));
			CheckValues([2024, 2022], FilterNums(null, null, 2025));
			CheckValues(m_numbers, FilterNums(null, null, null));
		}

		[TestMethod]
		public void TestTextualQueryItem()
		{
			CheckValues(["acbd", "bacd"], Filter(["ACBD", "BACD"], "cd", "a", "cd", false));
			CheckValues(["acbd", "bacd"], Filter(["acbd", "bacd"], "cd", "a", "cd"));
			CheckValues(["abcd", "bcda", "bacd"], Filter(null, "cd", "a", "cd"));
			CheckValues(["abcd"], Filter(null, null, "a", "cd"));
			CheckValues(["abcd", "acbd"], Filter(null, null, "a", null));
			CheckValues(["abcd", "bacd"], Filter(null, null, null, "cd"));
			CheckValues(m_textValues, Filter(null, null, null, null));
		}

		private static void CheckValues<T>(IList<T> expected, IList<T> actual)
		{
			Assert.AreEqual(expected.Count, actual.Count);

			for (var i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		private IList<DateTime?> Filter(IEnumerable<DateTime?>? values, DateTime? from, DateTime? to, bool hasValue = true)
		{
			return new DateQueryItem<DateTime?>(item => item, values, from, to, hasValue)
				.Filter(m_dates.AsQueryable()).ToList();
		}

		private IList<string> Filter(IEnumerable<string>? values, string? subText, string? textStart, string? textEnd, bool caseSensitive = true)
		{
			return new TextualQueryItem<string>(item => item, values, subText, textStart, textEnd, caseSensitive)
				.Filter(m_textValues.AsQueryable()).ToList();
		}

		private IList<int?> FilterNums(IEnumerable<int?>? values, int? from, int? to, bool hasValue = true)
		{
			return new NumericQueryItem<int?>(item => item, values, from, to, hasValue)
				.Filter(m_numbers.AsQueryable()).ToList();
		}
	}
}
