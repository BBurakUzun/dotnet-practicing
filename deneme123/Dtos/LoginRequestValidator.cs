using FluentValidation;

namespace deneme123.Dtos
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olmamalı")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalı");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olmamalı")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı");
        }
    }
}
