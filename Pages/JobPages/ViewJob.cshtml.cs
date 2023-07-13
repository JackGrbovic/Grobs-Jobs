using Job_Post_Website.CustomServices;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Job_Post_Website.Pages.JobPages
{
    public class ViewJobModel : PageModel
    {
        private readonly JobPostDbContext _db;

        [BindProperty]
        public Job _job { get; set; }

        public AspNetUser _loggedInUser { get; set; }

        private UserHandler _userGetter {  get; set; }

        public bool AuthorizedToEdit { get; set; }

        public bool AuthenticatedToMessageJobPoster { get; set; }

        public ViewJobModel(JobPostDbContext db, UserHandler userGetter)
        {
            _db = db;
            _userGetter = userGetter;
        }

        public void OnGet(int id)
        {
            _job = _db.Jobs.Find(id);

            bool userExists = _userGetter.ValidateClaimsPriciple(User);

            if (userExists)
            {
                _loggedInUser = _userGetter.ReturnFirstPartyUser(User);

                if (_job.JobPosterId == _loggedInUser.Id)
                {
                    AuthorizedToEdit = true;
                    AuthenticatedToMessageJobPoster = false;
                }
                else if (_job.JobPosterId != _loggedInUser.Id)
                {
                    AuthorizedToEdit = false;
                    AuthenticatedToMessageJobPoster = true;
                }
            }
            else
            {
                AuthorizedToEdit = false;
                AuthenticatedToMessageJobPoster = false;
            }
            
        }

        //We need to pass the id from the chosen job in Create.cshtml, when we link to one of the jobs (see video for how to link and pass the argument)
        //We then need to edit ViewJob.cshtml to display the different aspectsof a job
        //Then we need to create a button that says Edit Job, which will take us to the EditJob page, again passing the id
        //Then we use the same display we did before in creating the job, to utilise the update method and change the data (also returning to the ViewJob page after).
    }
}
