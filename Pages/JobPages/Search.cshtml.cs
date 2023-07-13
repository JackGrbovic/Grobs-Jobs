using Job_Post_Website.CustomServices;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.JobPages
{
    [ValidateAntiForgeryToken]
    public class SearchModel : PageModel
    {
        private readonly SearchHandler _searchHandler;

        public bool noResults = false;

        public List<Job> jobResults = new List<Job>();

        public SearchModel(SearchHandler searchHandler)
        {
            _searchHandler = searchHandler;
        }

        public IActionResult OnPost(string queryString)
        {
            jobResults = _searchHandler.GetJobFromSearchQueryString(queryString);

            if (jobResults.Any())
            {
                return Page();
            }
            else
            {
                noResults = true;
                return Page();
            }
        }
    }
}
