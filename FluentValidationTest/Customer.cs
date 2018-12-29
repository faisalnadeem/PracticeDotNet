using System;
using System.Globalization;
using FluentValidation; 
namespace FluentValidationTest
{
	public class Customer
	{
		public int Id { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string ConfirmEmail { get; set; }
		public string Forename { get; set; }
		public decimal Discount { get; set; }
		public Address Address { get; set; }
	}
	public class Address
	{
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Town { get; set; }
		public string County { get; set; }
		public string Postcode { get; set; }
	}

	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			RuleFor(address => address.Postcode).NotNull();
			//etc
		}
	}


	public class CustomerValidator : AbstractValidator<Customer>
	{
		public CustomerValidator()
		{
			//RuleFor(customer => customer.Surname).NotNull();
			//RuleFor(customer => customer.Address).SetValidator(new AddressValidator());
			//RuleFor(customer => customer.Address.Postcode).NotNull().When(customer => customer.Address != null);
			RuleFor(x => x.ConfirmEmail).Must((model, prop) => BeEqualToEmail(model.Email, model.ConfirmEmail));
		}

		private bool BeEqualToEmail(string email, string confirmEmail)
		{
			return email.Equals(confirmEmail, StringComparison.OrdinalIgnoreCase);
		}
	}
}