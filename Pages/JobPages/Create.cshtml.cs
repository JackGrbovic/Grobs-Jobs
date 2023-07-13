using FluentValidation;
using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;

namespace Job_Post_Website.Pages.JobPages
{
    public class CreateModel : PageModel
    {
        private readonly JobPostDbContext _db;

        [BindProperty]
        public Job _job { get; set; }

        private AspNetUser _AspNetUser { get; set; }

        private UserHandler _userGetter { get; set; }

        private IValidator<Job> _jobValidator { get; set; }

        public CreateModel(JobPostDbContext db, UserHandler userGetter, IValidator<Job> jobValidator)
        {
            _db = db;
            _userGetter = userGetter;
            _jobValidator = jobValidator;
        }

        public async Task<IActionResult> OnPost()
        {
            //if User == null, tell em to log in, will need to preserve their writing too which couild be a challenge
            _AspNetUser = _userGetter.ReturnFirstPartyUser(User);
            _job.DateTimePosted = DateTime.Now;
            _job.JobPosterNormalizedUserName = _AspNetUser.NormalizedUserName;
            _job.JobPosterId = _AspNetUser.Id;

            var validationResult = await _jobValidator.ValidateAsync(_job);

            if (validationResult.IsValid)
            {
                await _db.Jobs.AddAsync(_job);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
