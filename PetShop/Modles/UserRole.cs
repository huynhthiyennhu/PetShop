namespace PetShop.Models
{
    public class UserRole
    {
        public string UserId { get; set; }
        public required User User { get; set; }

        public string RoleId { get; set; }
        public required Role Role { get; set; }
    }
}
