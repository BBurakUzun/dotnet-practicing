using FluentValidation;

namespace deneme123.Dtos
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olmamalı")
                .MinimumLength(3);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olmamalı")
                .EmailAddress().WithMessage("Geçerli bir email adresi girin");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olmamalı")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı")
                .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermeli")
                .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermeli")
                .Matches(@"[0-9]").WithMessage("Şifre en az bir rakam içermeli")
                .Matches(@"[\W_]").WithMessage("Şifre en az bir özel karakter içermeli")
                .Equal(x => x.ConfirmPassword);
        }
    }
}
