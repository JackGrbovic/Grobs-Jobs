using Job_Post_Website.CustomServices;
using Job_Post_Website.Data;
using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Job_Post_Website.Pages.UserPages
{
    public class InboxModel : PageModel
    {
        public JobPostDbContext _jobPostDbContext { get; set; }

        public MessageDbContext _messageDbContext { get; set; }

        public MessageHandler _messageHandler { get; set; }

        public UserHandler _getUser { get; set; }

        public AspNetUser _user { get; set; }

        public List<AspNetUser> _messagedUsers { get; set; }

        public List<Message> _inbox { get; set; }

        public List<Message> _previewMessages { get; set; }

        public List<string> _normalizedUserNamesWithoutLoggedInUser = new List<string>();

        public List<string> _idsWithoutLoggedInUser = new List<string>();

        public List<string> _normalizedUserNamesWithLoggedInUser = new List<string>();

        public Dictionary<string, Message> PreviewMessagesAndNames = new Dictionary<string, Message>();

        public InboxModel(MessageHandler messageHandler, UserHandler getUser, JobPostDbContext jobPostDbContext, MessageDbContext messageDbContext)
        {
            _messageHandler = messageHandler;
            _getUser = getUser;
            _jobPostDbContext = jobPostDbContext;
            _messageDbContext = messageDbContext;
        }

        public void OnGet()
        {
            _user = _getUser.ReturnFirstPartyUser(User);
            _inbox = _messageHandler.GetUserInbox(_user);
            _messagedUsers = _messageHandler.GetUsersFromMessagesInInbox(_inbox, _user);
            _previewMessages = _messageHandler.GetPreviewMessages(_messagedUsers, _user);

            //we have a list of preview messages
            //each of these mesages has a messageSender and a messageRecipient
            //we want to find the name of the user associated with the message who is not the logged in user

            _normalizedUserNamesWithoutLoggedInUser = _messageHandler.GetNormalizedUserNameSansLoggedInUserSenderOrRecipient(_previewMessages, _user);
            _normalizedUserNamesWithLoggedInUser = _messageHandler.GetNormalizedUserNameWithLoggedInUserOnlySender(_previewMessages);
            _idsWithoutLoggedInUser = _messageHandler.GetIdsSansLoggedInUserSenderOrRecipient(_previewMessages, _user);
        }
    }
}
