using System;
using System.ComponentModel.DataAnnotations;

public class DateGreaterThanOrEqualAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanOrEqualAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var currentValue = (DateTime)value;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            throw new ArgumentException($"Property with name {_comparisonProperty} not found");

        var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

        if (currentValue < comparisonValue)
            return new ValidationResult($"The date must be greater than or equal to {_comparisonProperty}");

        return ValidationResult.Success;
    }
}
