using FluentValidation;
using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Job_Post_Website.Pages.UserPages
{
    public class StartMessageThreadModel : PageModel
    {
        //the value of _firstParty is being correctly attributed and then is later being set to null for some reason
        public AspNetUser _firstParty { get ; set; }
        public AspNetUser _secondParty = new AspNetUser();

        [BindProperty]
        public Message _messageToSend { get; set; }

        public string JobName = "";

        public string RecipientName = "";

        public UserHandler _getUser { get; set; }

        public List<Message> _inbox { get; set; }

        private MessageDbContext _messageDbContext { get; set; }

        private IValidator<Message> _messageValidator { get; set; }

        private JobPostDbContext _jobPostDbContext { get; set; }

        public MessageHandler _messageHandler { get; set; }

        public StartMessageThreadModel(MessageHandler messageHandler, UserHandler getUser, MessageDbContext messageDbContext, JobPostDbContext jobPostDbContext, IValidator<Message> messageValidator)
        {
            _messageHandler = messageHandler;
            _getUser = getUser;
            _messageDbContext = messageDbContext;
            _jobPostDbContext = jobPostDbContext;
            _messageValidator = messageValidator;
        }

        //this has been set to empty because there could be a message thread started without a preselected recipient (the recipient is added retrospectively)
        public void OnGet(string recipientNormalizedUserName = "Empty", string jobName = "Empty")
        {
            _firstParty = _getUser.ReturnFirstPartyUser(User);
            if (recipientNormalizedUserName != "Empty")
            {
                RecipientName = recipientNormalizedUserName;
            }
            if (jobName != "Empty")
            {
                JobName = jobName;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            _firstParty = _getUser.ReturnFirstPartyUser(User);

            var problemDetails = new ProblemDetails
            {
                Status = 404,
                Title = "An error occurred",
                Detail = "Recipient's email is either incorrect or does not exist. Reload the page and try again.",
                Instance = HttpContext.Request.Path
            };

            _secondParty = _getUser.ReturnSecondPartyUserByNormalizedUserName(_messageToSend.MessageRecipient);

            if (_secondParty.Id == null)
            {
                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            _messageToSend.MessageRecipient = _secondParty.Id;
            _messageToSend.MessageSender = _firstParty.Id;
            _messageToSend.MessageRecipientUserName = _secondParty.NormalizedUserName;
            _messageToSend.MessageSenderUserName = _firstParty.NormalizedUserName;
            _messageToSend.DateTimeSent = DateTime.Now;

            var validationResult = await _messageValidator.ValidateAsync(_messageToSend);

            if (validationResult.IsValid)
            {
                string secondPartyId = _secondParty.Id;
                _messageHandler.SendMessage(_messageDbContext, _messageToSend);
                //instead of returning the same page we need to return the message thread
                return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
            }
            //this return statement needs to take us to a page that just says "Message Sent"
            string errorMessage = "User not found. Please enter a valid UserName";
            return RedirectToPage("/UserPages/ErrorMessage", new { errorMessage });
        }
    }
}
