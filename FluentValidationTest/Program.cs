using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
//using System.Range;//c# 8, not in 7.3

namespace FluentValidationTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//var uri = new Uri("https://reactblobtests.blob.core.windows.net:443/tracking-files/tfpHermesUK/testTrackingFile_000002.csv");
   //         //var sasToken = responseArray[0].SaSToke;
			//var segments2Onwards = string.Concat(uri.Segments[2..]); //c#8 not in 7.3

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

            //new TestOrganisationValidation().ValidateWithNull();

            new PropertyLevelUnitTests().Should_validate_length();


			Console.ReadLine();

		}
	}
}
