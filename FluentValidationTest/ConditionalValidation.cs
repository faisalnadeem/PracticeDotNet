using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace FluentValidationTest
{
    public class Organisation
    {
        public string Name { get; set; }
    }

    public class OrganisationValidator : AbstractValidator<Organisation>
    {
        public OrganisationValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(50);
        }

        protected override bool PreValidate(ValidationContext<Organisation> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null) return true;

            result.Errors.Add(new ValidationFailure("", "org is null"));
            return base.PreValidate(context, result);
        }
    }


    public class TestOrganisationValidation
    {
        public void ValidateWithNull()
        {
            var validator = new OrganisationValidator();
            Organisation organisation = new Organisation();
            var result = validator.Validate(organisation);
            // result.Errors[0].ErrorMessage == "org is null";
        }
    }
 }
