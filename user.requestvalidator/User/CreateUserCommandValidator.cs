using FluentValidation;
using user.request.Commands.v1;

namespace user.requestvalidator.User;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .WithErrorCode("USER0001");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .WithErrorCode("USER0002");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .WithErrorCode("USER0003");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .WithErrorCode("USER0004");

        RuleFor(x => x.UserRole)
            .NotEmpty()
            .WithMessage("UserRole is required")
            .WithErrorCode("USER0005");
    }
}
