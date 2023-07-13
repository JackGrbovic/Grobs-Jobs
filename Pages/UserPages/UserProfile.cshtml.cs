using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Job_Post_Website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Job_Post_Website.Pages.UserPages
{
    public class UserProfileModel : PageModel
    {
        public AspNetUser user { get; set; } 

        public UserHandler getUser { get; set; }

        public UserProfileModel(UserHandler getUser)
        {
            this.getUser = getUser;
        }

        public void OnGet()
        {
            user = getUser.ReturnFirstPartyUser(this.User);
        }
    }
}
