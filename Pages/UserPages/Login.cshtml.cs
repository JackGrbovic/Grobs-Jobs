using Job_Post_Website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.UserPages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public Login LoginViewModel { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync(/*string returnUrl = null*/)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LoginViewModel.UserName, LoginViewModel.Password, LoginViewModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    //if(returnUrl == null || returnUrl == string.Empty || returnUrl == "/") 
                    //{
                        return RedirectToPage("/Index");
                    
                    //else
                    //{
                    //    return RedirectToPage(returnUrl);
                    //}
                }
                 ModelState.AddModelError("", "Username or Password is incorrect.");
            }

            return Page();
        }
    }
}
