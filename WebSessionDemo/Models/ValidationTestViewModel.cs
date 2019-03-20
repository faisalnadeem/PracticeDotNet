using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace WebSessionDemo.Models
{
	[Serializable, Validator(typeof(ValidationModelValidator))]
	public class ValidationTestViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Comments { get; set; }
	}

	public class ValidationModelValidator : AbstractValidator<ValidationTestViewModel>
	{
		public ValidationModelValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(ValidationRules.UsernameEmpty)
				.Matches(ValidationRules.UsernameValidRegexp)
				.WithMessage(ValidationRules.UsernameNotValid);
		}

	}

	public class ValidationRules
	{
		public static string UsernameEmpty => "usere empty validation rule";
		public static string UsernameValidRegexp => "username validation regex";
		public static string UsernameNotValid => "user not valid";
	}
}