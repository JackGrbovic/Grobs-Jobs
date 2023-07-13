using Job_Post_Website.CustomServices;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages
{
    public class IndexModel : PageModel
    {
        public bool isLoggedIn;

        private readonly JobPostDbContext _jobPostDbContext;
        
        private UserHandler _userHandler { get ; set; } 

        public IEnumerable<Job> Jobs { get; set; }

        public IndexModel(JobPostDbContext jobPostDbContext, UserHandler userHandler)
        {
            _jobPostDbContext = jobPostDbContext;
            _userHandler = userHandler;
        }

        public void OnGet()
        {
            Jobs = _jobPostDbContext.Jobs.OrderByDescending(x => x.DateTimePosted);
            isLoggedIn = _userHandler.IsUserLoggedIn(User);
        }
    }
}