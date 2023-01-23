using Synaplic.Inventory.Shared.Settings;
using System.Threading.Tasks;
using Synaplic.Inventory.Shared.Wrapper;

namespace Synaplic.Inventory.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}