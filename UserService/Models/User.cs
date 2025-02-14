namespace UserService.Models
{
    public enum MembershipType
    {
        Regular,
        Premium
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public MembershipType MembershipType { get; set; }
        public bool IsLocked { get; set; }
        public int NombreMaxEmprunt { get; set; }
    }
}
