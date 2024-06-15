namespace UserService.Models
{
    public class Company
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool IsBlocked { get; set; }
        public ICollection<ApplicationUser> ?Users { get; set; }
    }
}
