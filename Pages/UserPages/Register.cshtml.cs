using Job_Post_Website.CustomServices;
using Job_Post_Website.ScaffoldedModels;
using Job_Post_Website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.UserPages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Register RegisterViewModel {  get; set; }

        public UserManager<IdentityUser> userManager { get; }
        
        public SignInManager<IdentityUser> signInManager { get; }

        private UserHandler _userHandler { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UserHandler userHandler)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _userHandler = userHandler;
            //create method to validate stuff in user handler
        }

        public async Task<IActionResult> OnPostAsync() 
        { 
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    Email = RegisterViewModel.Email,
                    NormalizedEmail = RegisterViewModel.Email,
                    UserName = RegisterViewModel.DisplayName,
                    EmailConfirmed = true
                    //sort out display name stuff to make create job valid
                };

                bool IsUserNameUnique;
                bool IsEmailUnique;

                IsUserNameUnique = _userHandler.IsRegistrationStringValid(user.UserName);
                IsEmailUnique = _userHandler.IsRegistrationStringValid(user.Email);

                string userNameTakenError = "Sorry, but the UserName you have provided is already taken. Please choose a different UserName";
                string emailTakenError = "Sorry, but the Email you have provided is already taken. Please choose a different Email";

                if (IsUserNameUnique == false)
                {
                    string errorMessage = userNameTakenError;
                    return RedirectToPage("/UserPages/ErrorMessage", new { errorMessage });
                }

                if (IsEmailUnique == false)
                {
                    string errorMessage = emailTakenError;
                    return RedirectToPage("/UserPages/ErrorMessage", new { errorMessage });
                }

                var result = await userManager.CreateAsync(user, RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
