using System.ComponentModel.DataAnnotations;

namespace Job_Post_Website.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
