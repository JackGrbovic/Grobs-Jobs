namespace Job_Post_Website.Model
{
    public class Inbox
    {
        public User user { get; set; }

        public List<Message> messages { get; set; }

        public Inbox(User user) 
        {
            this.user = user;
        }  
    }
}
