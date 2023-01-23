using System.Linq;
using Synaplic.Inventory.Shared.Constants.Localization;
using Synaplic.Inventory.Shared.Settings;

namespace Synaplic.Inventory.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? LocalizationConstants.DefaultLanguageCode; 

        //TODO - add server preferences
    }
}