using Sitecore.Data.Fields;

namespace Sitecore.Feature.Reports.Fields
{
	/// <summary>
	/// Implements a custom field which is used to represent users.
	/// </summary>
	public class User : CustomField
	{
		public User(Field innerField)
			: base(innerField)
		{ }

		private Sitecore.Security.Accounts.User _userProfile;
		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <value>The user.</value>
		private Sitecore.Security.Accounts.User UserProfile
		{
			get
			{
				if (_userProfile == null && InnerField.HasValue && !string.IsNullOrEmpty(InnerField.Value))
				{
					_userProfile = Sitecore.Security.Accounts.User.FromName(InnerField.Value, false);
				}
				return _userProfile;
			}
		}

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>The full name.</value>
		public string FullName
		{
			get
			{
				if (UserProfile != null)
				{
					return UserProfile.Profile.FullName;
				}
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the business unit.
		/// </summary>
		/// <value>The business unit.</value>
		public string OrganisationalUnit
		{
			get
			{
				if (UserProfile != null)
				{
					return UserProfile.Profile.GetCustomProperty("Business unit");
				}
				return string.Empty;
			}
		}
	}
}
