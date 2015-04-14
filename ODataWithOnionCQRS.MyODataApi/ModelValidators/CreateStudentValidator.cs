using ODataWithOnionCQRS.Core.DomainModels;
using FluentValidation;
using ODataWithOnionCQRS.MyODataApi.ViewModels;

namespace ODataWithOnionCQRS.MyODataApi.ModelValidators
{
    /// <summary>
    /// We can also validate against our ViewModels
    /// </summary>
    public class CreateStudentValidator : AbstractValidator<CreateStudentViewModel>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.FirstMidName).Length(0, 10);
        }
    }
}