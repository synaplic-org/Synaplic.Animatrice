﻿using Synaplic.Inventory.Shared.Enums;

namespace Synaplic.Inventory.Transfer.Responses.Identity
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public string SiteID { get; set; }
        public UserType UserType { get; set; }
        public string EmployeeID { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureDataUrl { get; set; }
    }
}