using FluentValidation;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Transfer.Requests.Identity;

namespace Synaplic.Inventory.Transfer.Validators.Requests.Identity
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> localizer)
        {
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le mot de passe actuel est requis !"]);
            RuleFor(request => request.NewPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Mot de passe requis!"])
                .MinimumLength(8).WithMessage(localizer["Le mot de passe doit être au moins de longueur 8"])
                .Matches(@"[A-Z]").WithMessage(localizer["Le mot de passe doit contenir au moins une majuscule"])
                .Matches(@"[a-z]").WithMessage(localizer["Le mot de passe doit contenir au moins une lettre minuscule"])
                .Matches(@"[0-9]").WithMessage(localizer["Le mot de passe doit contenir au moins un chiffre"]);
            RuleFor(request => request.ConfirmNewPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["La confirmation du mot de passe est requise!"])
                .Equal(request => request.NewPassword).WithMessage(x => localizer["Les mots de passe ne correspondent pas"]);
        }
    }
}