using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.Data.Items;
  using Sitecore.Data.Validators;
  using Sitecore.Feature.Reports.Interface;

  public class ValidationViewer : BaseViewer
  {
    public override void Display(DisplayElement displayElement)
    {
      Item item = displayElement.Element as Item;
      if (item != null)
      {
        ValidatorCollection validators = ValidatorManager.BuildValidators(Mode, item);
        ValidatorManager.Validate(validators, new ValidatorOptions(false));
        List<string> validationErrors = new List<string>();
        foreach (BaseValidator validator in validators)
        {
          if (validator.Result >= MinErrorLevel)
          {
            validationErrors.Add(validator.Text);
          }
        }
        var text = validationErrors.Aggregate("", (current, validationError) => current + (validationError + "<br/>"));
        displayElement.AddColumn("ValidationResult", text);
      }
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

    public ValidatorResult MinErrorLevel
    {
      //get
      //{
      //  string value = base.GetParameter("MinErrorLevel");
      //  return (ValidatorResult)Enum.Parse(typeof(ValidatorResult), value);
      //}
      get; set;
    }

  }
}