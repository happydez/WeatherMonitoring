using FluentValidation;
using FluentValidation.Results;

namespace WeatherMonitoring.Common.ParamsValidator;

public interface IParamsValidator
{
    ValidationResult ValidateOffset(int offset);
    ValidationResult ValidateLimit(int limit);
}