namespace WeatherMonitoring.Common.ParamsValidator;

using FluentValidation;
using FluentValidation.Results;

public class ParamsValidator : IParamsValidator
{
    private const int offsetMinBound = 0;
    private const int offsetMaxBound = 1000;

    private const int limitMinBound = 1;
    private const int limitMaxBound = 1000;

    private readonly IValidator<int> _offsetValidator;
    private readonly IValidator<int> _limitValidator;

    public ParamsValidator()
    {
        _offsetValidator = new InlineValidator<int>
        {
            v => v.RuleFor(x => x)
                .GreaterThanOrEqualTo(offsetMinBound).WithMessage("Offset must be greater than or equal to 0")
                .LessThanOrEqualTo(offsetMaxBound).WithMessage("Offset must be less than or equal to 100")
                .WithName("Offset")
        };

        _limitValidator = new InlineValidator<int>
        {
            v => v.RuleFor(x => x)
                .GreaterThanOrEqualTo(limitMinBound).WithMessage("Limit must be greater than or equal to 1")
                .LessThanOrEqualTo(limitMaxBound).WithMessage("Limit must be less than or equal to 1000")
                .WithName("Limit")
        };
    }

    public ValidationResult ValidateOffset(int offset)
    {
        var context = new ValidationContext<int>(offset);
        return _offsetValidator.Validate(context);
    }

    public ValidationResult ValidateLimit(int limit)
    {
        var context = new ValidationContext<int>(limit);
        return _limitValidator.Validate(context);
    }
}