namespace Ordering.Domain.Models
{
    public record Address
    {
        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string? EmailAddress { get; } = default!;
        public string AddressLine { get; } = default!;
        public string Country { get; } = default!;
        public string City { get; } = default!;
        public string State { get; } = default!;
        public string ZipCode { get; } = default!;

        protected Address() { }
        private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            AddressLine = addressLine;
            Country = country;
            City = city;
            State = state;
            ZipCode = zipCode;

        }
        public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string city, string state, string zipCode)
        {
            ArgumentNullException.ThrowIfNull(firstName);
            ArgumentNullException.ThrowIfNull(lastName);
            ArgumentNullException.ThrowIfNull(addressLine);
            ArgumentNullException.ThrowIfNull(country);
            ArgumentNullException.ThrowIfNull(city);
            ArgumentNullException.ThrowIfNull(state);
            ArgumentNullException.ThrowIfNull(zipCode);
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("FirstName cannot be empty or whitespace.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("LastName cannot be empty or whitespace.", nameof(lastName));
            }
            if (string.IsNullOrWhiteSpace(addressLine))
            {
                throw new ArgumentException("AddressLine cannot be empty or whitespace.", nameof(addressLine));
            }
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be empty or whitespace.", nameof(country));
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be empty or whitespace.", nameof(city));
            }
            if (string.IsNullOrWhiteSpace(state))
            {
                throw new ArgumentException("State cannot be empty or whitespace.", nameof(state));
            }
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                throw new ArgumentException("ZipCode cannot be empty or whitespace.", nameof(zipCode));
            }
            return new Address(firstName, lastName, emailAddress, addressLine, country, city, state, zipCode);
        }
    }
}
