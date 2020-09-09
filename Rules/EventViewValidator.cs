using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Validators;
using MiidWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Rules
{
    public class EventViewValidator : AbstractValidator<EventViewModel>
    {
        public EventViewValidator()
        {
            RuleFor(x => x.Event.StartDateTime)
                .GreaterThanOrEqualTo(x => x.DateToCompareAgainst)
                .WithMessage("Invalid start date");
        }
    }
    //public class GreaterThenOrEqualTo : FluentValidationPropertyValidator
    //{
    //    public GreaterThenOrEqualTo(ModelMetadata metadata,
    //                                ControllerContext controllerContext,
    //                                PropertyRule rule,
    //                                IPropertyValidator validator)
    //        : base(metadata, controllerContext, rule, validator)
    //    {
    //    }

    //    public override IEnumerable<ModelClientValidationRule>
    //                                                    GetClientValidationRules()
    //    {
    //        if (!this.ShouldGenerateClientSideRules())
    //        {
    //            yield break;
    //        }

    //        var validator = Validator as GreaterThanOrEqualValidator;

    //        var errorMessage = new MessageFormatter()
    //            .AppendPropertyName(this.Rule.GetDisplayName())
    //            .BuildMessage(validator.ErrorMessageSource.GetString());

    //        var rule = new ModelClientValidationRule
    //        {
    //            ErrorMessage = errorMessage,
    //            ValidationType = "greaterthanorequaldate"
    //        };
    //        rule.ValidationParameters["other"] =
    //            CompareAttribute.FormatPropertyForClientValidation(
    //                validator.MemberToCompare.Name);
    //        yield return rule;
    //    }
    //}

    //public class LessThanOrEqualToFluentValidationPropertyValidator : FluentValidationPropertyValidator
    //{
    //    public LessThanOrEqualToFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
    //        : base(metadata, controllerContext, rule, validator)
    //    {
    //    }

    //    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    //    {
    //        if (!this.ShouldGenerateClientSideRules())
    //        {
    //            yield break;
    //        }

    //        var validator = Validator as LessThanOrEqualValidator;

    //        var errorMessage = new MessageFormatter()
    //            .AppendPropertyName(this.Rule.GetDisplayName())
    //            .BuildMessage(validator.ErrorMessageSource.GetString());

    //        var rule = new ModelClientValidationRule
    //        {
    //            ErrorMessage = errorMessage,
    //            ValidationType = "lessthanorequaldate"
    //        };
    //        rule.ValidationParameters["other"] = CompareAttribute.FormatPropertyForClientValidation(validator.MemberToCompare.Name);
    //        yield return rule;
    //    }
    //}
}