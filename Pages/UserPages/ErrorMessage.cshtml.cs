using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.UserPages
{
    public class ErrorMessageModel : PageModel
    {
        public string _errorMessage;
        public void OnGet(string errorMessage)
        {
            _errorMessage = errorMessage;
        }
    }
}
