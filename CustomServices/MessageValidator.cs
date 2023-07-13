using FluentValidation;
using Job_Post_Website.Model;

namespace Job_Post_Website.CustomServices
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator() {

            RuleFor(m => m.MessageSender).NotEmpty().WithMessage("Sender is required.");
            RuleFor(m => m.MessageRecipient).NotEmpty().WithMessage("Recipient is required.");
            RuleFor(m => m.MessageSenderUserName).NotEmpty().WithMessage("Sender is required.");
            RuleFor(m => m.MessageRecipientUserName).NotEmpty().WithMessage("Recipient is required.");
            RuleFor(m => m.MessageTitle).NotEmpty().WithMessage("Title is required.");
            RuleFor(m => m.MessageBody).NotEmpty().WithMessage("Body is required.");
        }
    }
}
