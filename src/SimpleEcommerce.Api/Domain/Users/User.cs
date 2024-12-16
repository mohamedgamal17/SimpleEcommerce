using SimpleEcommerce.Api.Domain.Media;

namespace SimpleEcommerce.Api.Domain.Users
{
    public class User : Entity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int? AvatarId { get; set; }
        public List<Address> Addresses { get; set; }
        public Picture Avatar { get; set; }
        private User()
        {
        }
        public User(string id)
        {
            Id = id;
        }      
    }
}
