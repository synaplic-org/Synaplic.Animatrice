using System.Collections.Generic;
using System.Linq;
using Synaplic.Inventory.Shared.Constants.Localization;
using Synaplic.Inventory.Shared.Settings;

namespace Synaplic.Inventory.Client.Infrastructure.Settings
{
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsRtl => LanguageCode.StartsWith("ar-");
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ??  LocalizationConstants.DefaultLanguageCode;
        public List<string> MyTasklist { get; set; }

    }
}