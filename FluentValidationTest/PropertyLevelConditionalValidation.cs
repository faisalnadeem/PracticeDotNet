using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NUnit.Framework;


namespace FluentValidationTest
{
    public class PropertyLevelUnitTests
    {
        public void Should_validate_length()
        {
            var ok1 = new Model { MyProperty = null };
            var ok2 = new Model { MyProperty = "" };
            var ok3 = new Model { MyProperty = "55555" };
            var fail = new Model { MyProperty = "1" };

            var v = new ModelValidator();
            var result = v.Validate(ok1);
            result = v.Validate(ok2);
            result = v.Validate(ok3);
            result = v.Validate(fail);

            //var result = v.ValidateAndThrow(ok1);
            v.ValidateAndThrow(ok2);
            v.ValidateAndThrow(ok3);

            //this should fail
            v.ValidateAndThrow(fail);

        }

        public class Model
        {
            public string MyProperty { get; set; }
        }

        public class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor(x => x.MyProperty)
                    .Length(5, 10)
                    .When((model, prop) => !string.IsNullOrEmpty(prop.ToString()));
            }
        }
    }

    public static class Extensions
    {
        //public static IRuleBuilderOptions<T, string> MustHaveLengthBetween<T>(this IRuleBuilder<T, string> rule, int min, int max)
        //{
        //    return rule
        //        .Length(min, max).WithMessage("AGAGA")
        //        .When((model, prop) => !string.IsNullOrEmpty(prop));
        //}

        public static IRuleBuilderOptions<T, TProperty> When<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Func<T, TProperty, bool> predicate, ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            return rule.Configure(config =>
            {
                config.ApplyCondition(ctx => predicate((T)ctx.Instance, (TProperty)ctx.PropertyValue), applyConditionTo);
            });
        }
    }
}
