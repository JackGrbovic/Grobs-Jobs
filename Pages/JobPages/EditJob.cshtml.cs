using FluentValidation;
using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Job_Post_Website.Pages.JobPages
{
    public class EditJobModel : PageModel
    {
        private readonly JobPostDbContext _db;

        [BindProperty]
        public Job Job { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsDelete { get; set; }

        IValidator<Job> _jobValidator { get; set; }

        AspNetUser _user { get; set; }

        UserHandler _userHandler { get; set; }

        public EditJobModel(JobPostDbContext db, IValidator<Job> jobValidator, UserHandler userHandler)
        {
            _db = db;
            _jobValidator = jobValidator;
            _userHandler = userHandler;
        }

        public void OnGet(int id)
        {
            Job = _db.Jobs.Find(id);
        }

        public async Task<IActionResult> OnPost(string handler)
        {
            if (handler == "Update")
            {
                IsUpdate = true;
            }
            else if (handler == "Delete")
            {
                IsDelete = true;
            }

            _user = _userHandler.ReturnFirstPartyUser(User);

            Job.JobPosterId = _user.Id;
            Job.JobPosterNormalizedUserName = _user.NormalizedUserName;
            Job.DateTimePosted = DateTime.Now;
            //assign the missing fields of jobposter id, username and datetime
            var validationResult = _jobValidator.Validate(Job);

            if (IsUpdate && validationResult.IsValid)
            {
                _db.Jobs.Update(Job);
                await _db.SaveChangesAsync();
                return RedirectToPage("/JobPages/ViewJob", new { id = Job.Id });
            }
            else if (IsDelete)
            {
                var jobFromDb = _db.Jobs.Where(j => j.Id == Job.Id);
                if (jobFromDb != null) {
                    _db.Jobs.Remove(Job);
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Index");
                }
            }
            return Page();
        }
    }
}
