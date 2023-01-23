using System.Threading.Tasks;
using Synaplic.Inventory.Transfer.Requests.Mail;

namespace Synaplic.Inventory.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}