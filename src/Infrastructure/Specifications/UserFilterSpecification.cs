using Synaplic.Inventory.Infrastructure.Models.Identity;
using Synaplic.Inventory.Application.Specifications.Base;

namespace Synaplic.Inventory.Infrastructure.Specifications
{
    public class UserFilterSpecification : HeroSpecification<UniUser>
    {
        public UserFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString) || p.Email.Contains(searchString) || p.PhoneNumber.Contains(searchString) || p.UserName.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}