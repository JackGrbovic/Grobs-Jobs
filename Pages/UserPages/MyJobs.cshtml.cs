using Job_Post_Website.CustomServices;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.UserPages
{
    public class MyJobsModel : PageModel
    {
        private JobPostDbContext _jobPostDbContext { get; set; }

        public AspNetUser _user { get; set; }

        private UserHandler _userHandler { get; set; }

        public List<Job> _jobs { get; set; }

        public bool UserIsAuthenticated;

        public MyJobsModel(JobPostDbContext jobPostDbContext, UserHandler userHandler) 
        { 
            _jobPostDbContext = jobPostDbContext;
            _userHandler = userHandler;
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                UserIsAuthenticated = false;
            }

            else 
            {
                UserIsAuthenticated = true;
                _user = _userHandler.ReturnFirstPartyUser(User);
                _jobs = _jobPostDbContext.Jobs.Where(j => j.JobPosterId == _user.Id).OrderBy(j => j.DateTimePosted).ToList();
                _jobs.Reverse();
            }
        }
    }
}
