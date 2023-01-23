using FluentValidation;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Transfer.Requests.Identity;

namespace Synaplic.Inventory.Transfer.Validators.Requests.Identity
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le prénom est requis"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le nom de famille est requis"]);
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["L'e-mail est requis"])
                .EmailAddress().WithMessage(x => localizer["L'e-mail n'est pas correct"]);
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Nom d'utilisateur est requis"])
                .MinimumLength(6).WithMessage(localizer["Le nom d'utilisateur doit avoir au moins une longueur de 6"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Mot de passe requis!"])
                .MinimumLength(8).WithMessage(localizer["Le mot de passe doit être au moins de longueur 8"])
                .Matches(@"[A-Z]").WithMessage(localizer["Le mot de passe doit contenir au moins une majuscule"])
                .Matches(@"[a-z]").WithMessage(localizer["Le mot de passe doit contenir au moins une lettre minuscule"])
                .Matches(@"[0-9]").WithMessage(localizer["Le mot de passe doit contenir au moins un chiffre"]);
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["La confirmation du mot de passe est requise!"])
                .Equal(request => request.Password).WithMessage(x => localizer["Les mots de passe ne correspondent pas"]);
        }
    }
}