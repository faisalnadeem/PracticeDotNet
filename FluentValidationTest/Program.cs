using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace FluentValidationTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Customer customer = new Customer()
			{
				Email = "email@test.co",
				ConfirmEmail = "eMail@test.co",
				Address = new Address()
			};
			CustomerValidator validator = new CustomerValidator();

			ValidationResult results = validator.Validate(customer);

			if (!results.IsValid)
			{
				foreach (var failure in results.Errors)
				{
					Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
				}
			}

			string allMessages = results.ToString("~");
			Console.WriteLine(allMessages);


			Console.ReadLine();

		}
	}
}
