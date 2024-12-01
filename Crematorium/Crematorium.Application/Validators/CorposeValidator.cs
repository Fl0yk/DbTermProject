using Crematorium.Domain.Entities;
using FluentValidation;

namespace Crematorium.Application.Validators
{
    public class CorposeValidator : AbstractValidator<Corpose>
    {
        public CorposeValidator()
        {
            RuleFor(u => u.Name).NotNull().NotEmpty().Length(1, 20).WithMessage("Некорректное имя");
            RuleFor(u => u.SurName).NotNull().NotEmpty().Length(1, 20).WithMessage("Некорректная фамилия");
            RuleFor(u => u.NumPassport).NotNull().Matches(@"\d{7}\w\d{3}(PB|BI|BA)\d").WithMessage("Некорректный номер паспорта");
        }
    }
}
