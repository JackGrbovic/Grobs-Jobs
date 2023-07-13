using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Job_Post_Website.CustomServices
{
    public class UserHandler
    {
        public SignInManager<IdentityUser> signInManager { get; set; }

        public JobPostDbContext _dbContext { get; set; }

        public UserHandler(SignInManager<IdentityUser> signInManager, JobPostDbContext dbContext)
        {
            this.signInManager = signInManager;
            _dbContext = dbContext;
        }

        public bool IsUserLoggedIn(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AspNetUser ReturnFirstPartyUser(ClaimsPrincipal user)
        {
            ClaimsPrincipal currentUser = user;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var aspUser = _dbContext.AspNetUsers
                .Single(b => b.Id == currentUserID);
            return aspUser;
        }

        public AspNetUser ReturnSecondPartyUserById(string secondPartyId)
        {
            AspNetUser userToReturn = new AspNetUser();

            var userById = _dbContext.AspNetUsers
                .Single(b => b.Id == secondPartyId);

            if (userById != null)
            {
                userToReturn = userById;
            }
            return userToReturn;
        }

        public AspNetUser ReturnSecondPartyUserByNormalizedUserName(string secondPartyNormalizedUserName)
        {
            secondPartyNormalizedUserName = secondPartyNormalizedUserName.ToUpper();
            AspNetUser userToReturn = new AspNetUser();
       
            var userByNormalizedUserName = _dbContext.AspNetUsers
                .Single(b => b.NormalizedUserName == secondPartyNormalizedUserName);

            if (userByNormalizedUserName != null)
            {
                userToReturn = userByNormalizedUserName;
            }
            return userToReturn;
        }

        public bool ValidateClaimsPriciple(ClaimsPrincipal user)
        {
            if (user.Identity.Name != null)
            {
                return true;
            }
            return false;
        }

        public bool IsRegistrationStringValid(string stringToValidate)
        {
            stringToValidate = stringToValidate.ToUpper();
            List<string> normalizedUserNames = _dbContext.AspNetUsers.Select(u => u.NormalizedUserName).ToList();
            List<string> emailAddresses = _dbContext.AspNetUsers.Select(u => u.Email).ToList();
            if (normalizedUserNames.Contains(stringToValidate))
            {
                return false;
            }
            else if (emailAddresses.Contains(stringToValidate))
            {
                return false;
            }
            return true;
        }
    }
}
