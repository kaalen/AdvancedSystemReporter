using System;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Data.Validators;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
  public class ValidationErrors : BaseFilter
  {
    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item != null)
      {
        ValidatorCollection validators = ValidatorManager.BuildValidators(Mode, item);
        ValidatorManager.Validate(validators, new ValidatorOptions(false));
        foreach (BaseValidator validator in validators)
        {
          if (validator.Result >= MinErrorLevel)
          {
            return true;
          }
        }
      }
      return false;
    }

    public ValidatorResult MinErrorLevel
    {
      //get
      //{
      //  string value = base.GetParameter("MinErrorLevel");
      //  return (ValidatorResult)Enum.Parse(typeof(ValidatorResult), value);
      //}
      get; set;
    }

    public ValidatorsMode Mode
    {
      //get
      //{
      //  string value = this.GetParameter("Mode");
      //  return (ValidatorsMode)Enum.Parse(typeof(ValidatorsMode), value);
      //}
      get; set;
    }
  }
}