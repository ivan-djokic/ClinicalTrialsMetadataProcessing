namespace ClinicalTrialsMetadataProcessing.Utils;

public class ValidationException(string format, params object?[] args) : Exception(string.Format(format, args))
{
	public ValidationException(string format, Exception ex)
		: this(string.Format(format, ex.Message))
	{
	}
}