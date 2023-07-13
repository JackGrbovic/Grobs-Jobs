using Job_Post_Website.Data;
using Job_Post_Website.Model;
using Job_Post_Website.ScaffoldedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Post_Website.CustomServices
{
    public class MessageHandler
    {
        public MessageDbContext _messageDb { get; set; }

        public JobPostDbContext _jobPostDb { get; set; }

        public MessageHandler(MessageDbContext messageDb, JobPostDbContext jobPostDbContext)
        {
            _messageDb = messageDb;
            _jobPostDb = jobPostDbContext;
        }

        public Message CreateMessage(string messageSender, string messageRecipient, string messageTitle, string messageBody)
        {
            Message message = new Message();
            message.MessageSender = messageSender;
            message.MessageRecipient = messageRecipient;
            message.MessageTitle = messageTitle;
            message.MessageBody = messageBody;
            message.DateTimeSent = DateTime.Now;
            return message;
        }

        public async Task SendMessage(MessageDbContext context, Message messageToSend)
        {
            await context.Message.AddAsync(messageToSend);
            await context.SaveChangesAsync();
        }

        public List<Message> GetUserInbox(AspNetUser user)
        {
            List<Message> messages = new List<Message>();

            var receivedMessages = _messageDb.Message.Where(x => x.MessageRecipient == user.Id).ToList();
            var sentMessages = _messageDb.Message.Where(x => x.MessageSender == user.Id).ToList();

            messages.AddRange(receivedMessages);
            messages.AddRange(sentMessages);
            return messages;
        }

        public int GetUserInboxCount(AspNetUser recipient)
        {
            var messageCollection = _messageDb.Message.Where(x => x.MessageRecipient == recipient.UserName).ToList();
            int messageCount = messageCollection.Count();
            return messageCount;
        }

        public List<Message> GetUserOutbox(AspNetUser sender)
        {
            var messageCollection = _messageDb.Message.Where(x => x.MessageSender == sender.UserName).ToList();
            return messageCollection;
        }

        public List<Message> GetMessageThreadOfTwoUsers(string secondPartyId, List<Message> firstPartyMessages)
        {
            List<Message> listToReturn = new List<Message>();

            var messagesByRecipient = firstPartyMessages.Where(m => m.MessageRecipient == secondPartyId).ToList();
            var messagesBySender = firstPartyMessages.Where(m => m.MessageSender == secondPartyId).ToList();

            //this method needs to return the messages of the other user as well

            listToReturn.AddRange(messagesByRecipient);
            listToReturn.AddRange(messagesBySender);
            listToReturn.OrderBy(m => m.DateTimeSent).ToList();
            listToReturn.Reverse();

            return listToReturn;
        }

        public List<string> GetNormalizedUserNameSansLoggedInUserSenderOrRecipient(List<Message> previewMessages, AspNetUser loggedInUser)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                if (message.MessageSender != loggedInUser.Id)
                {
                    string normalizedUserName = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageSender).FirstOrDefault().NormalizedUserName;
                    valuesToReturn.Add(normalizedUserName);
                }
                else
                {
                    string normalizedUserName = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().NormalizedUserName;
                    valuesToReturn.Add(normalizedUserName);
                }
            }
            return valuesToReturn;
        }

        public List<string> GetIdsSansLoggedInUserSenderOrRecipient(List<Message> previewMessages, AspNetUser loggedInUser)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                if (message.MessageSender != loggedInUser.Id)
                {
                    string normalizedUserName = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageSender).FirstOrDefault().Id;
                    valuesToReturn.Add(normalizedUserName);
                }
                else
                {
                    string normalizedUserName = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().Id;
                    valuesToReturn.Add(normalizedUserName);
                }
            }
           return valuesToReturn;
        }

        public List<string> GetNormalizedUserNameWithLoggedInUserOnlySender(List<Message> previewMessages)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                string normalizedUserName = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().NormalizedUserName;
                valuesToReturn.Add(normalizedUserName);
            }
           return valuesToReturn;
        }

        public List<AspNetUser> GetUsersFromMessagesInInbox(List<Message> inbox, AspNetUser user)
        {
            List<AspNetUser> userList = new List<AspNetUser>();
            AspNetUser userToAdd = new AspNetUser();

            //we want to return every user that has been involved on one user's inbox, including the loggedinuser

            foreach (Message message in inbox)
            {
                var messageSender = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageSender).FirstOrDefault();
                var messageRecipient = _jobPostDb.AspNetUsers.Where(u => u.Id == message.MessageRecipient).FirstOrDefault();

                bool messageSenderAlreadyInList = false;
                bool messageRecipientAlreadyInList = false;

                if (userList.Count == 0)
                {
                    userList.Add(messageSender);
                    userList.Add(messageRecipient);
                }

                else
                {
                    if (userList.Contains(messageSender))
                    {
                        messageSenderAlreadyInList = true;
                    }
                    if (userList.Contains(messageRecipient))
                    {
                        messageRecipientAlreadyInList = true;
                    }

                    if (messageSenderAlreadyInList == false && messageSender.Id != null)
                    {
                        userList.Add(messageSender);
                    }
                    if (messageRecipientAlreadyInList == false && messageRecipient.Id != null)
                    {
                        userList.Add(messageRecipient);
                    }
                }
            }
            return userList;
        }

        public Message ShortenMessage(Message messageToShorten)
        {
            Message messageToReturn;
            messageToReturn = messageToShorten;
            string shortenedMessage = "";
            if (messageToShorten.MessageBody.Length > 100)
            {
                for (int i = 0; i < 100; i++)
                {
                    shortenedMessage += messageToShorten.MessageBody[i];
                }
                shortenedMessage += "...";
                messageToReturn.MessageBody = shortenedMessage;
            }
            return messageToReturn;
        }

        public List<Message> GetPreviewMessages(List<AspNetUser> messagedUsers, AspNetUser loggedInUser)
        {
            //List<Message> latestMessages = new List<Message>();
            //List<string> recipients = messageDbContext.Message.Where(x => x.MessageSender == loggedInUser.Id).Select(x => x.MessageRecipient).Distinct().ToList();

            //foreach(string recipient in recipients)
            //{
            //    latestMessages.Add(messageDbContext.Message.Where(x => x.MessageRecipient == recipient).OrderByDescending(x => x.DateTimeSent).First());
            //}

            //return latestMessages;


            List<Message> previewMessages = new List<Message>();

            for (int i = 0; i < messagedUsers.Count; i++)
            {
                var mostRecentMessageFromLoggedInUser = _messageDb.Message
                                    .Where(x => x.MessageSender == loggedInUser.Id)
                                    .OrderByDescending(x => x.DateTimeSent)
                                    .FirstOrDefault();

                var mostRecentMessageFromOtherUser = _messageDb.Message
                                    .Where(x => x.MessageSender == messagedUsers[i].Id)
                                    .OrderByDescending(x => x.DateTimeSent)
                                    .FirstOrDefault();

                if (mostRecentMessageFromLoggedInUser != null && mostRecentMessageFromOtherUser != null)
                {
                    if (mostRecentMessageFromLoggedInUser.DateTimeSent > mostRecentMessageFromOtherUser.DateTimeSent)
                    {
                        ShortenMessage(mostRecentMessageFromLoggedInUser);
                        previewMessages.Add(mostRecentMessageFromLoggedInUser);
                    }
                    else if (mostRecentMessageFromLoggedInUser.DateTimeSent < mostRecentMessageFromOtherUser.DateTimeSent)
                    {
                        ShortenMessage(mostRecentMessageFromOtherUser);
                        previewMessages.Add(mostRecentMessageFromOtherUser);
                    }
                }

                else if (mostRecentMessageFromLoggedInUser != null && mostRecentMessageFromOtherUser == null)
                {
                    ShortenMessage(mostRecentMessageFromLoggedInUser);
                    previewMessages.Add(mostRecentMessageFromLoggedInUser);
                }

                else if (mostRecentMessageFromOtherUser != null && mostRecentMessageFromLoggedInUser == null)
                {
                    ShortenMessage(mostRecentMessageFromOtherUser);
                    previewMessages.Add(mostRecentMessageFromOtherUser);
                }
            }
            return previewMessages;
        }
    }
}
