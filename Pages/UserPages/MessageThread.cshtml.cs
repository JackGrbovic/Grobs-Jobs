using FluentValidation;
using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace Job_Post_Website.Pages.UserPages
{
    public class MessageThreadModel : PageModel
    {
        public AspNetUser _firstParty { get; set; }
        public AspNetUser _secondParty { get; set; }

        public List<Message> _firstPartyMessages { get; set; }
        public List<Message> _sharedMessages { get; set; }

        [BindProperty]
        public Message _messageToSend { get; set; }

        private IValidator<Message> _messageValidator { get; set; }

        public UserHandler _getUser { get; set; }

        MessageDbContext _messageDbContext { get; set; }

        public MessageHandler _messageHandler { get; set; }

        public MessageThreadModel(MessageHandler messageHandler, UserHandler getUser, MessageDbContext messageDbContext, IValidator<Message> messageValidator) 
        {
            _messageHandler = messageHandler;
            _getUser = getUser;
            _messageDbContext = messageDbContext;
            _messageValidator = messageValidator;
        }

        public void OnGet(string secondPartyId)
        {
            _firstParty = _getUser.ReturnFirstPartyUser(User);
            _secondParty = _getUser.ReturnSecondPartyUserById(secondPartyId);
            _firstPartyMessages = _messageHandler.GetUserInbox(_firstParty);
            _sharedMessages = _messageHandler.GetMessageThreadOfTwoUsers(secondPartyId, _firstPartyMessages);
        }

        public async Task<IActionResult> OnPost(string secondPartyId)
        {
            //put together problem details as necessary
            _firstParty = _getUser.ReturnFirstPartyUser(User);
            _secondParty = _getUser.ReturnSecondPartyUserById(secondPartyId);
            _firstPartyMessages = _messageHandler.GetUserInbox(_firstParty);
            _sharedMessages = _messageHandler.GetMessageThreadOfTwoUsers(secondPartyId, _firstPartyMessages);

            _messageToSend.MessageTitle = _sharedMessages[0].MessageTitle;
            _messageToSend.MessageRecipient = _secondParty.Id;
            _messageToSend.MessageSender = _firstParty.Id;
            _messageToSend.MessageRecipientUserName = _secondParty.NormalizedUserName;
            _messageToSend.MessageSenderUserName = _firstParty.NormalizedUserName;
            _messageToSend.DateTimeSent = DateTime.Now;

            var messageValidation = _messageValidator.Validate(_messageToSend);

            if (messageValidation.IsValid)
            {
                await _messageHandler.SendMessage(_messageDbContext, _messageToSend);
                return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
            }
            //this is where we can put an error message
            return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
        }
    }
}
