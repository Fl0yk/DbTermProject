namespace Crematorium.Domain.Entities
{
    public class User : Entity
    {
        public string Surname { get; set; } = "";

        public string MailAdress { get; set; } = "";

        public string NumPassport { get; set; } = "";

        public List<Order> Orders { get; set; } = new();

        public Role UserRole { get; set; } = Role.NoName;
    }

    public enum Role
    {
        Admin = 2,
        Employee = 3,
        Customer = 1,
        NoName = 0
    }

}
