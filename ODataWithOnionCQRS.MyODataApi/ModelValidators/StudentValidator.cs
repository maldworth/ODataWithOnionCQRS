using ODataWithOnionCQRS.Core.DomainModels;
using FluentValidation;

namespace ODataWithOnionCQRS.MyODataApi.ModelValidators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FirstMidName).Length(0, 10);
        }
    }
}