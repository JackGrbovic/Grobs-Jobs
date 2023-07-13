using Job_Post_Website.ScaffoldedModels;
using Nest;
using System.Buffers.Text;
using System.Security.AccessControl;
using Job = Job_Post_Website.ScaffoldedModels.Job;

namespace Job_Post_Website.CustomServices
{
    public class SearchHandler
    {
        public List<Job> _searchResults = new List<Job>();

        public JobPostDbContext _jobPostDbContext { get; set; }

        public SearchHandler(JobPostDbContext jobPostDbContext)
        {
            _jobPostDbContext = jobPostDbContext;
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
        }

        public List<Job> GetJobFromSearchQueryString(string queryString)
        {
            return _jobPostDbContext.Jobs.Where(
                x => x.Name.Contains(queryString) || 
                x.Description.Contains(queryString) || 
                x.JobPosterNormalizedUserName.Contains(queryString))
                .ToList();
        }
    }
}
