namespace SimpleEcommerce.Api.Domain.Users
{
    public class Address : Entity
    {
        public string Phone { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
