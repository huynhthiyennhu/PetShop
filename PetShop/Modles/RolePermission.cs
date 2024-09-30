﻿namespace PetShop.Models
{
    public class RolePermission
    {
        public string RoleId { get; set; } = Guid.NewGuid().ToString();
        public Role Role { get; set; }

        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
