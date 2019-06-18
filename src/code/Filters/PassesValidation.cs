using System;
using Sitecore.Data.Validators;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	/// <summary>
	/// Filters all items which do not pass validation rules.
	/// </summary>
    public class PassesValidation : Sitecore.Feature.Reports.Interface.BaseFilter
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
                    if (validator.Result != ValidatorResult.Valid)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public ValidatorsMode Mode
        {
            get
            {
                string value = this.GetParameter("Mode");
                return (ValidatorsMode)Enum.Parse(typeof(ValidatorsMode), value);
            }
        }
    }
}
