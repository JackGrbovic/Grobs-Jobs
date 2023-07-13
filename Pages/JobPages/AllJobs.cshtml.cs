using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.JobPages
{
    public class AllJobsModel : PageModel
    {
        public AllJobsModel(JobPostDbContext jobPostDbContext) 
        {
            
        }

        public void OnGet()
        {
        }
    }
}
