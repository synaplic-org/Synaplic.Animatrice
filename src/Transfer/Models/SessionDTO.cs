using FluentValidation;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Transfer.Requests.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Transfer.Models
{
	public class SessionDTO
	{
        public int Id { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        public DateTime? DateDebut { get; set; }
        public DateTime DateDebutPrevisionnel { get; set; }

        [Required]
        public DateTime? DateFin { get; set; }
        public SessionStatus Status { get; set; }

        //[JsonIgnore]
        //public string StatusString
        //{
        //    get
        //    {
        //        switch (Status)
        //        {
        //            case 0: return "Brouillon";
        //            case 1: return "Validée";
        //            case 2: return "En cours";
        //            case 3: return "Terminée";
        //            case 4: return "Cloturée";
        //            case 99: return "Annulée";
        //            default: return "?";
        //        }
        //    }
        //}

        public string DepotId { get; set; }
        public string Filtre { get; set; }

        [JsonIgnore]
        public string[] FiltreList => Filtre?.Split(",") ?? Array.Empty<string>();
        
        public string Log { get; set; }

        public long NbrScans { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }

    public class SessionDTOValidator : AbstractValidator<SessionDTO>
    {
        public SessionDTOValidator(IStringLocalizer<SessionDTOValidator> localizer)
        {
            RuleFor(request => request.Designation)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["La designation est requise !"]);
            RuleFor(request => request.DepotId)
                .NotEmpty().NotEqual("0").WithMessage(x => localizer["Dépot requis!"]);
            RuleFor(request => request.Filtre)
             .NotEmpty().WithMessage(x => localizer["Au moins un filtre est requis!"]);
            RuleFor(request => request.DateDebut)
                 .NotNull().WithMessage(x => localizer["La date début est requise!"])
                 .LessThanOrEqualTo(request => request.DateFin).WithMessage(x => localizer["La date debut doit etre <= date fin!"]) ;
            RuleFor(request => request.DateFin)
               .NotNull().WithMessage(x => localizer["La date fin est requise!"]);
        }
    }
}
