using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using DataLayer.Models;

namespace DataLayer.CustomValidator
{
    public class UniqueEmailValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value as string;
            if (!string.IsNullOrEmpty(email))
            {
                var context = new NavasthalaContext();
                if (ClaimsPrincipal.Current.Identity.IsAuthenticated)
                {
                    var userName = ClaimsPrincipal.Current.Identity.Name;
                    return !context.UserProfiles.Where(p => !p.UserName.ToLower().Equals(userName.ToLower())).Any(p => p.Email.ToLower().Equals(email.ToLower()));
                }
                return !context.UserProfiles.Any(p => p.Email.ToLower().Equals(email.ToLower()));
            }
            return true;
        }
    }
}
